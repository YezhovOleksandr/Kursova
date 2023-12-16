using Kursova.Models;
using Kursova.Models.DTO;
using Kursova.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kursova.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;



        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sTerm="", int categoryId = 0)
        {
            var tours = await _homeRepository.GetTours(sTerm, categoryId);
            var categories = await _homeRepository.Categories();
            TourDisplayModel tourModel = new TourDisplayModel()
            {
                Tours = tours,
                Categories = categories,
                sTerm = sTerm,
                CategoryId = categoryId
            };
            return View(tourModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
