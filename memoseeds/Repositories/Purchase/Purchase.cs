namespace memoseeds.Repositories
{
    public class PurchaseConfig
    {
        public string defaultCountry { get; set; }
    }

    public class Price
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
    }

    public class Purchase
    {
        public int Credits { get; set; }
        public string Name { get; set; }
        public Price Price { get; set; }
        public string Id { get; set; } = null;
    }
}
