using Kursova.Models.Categories;

namespace Kursova.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Tour>> GetTours(string sTerm = "", int categoryId = 0);
        public  Task<IEnumerable<Category>> Categories();
    }
}