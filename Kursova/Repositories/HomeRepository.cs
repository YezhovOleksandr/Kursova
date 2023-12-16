using Kursova.Data;
using Kursova.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace Kursova.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return _context.Categories;
        }

        public async Task<IEnumerable<Tour>> GetTours(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            var tours = await (from tour in _context.Tours
                         join category in _context.Categories
                         on tour.CategoryId equals category.CategoryId
                         where string.IsNullOrWhiteSpace(sTerm) ||(tour!=null && tour.TourName.ToLower().StartsWith(sTerm))

                         select new Tour
                         {
                             TourId = tour.TourId,
                             Image = tour.Image,
                             TourDescription = tour.TourDescription,
                             Price = tour.Price,
                             CategoryId = tour.CategoryId,
                             TourName = tour.TourName,
                             CategoryName = tour.CategoryName,
                         }).ToListAsync();

            if (categoryId > 0)
            {
                tours = tours.Where(x => x.CategoryId == categoryId).ToList();
            }
            return tours;
        }

    }
}
