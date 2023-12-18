namespace Kursova.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly ApplicationDbContext _context;

        public TourRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTour(Tour tour)
        {
            await _context.Tours.AddAsync(tour);
            _context.SaveChanges();
        }

        public async Task DeleteTour(int tourId)
        {
            Tour tour = await _context.Tours.FindAsync(tourId);
             _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
        }

        public async Task<Tour> GetTourById(int tourId)
        {
           return await _context.Tours.FindAsync(tourId);
        }

        public async Task UpdateTour(Tour tour)
        {
           Tour updateTour = await _context.Tours.FindAsync(tour.TourId);
            updateTour.TourName = tour.TourName;
            updateTour.TourDescription = tour.TourDescription;
            updateTour.Image = tour.Image;
            updateTour.Price = tour.Price;
            updateTour.CategoryId = tour.CategoryId;

            _context.Tours.Update(updateTour);
            _context.SaveChanges();
        }
    }
}
