using Kursova.Models.Categories;

namespace Kursova.Models.DTO
{
    public class TourDisplayModel
    {
        public IEnumerable<Tour> Tours { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public string sTerm { get; set; } = "";
        public int CategoryId { get; set; } = 0;
    }
}
