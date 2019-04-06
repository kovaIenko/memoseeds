using memoseeds.Repositories.Purchase.DataConfig;
using memoseeds.Repositories.Purchase.Requests;

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using System;

using memoseeds.Repositories;
using memoseeds.Models.Entities;


namespace memoseeds.Controllers
{
    [Route("purchase")]
    [ApiController]
    public class PaymentController : Controller
    {
        private static PurchaseConfig purchaseConfig = null; 
        private static Dictionary<string, List<PurchaseData>> countryToPurchases = null;
        private static Dictionary<string, PurchaseData> idToPurchase = null;
        private static CustomerService customerService = null;
        private static ChargeService chargeService = null;
        static PaymentController()
        {
            readConfigAndData();
            setupIds(PaymentController.countryToPurchases);
            setupStripe(PaymentController.purchaseConfig);
        }

        private static void readConfigAndData()
        {
            string configJSON = null;
            string dataJSON = null;

            string purchaseRepoPath = Environment.CurrentDirectory;
            char pathDirSep = Path.DirectorySeparatorChar;
            string configPath = String.Format("{0}Repositories{0}Purchase{0}config.json", pathDirSep);
            string dataPath = String.Format("{0}Repositories{0}Purchase{0}data.json", pathDirSep);

            using (
                StreamReader
                configFile = new StreamReader(purchaseRepoPath + configPath),
                dataFile = new StreamReader(purchaseRepoPath + dataPath))
            {
                configJSON = configFile.ReadToEnd();
                dataJSON = dataFile.ReadToEnd();
            }

            PaymentController.purchaseConfig = JsonConvert.DeserializeObject<PurchaseConfig>(configJSON);
            PaymentController.countryToPurchases = JsonConvert.DeserializeObject<Dictionary<string, List<PurchaseData>>>(dataJSON);
        }
        private static void setupIds(Dictionary<string, List<PurchaseData>> countryToPurchases)
        {
            PaymentController.idToPurchase = new Dictionary<string, PurchaseData>();
            foreach (string countryName in countryToPurchases.Keys)
            {
                int i = -1;
                foreach (PurchaseData p in countryToPurchases[countryName])
                {
                    p.id = countryName + (++i);
                    PaymentController.idToPurchase.Add(p.id, p);
                }
            }
        }
        private static void setupStripe(PurchaseConfig purchaseConfig)
        {
            StripeConfiguration.SetApiKey(purchaseConfig.stripeConfig.secretKey);

            customerService = new CustomerService();
            chargeService = new ChargeService();
        }
        private static int getUnitMultiplier(PurchaseData purchaseData)
        {
            return PaymentController.purchaseConfig.currencyToMultiplier[purchaseData.price.currency];
        }

        private IUserRepository userRepository = null;
        public PaymentController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("options")]
        public ActionResult provideOptions(UserInfo info)
        {
            string countryRes = info.country;
            if (
                countryRes == null ||
                countryRes.Length < 1 ||
                !countryToPurchases.ContainsKey(countryRes)
            ) {
                countryRes = purchaseConfig.defaultCountry;
            }

            PurchasesInfo purchasesInfo = new PurchasesInfo(
                countryToPurchases[countryRes],
                purchaseConfig.stripeConfig.publishableKey
            );

            string resInfo = JsonConvert.SerializeObject(purchasesInfo);
            ActionResult res = new ContentResult {
                Content = resInfo,
                ContentType = "application/json"
            };

            return res;
        }

        [HttpPost("checkout")]
        public ActionResult tryCheckout(CheckoutInfo info)
        {
            ActionResult res = null;

            string purchaseId = info.purchaseId;
            if(!PaymentController.idToPurchase.ContainsKey(purchaseId))
            {
                purchaseId = PaymentController.purchaseConfig.defaultPurchaseId;
            }
            PurchaseData purchase = PaymentController.idToPurchase[purchaseId];

            try
            {
                var customer = customerService.Create(new CustomerCreateOptions
                {
                    Email = info.email,
                    SourceToken = info.sourceToken
                });

                var amountMultiplier = getUnitMultiplier(purchase);

                var charge = chargeService.Create(new ChargeCreateOptions
                {
                    Amount = purchase.price.amount*amountMultiplier,
                    Currency = purchase.price.currency,
                    Description = purchase.name,
                    CustomerId = customer.Id
                });

                if (
                    charge != null && 
                    charge.Paid
                ) { 
                    User user = userRepository.GetById(info.userId);
                    user.Credits += purchase.credits;
                    userRepository.Insert(user);
                }

                string chargeString = JsonConvert.SerializeObject(charge);
                res = new ContentResult {
                    Content = chargeString,
                    ContentType = "application/json"
                };
            } catch(Exception e)
            {
                res = new ContentResult {
                    Content = JsonConvert.SerializeObject(e),
                    ContentType = "application/json",
                    StatusCode = 500
                };
            }

            return res;
        }

        [HttpPost("option")]
        public ActionResult provideOption(PurchaseInfo info)
        {
            ActionResult res = null;

            string purchaseId = info.purchaseId;
            if (PaymentController.idToPurchase.ContainsKey(purchaseId))
            {
                PurchaseData purchase = PaymentController.idToPurchase[purchaseId];
                string purchaseString = JsonConvert.SerializeObject(purchase);
                res = new ContentResult {
                    Content = purchaseString,
                    ContentType = "application/json"
                };
            } else
            {
                res = new ContentResult {
                    Content = "No purchase with id: [" + purchaseId + "]",
                    ContentType = "application/json"
                };
            }

            return res;
        }
    }
}