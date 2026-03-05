namespace ParcelTrack.Utils
{
    public enum ParcelEventTypes { COLLECTED = 1, SRC_SORT = 2, DEST_SORT = 3, DELIVERY_POINT = 4, DELIVERY_READY = 5, DELIVERED = 6, DELIVERY_FAIL = 7, DELIVERY_RETRY = 8, DELIVERY_RETURN = 9 };

    public static class ShippingPrices
    {
        public const decimal InitialCost = 100.0m;
        public const int PriceSlab = 50;
        public const decimal ExtraSlabCost = 0.20m;
    }
}