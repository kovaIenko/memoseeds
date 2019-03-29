using memoseeds.Repositories.Purchase.DataConfig;
using memoseeds.Repositories.Purchase.Requests;

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using System;

namespace memoseeds.Controllers
{
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
            string purchaseRepoPath = System.Environment.CurrentDirectory;
            using (
                StreamReader
                configFile = new StreamReader(purchaseRepoPath + "\\Repositories\\Purchase\\config.json"),
                dataFile = new StreamReader(purchaseRepoPath + "\\Repositories\\Purchase\\data.json"))
            {
                configJSON = configFile.ReadToEnd();
                dataJSON = dataFile.ReadToEnd();
            }
            PaymentController.purchaseConfig = JsonConvert.DeserializeObject<PurchaseConfig>(configJSON);
            PaymentController.countryToPurchases = JsonConvert.DeserializeObject<Dictionary<string, List<PurchaseData>>>(dataJSON);
            setupIds(PaymentController.countryToPurchases);
            setupStripe(PaymentController.purchaseConfig);
        }

        private static void setupIds(Dictionary<string, List<PurchaseData>> d)
        {
            foreach (string key in d.Keys)
            {
                int i = -1;
                foreach (PurchaseData p in d[key])
                {
                    p.Id = key + (++i);
                }
            }
        }
        private static void setupStripe(PurchaseConfig purchaseConfig)
        {
            StripeConfiguration.SetApiKey(purchaseConfig.stripeConfig.secretKey);

            customerService = new CustomerService();
            chargeService = new ChargeService();
        }
        private static PurchaseData findPurchaseData(Dictionary<string, List<PurchaseData>> d, string id)
        {
            foreach (string key in d.Keys)
            {
                foreach (PurchaseData p in d[key])
                {
                    if(p.Id == id)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        [HttpPost("options")]
        public ActionResult provideOptions(UserInfo info)
        {
            string countryRes = info.country ?? purchaseConfig.defaultCountry;

            string puchasesList = JsonConvert.SerializeObject(countryToPurchases[countryRes]);
            ActionResult res = new ContentResult { Content = puchasesList, ContentType = "application/json" };

            return res;
        }

        [HttpPost("checkout")]
        public ActionResult tryCheckout(CheckoutInfo info)
        {
            string id = info.purchaseId ?? purchaseConfig.defaultPurchaseId;
            PurchaseData purchase = findPurchaseData(PaymentController.countryToPurchases, id);

            ActionResult res = null;
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

                string chargeString = JsonConvert.SerializeObject(charge);
                res = new ContentResult { Content = chargeString, ContentType = "application/json" };

            } catch(Exception e)
            {
                res = null;
            }

            return res;
        }
    }
}