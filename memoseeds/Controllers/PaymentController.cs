using System.Collections.Generic;
using System.IO;
using memoseeds.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace memoseeds.Controllers
{
    [Route("purchase")]
    [ApiController]
    public class PaymentController : Controller
    {
        private static PurchaseConfig purchaseConfig; 
        private static Dictionary<string, List<Purchase>> countryToPurchases = null;
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
            PaymentController.countryToPurchases = JsonConvert.DeserializeObject<Dictionary<string, List<Purchase>>>(dataJSON);
            setupIds(PaymentController.countryToPurchases);
        }
        private static void setupIds(Dictionary<string, List<Purchase>> d)
        {
            foreach (string key in d.Keys)
            {
                int i = -1;
                foreach (Purchase p in d[key])
                {
                    p.Id = key + (++i);
                }
            }
        }

        [HttpPost("options")]
        public ActionResult allPurchases(UserInfo info)
        {
            string countryRes = info.country;

            if(!countryToPurchases.ContainsKey(countryRes))
            {
                countryRes = purchaseConfig.defaultCountry;
            }

            string puchasesList = JsonConvert.SerializeObject(countryToPurchases[countryRes]);
            ActionResult res = new ContentResult { Content = puchasesList, ContentType = "application/json" };

            return res;
        }
    }

    
}