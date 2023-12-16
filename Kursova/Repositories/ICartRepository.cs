namespace Kursova.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int tourId, int qty);
        Task<int> RemoveItem(int tourId);
        Task<ShoppingCart> GetUserCart();

        Task<int> GetCartItemCount(string userId = "");
    }
}