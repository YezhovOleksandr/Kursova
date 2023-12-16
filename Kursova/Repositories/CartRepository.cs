using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kursova.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public CartRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<int>  AddItem(int tourId, int qty)
        {
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User Id is invalid");
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                    };
                    _context.ShoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                var carItem = _context.CartDetails.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId && x.TourId == tourId);
                if (carItem is not null)
                {
                    carItem.Quantity += qty;
                } else
                {
                    carItem = new CartDetail
                    {
                        TourId = tourId,
                        ShoppingCartId = cart.ShoppingCartId,
                        Quantity = qty,
                        
                    };
                    _context.CartDetails.Add(carItem);
                }
                _context.SaveChanges();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int tourId)
        {
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User is invalid");
            }
            try
            {
                
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Invalid cart");
                }
                _context.SaveChanges();
                var carItem = _context.CartDetails.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId && x.TourId == tourId);
                if (carItem is null)
                {
                    throw new Exception("No items in the cart");
                } else if (carItem.Quantity==1) 
                {
                    _context.CartDetails.Remove(carItem);
                }
                
                else
                {
                    carItem.Quantity = carItem.Quantity - 1;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null) throw new Exception("Invalid userId");
            var shoppingCart = await _context.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.tour)
                .ThenInclude(a => a.Categories)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();

            return shoppingCart;
        }
        private async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        private string GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);
            return userId;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                        join cartDetail in _context.CartDetails
                        on cart.ShoppingCartId equals cartDetail.ShoppingCartId
                        select new { cartDetail.CartDetailId }).ToListAsync();
            return data.Count;
        }
    }
}
