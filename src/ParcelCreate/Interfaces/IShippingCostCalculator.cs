namespace ParcelCreate.Interfaces
{
    public interface IShippingCostCalculator
    {
        Task<float> GetShippingCost(int Height, int Width, int Length);
    }
}
