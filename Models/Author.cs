namespace Marginean_Silviu_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; } // cheia primară
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public ICollection<Book>? Books { get; set; } // navigation property către cărți
    }
}
