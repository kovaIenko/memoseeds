using memoseeds.Repositories.Purchase.DataConfig;

using System.Collections.Generic;

namespace memoseeds.Repositories.Purchase.Requests
{
    public class UserInfo
    {
        public string country { get; private set; }

        public UserInfo(string country)
        {
            this.country = country;
        }
    }

    public class CheckoutInfo
    {
        public string purchaseId { get; private set; }
        public string email { get; private set; }
        public string sourceToken { get; private set; }
        public long userId { get; private set; }

        public CheckoutInfo(
            string purchaseId, 
            string email, 
            string sourceToken, 
            long userId
        ) {
            this.purchaseId = purchaseId;
            this.email = email;
            this.sourceToken = sourceToken;
            this.userId = userId;      
        }
    }

    public class PurchasesInfo
    {
        public List<PurchaseData> purchases { get; private set; }
        public string publishableKey { get; private set; }

        public PurchasesInfo(
            List<PurchaseData> purchases, 
            string publishableKey
        ) {
            this.purchases = purchases;
            this.publishableKey = publishableKey;
        }
    }

}
