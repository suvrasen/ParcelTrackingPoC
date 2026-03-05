using ParcelCreate.Interfaces;
using ParcelCreate.Utilities;

namespace ParcelCreate.BLogic
{
    public class ShippingCostCalculator : IShippingCostCalculator
    {
        async Task<float> IShippingCostCalculator.GetShippingCost(int Height, int Width, int Length)
        {
            float cost = 0;
            int[] dimensions = {Height, Width, Length};
            bool largeParcel = dimensions.Any(d => d > 50);
            if (largeParcel) 
            { 
                cost = (MasterData.ExtraCharge * MasterData.BaseCharge) + MasterData.BaseCharge;
                return cost;
            }
            return MasterData.BaseCharge;
        }
    }
}
