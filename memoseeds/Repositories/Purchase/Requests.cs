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

        public CheckoutInfo(
            string purchaseId, 
            string email, 
            string sourceToken
        ) {
            this.purchaseId = purchaseId;
            this.email = email;
            this.sourceToken = sourceToken;
        }
    }
}
