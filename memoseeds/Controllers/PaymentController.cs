using System.Collections.Generic;
using System.IO;
using memoseeds.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace memoseeds.Controllers
{
    [Route("purchase")]
    [ApiController]
    public class PaymentController : ControllerBase
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
        }
    }
   
}