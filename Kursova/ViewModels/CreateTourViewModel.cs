using Kursova.Models.Categories;
using System.ComponentModel.DataAnnotations;

namespace Kursova.ViewModels
{
    public class CreateTourViewModel
    {

        
        public string TourName { get; set; }
        public string TourDescription { get; set; }

        public double Price { get; set; }
        public List<Category> Categories { get; set; }
    }
}
