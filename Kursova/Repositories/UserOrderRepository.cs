using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kursova.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User is invalid");
            }
                
            var orders = await _context.Orders
                .Include(x => x.OrderStatus).
                Include(x => x.OrderDetail)
                .ThenInclude(x => x.Tour)
                .ThenInclude(x => x.Category)
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return orders;
        }

        private string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
