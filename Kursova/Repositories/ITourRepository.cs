namespace Kursova.Repositories
{
    public interface ITourRepository
    {
        Task AddTour(Tour tour);
        Task DeleteTour(int tourId);
        Task UpdateTour(Tour tour);
        Task<Tour> GetTourById(int tourId);
    }
}