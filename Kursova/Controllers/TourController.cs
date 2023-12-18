using Kursova.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kursova.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly IHomeRepository _homeRepository;
        public TourController(ITourRepository tourRepository, IHomeRepository homeRepository)
        {
            _tourRepository = tourRepository;
            _homeRepository = homeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            var categories = await _homeRepository.Categories();
            var tour = new CreateTourViewModel()
            {
                Categories = categories.ToList()
            };
            return View(tour);
        }
        [HttpGet("/Tour/UpdateGroup/{tourId}")]
        public async Task<IActionResult> UpdateTour([FromRoute]int tourId)
        {
            var categories = await _homeRepository.Categories();
            Tour tour = await _tourRepository.GetTourById(tourId);
            var model = new UpdateTourModel()
            {
                Categories = categories.ToList(),
                TourName = tour.TourName,
                TourId = tourId,
                TourDescription = tour.TourDescription,
                Price = tour.Price
            };

            return View("UpdateTour", model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTour(Tour model)
        {
            var tour = new Tour
            {
                TourName = model.TourName,
                TourDescription = model.TourDescription,
                Price = model.Price,
                CategoryId = model.CategoryId
            };
            await _tourRepository.AddTour(tour);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTours(Tour model)
        {
            var tour = new Tour
            {
                TourId = model.TourId,
                TourName = model.TourName,
                TourDescription = model.TourDescription,
                Price = model.Price,
                CategoryId = model.CategoryId
            };
            await _tourRepository.UpdateTour(tour);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("/Tour/DeleteTour/{tourId}")]
        public async Task<IActionResult> DeleteTour([FromRoute]int tourId)
        {
            await _tourRepository.DeleteTour(tourId);
            return RedirectToAction("Index", "Home");
        }
    }
}
