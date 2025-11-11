using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Marginean_Silviu_Lab2.Data;
using Marginean_Silviu_Lab2.Models;

namespace Marginean_Silviu_Lab2.Pages.Borrowings
{
    public class DetailsModel : PageModel
    {
        private readonly Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context _context;

        public DetailsModel(Marginean_Silviu_Lab2.Data.Marginean_Silviu_Lab2Context context)
        {
            _context = context;
        }

        public Borrowing Borrowing { get; set; } = default!;

        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
        //    if (borrowing == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        Borrowing = borrowing;
        //    }
        //    return Page();
        //}


        public async Task<IActionResult> OnGetAsync(int? id)
        {
         

            if (id == null || _context.Borrowing == null)
            {
                return NotFound();
            }

         
            var borrowing = await _context.Borrowing
                .Include(b => b.Book)
                .ThenInclude(b => b.Author) 
                .Include(b => b.Member)      
                .FirstOrDefaultAsync(m => m.ID == id);
           

            if (borrowing == null)
            {
                return NotFound();
            }
            else
            {
                Borrowing = borrowing;
            }
            return Page();
        }
    }
}
