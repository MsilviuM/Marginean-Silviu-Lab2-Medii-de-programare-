using Marginean_Silviu_Lab2.Models;
namespace Marginean_Silviu_Lab2.Models.ViewModels
{
    public class CategoryIndexData
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Book> Books { get; set; }

    }
}
