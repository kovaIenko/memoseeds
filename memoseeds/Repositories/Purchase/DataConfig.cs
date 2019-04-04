﻿using System.Collections.Generic;

namespace memoseeds.Repositories.Purchase.DataConfig
{
    public class StripeConfig
    {
        public string publishableKey { get; private set; }
        public string secretKey { get; private set; }

        public StripeConfig(
            string publishableKey, 
            string secretKey
        ) {
            this.publishableKey = publishableKey;
            this.secretKey = secretKey;
        }
    }

    public class PurchaseConfig
    {
        public string defaultCountry { get; private set; }
        public string defaultPurchaseId { get; private set; }
        public StripeConfig stripeConfig { get; private set; }
        public Dictionary<string, int> currencyToMultiplier { get; private set; }

        public PurchaseConfig(
            string defaultCountry, 
            string defaultPurchaseId, 
            StripeConfig stripeConfig,
            Dictionary<string, int> currencyToMultiplier
        ) {
            this.defaultCountry = defaultCountry;
            this.defaultPurchaseId = defaultPurchaseId;
            this.stripeConfig = stripeConfig;
            this.currencyToMultiplier = currencyToMultiplier;
        }
    }

    public class Price
    {
        public int amount { get; private set; }
        public string currency { get; private set; }

        public Price(
            int amount, 
            string currency
        ) {
            this.amount = amount;
            this.currency = currency;
        }
    }

    public class PurchaseData
    {
        public int credits { get; private set; }
        public string name { get; private set; }
        public Price price { get; private set; }
        public string id { get; set; } = null;

        public PurchaseData(
            int credits, 
            string name, 
            Price price, 
            string id
        ) {
            this.credits = credits;
            this.name = name;
            this.price = price;
            this.id = id;
        }
    }
}
