using memoseeds.Repositories.Purchase.DataConfig;
using memoseeds.Repositories.Purchase.Requests;
using memoseeds.Database;

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using System;

using Microsoft.AspNetCore.Authorization;
using memoseeds.Repositories;
using memoseeds.Models.Entities;


namespace memoseeds.Controllers
{
    //[Authorize]
    [Route("purchase")]
    [ApiController]
    public class PaymentController : Controller
    {
        private static PurchaseConfig purchaseConfig = null; 
        private static Dictionary<string, List<PurchaseData>> countryToPurchases = null;
        private static CustomerService customerService = null;
        private static ChargeService chargeService = null;
        static PaymentController()
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

            setupIds(PaymentController.countryToPurchases);
            SetupStripe(PaymentController.purchaseConfig);
        }

        private static void setupIds(Dictionary<string, List<PurchaseData>> d)
        {
            foreach (string key in d.Keys)
            {
                int i = -1;
                foreach (PurchaseData p in d[key])
                {
                    p.id = key + (++i);
                }
            }
        }
        private static void SetupStripe(PurchaseConfig purchaseConfig)
        {
            StripeConfiguration.SetApiKey(purchaseConfig.stripeConfig.secretKey);

            customerService = new CustomerService();
            chargeService = new ChargeService();
        }

        private static PurchaseData FindPurchaseData(Dictionary<string, List<PurchaseData>> d, string id)
        {
            foreach (string key in d.Keys)
            {
                foreach (PurchaseData p in d[key])
                {
                    if(p.id == id)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        private IUserRepository UserRepository;
        public PaymentController(IUserRepository UserRepository)
        {
            this.UserRepository = UserRepository;
        }

        [HttpPost("options")]
        public ActionResult ProvideOptions(UserInfo info)
        {
            string countryRes = info.country;
            if (info.country == null || info.country.Length < 1)
            {
                countryRes = purchaseConfig.defaultCountry;
            }

            PurchasesInfo purchasesInfo = new PurchasesInfo(
                countryToPurchases[countryRes],
                purchaseConfig.stripeConfig.publishableKey
            );

            string resInfo = JsonConvert.SerializeObject(purchasesInfo);
            ActionResult res = new ContentResult { Content = resInfo, ContentType = "application/json" };

            return res;
        }

        [HttpPost("checkout")]
        public ActionResult TryCheckout(CheckoutInfo info)
        {
            string id = info.purchaseId ?? purchaseConfig.defaultPurchaseId;
            PurchaseData purchase = FindPurchaseData(PaymentController.countryToPurchases, id);

            ActionResult response = Unauthorized();
            try
            {
                var customer = customerService.Create(new CustomerCreateOptions
                {
                    Email = info.email,
                    SourceToken = info.sourceToken
                });

                var charge = chargeService.Create(new ChargeCreateOptions
                {
                    Amount = purchase.price.amount,
                    Description = purchase.name,
                    Currency = purchase.price.currency,
                    CustomerId = customer.Id
                });

                if(charge != null)
                {
                    if (charge.Paid)
                    { 
                        User user =  UserRepository.GetById(info.userId);
                        user.Credits += purchase.credits;
                        UserRepository.Insert(user);
                    }

                    string chargeString = JsonConvert.SerializeObject(charge);
                    response = new ContentResult { Content = chargeString, ContentType = "application/json" };
                }

            } catch(Exception e)
            {
                response = Ok(new { e });
            }

            return response;
        }
    }
}