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
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User Id is invalid");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.ShoppingCarts.Add(cart);
                }
                _context.SaveChanges();
                var cartItem = _context.CartDetails.FirstOrDefault(x => x.ShoppingCartId == cart.ShoppingCartId && x.TourId == tourId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                } else
                {
                    var tour = _context.Tours.Find(tourId);
                    cartItem = new CartDetail
                    {
                        TourId = tourId,
                        ShoppingCartId = cart.ShoppingCartId,
                        Quantity = qty,
                        UnitPrice = tour.Price
                        
                    };
                    _context.CartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();
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
            
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is invalid");
                }
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
                .ThenInclude(a => a.Tour)
                .ThenInclude(a => a.Category)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();

            return shoppingCart;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        private string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails
                              on cart.ShoppingCartId equals cartDetail.ShoppingCartId where cart.UserId == userId
                              select new {cartDetail.CartDetailId}  
                              ).ToListAsync();
            return data.Count;
        }

        public async Task<bool> DoCheckOut()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not valid");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Cart is not valid");
                }
                var cartDetail =  await _context.CartDetails.Where(x => x.ShoppingCartId == cart.ShoppingCartId).ToListAsync();

                if (cartDetail.Count == 0)
                {
                    throw new Exception("CartDetails are invalid");
                }
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        TourId = item.TourId,
                        OrderId = order.OrderId,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();
                _context.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
