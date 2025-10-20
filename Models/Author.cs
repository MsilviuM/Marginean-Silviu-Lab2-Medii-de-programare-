namespace Marginean_Silviu_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation property: un autor poate avea mai multe cărți
        public ICollection<Book>? Books { get; set; }
    }
}
