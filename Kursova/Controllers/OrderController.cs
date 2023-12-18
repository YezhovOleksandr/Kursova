using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace Kursova.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUserOrderRepository _orderRepository;

        public OrderController(IUserOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> UserOrders()
        {
            var orders = await _orderRepository.UserOrders();
            return View(orders);
        }
    }
}
