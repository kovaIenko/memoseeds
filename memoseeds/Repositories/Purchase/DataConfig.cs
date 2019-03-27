namespace memoseeds.Repositories.Purchase.DataConfig
{
    public class PurchaseConfig
    {
        public string defaultCountry { get; set; }
        public string defaultPurchaseId { get; set; }
    }

    public class Price
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
    }

    public class PurchaseData
    {
        public int Credits { get; set; }
        public string Name { get; set; }
        public Price Price { get; set; }
        public string Id { get; set; } = null;
    }
}
