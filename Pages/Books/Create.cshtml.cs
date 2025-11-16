//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Marginean_Silviu_Lab2.Data;
//using Marginean_Silviu_Lab2.Models;

//namespace Marginean_Silviu_Lab2.Pages.Books
//{
//    public class CreateModel : BookCategoriesPageModel
//    {
//        private readonly Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context _context;

//        public CreateModel(Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context context)
//        {
//            _context = context;
//        }

//        public IActionResult OnGet()
//        {
//            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID",
//            "PublisherName");
//            ViewData["AuthorID"] = new SelectList(
//                _context.Author
//                    .Select(a => new
//                    {
//                        a.ID,
//                        FullName = a.FirstName + " " + a.LastName
//                    }),
//                "ID",
//            "FullName"
//        );

//            var book = new Book();
//            book.BookCategories = new List<BookCategory>();
//            PopulateAssignedCategoryData(_context, book);

//            return Page();
//        }

//        [BindProperty]
//        public Book Book { get; set; } = default!;

//        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
//        {
//            var newBook = new Book();
//            if (selectedCategories != null)
//            {
//                newBook.BookCategories = new List<BookCategory>();
//                foreach (var cat in selectedCategories)
//                {
//                    var catToAdd = new BookCategory
//                    {
//                        CategoryID = int.Parse(cat)
//                    };
//                    newBook.BookCategories.Add(catToAdd);
//                }
//            }
//            Book.BookCategories = newBook.BookCategories;
//            _context.Book.Add(Book);
//            await _context.SaveChangesAsync();
//            return RedirectToPage("./Index");
//        }
//    }
//}





using Marginean_Silviu_Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace Marginean_Silviu_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context _context;

        public CreateModel(Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Populează dropdown-urile pentru autori și edituri
            var authorList = _context.Author
                .Select(a => new
                {
                    a.ID,
                    FullName = a.FirstName + " " + a.LastName
                })
                .ToList();

            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");

            var book = new Book
            {
                BookCategories = new List<BookCategory>()
            };
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            // dacă modelul nu e valid, reafișează formularul cu dropdownurile completate
            if (!ModelState.IsValid)
            {
                var authorList = _context.Author
                    .Select(a => new
                    {
                        a.ID,
                        FullName = a.FirstName + " " + a.LastName
                    })
                    .ToList();

                ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName", Book.AuthorID);
                ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName", Book.PublisherID);
                PopulateAssignedCategoryData(_context, Book);
                return Page();
            }

            // asociază categoriile selectate
            if (selectedCategories != null)
            {
                Book.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    Book.BookCategories.Add(new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    });
                }
            }

            // salvează noua carte cu autorul selectat
            _context.Book.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
