using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab5NET.Data;
using Lab5NET.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lab5NET.Pages.Predictions
{
    public class IndexModel : PageModel
    {
        private readonly SportsDbContext _context;

        public IndexModel(SportsDbContext context)
        {
            _context = context;
        }

        public IList<Prediction> Predictions { get; set; }
        public string SportClubTitle { get; set; }

        public string SportClubId { get; set; }

        public async Task OnGetAsync(string id)
        {
            SportClubId = id;
            if (!string.IsNullOrEmpty(id))
            {
                Predictions = await _context.Predictions
                    .Where(p => p.SportClubId == id)
                    .ToListAsync();

                var sportClub = await _context.SportClubs
                    .Where(sc => sc.Id == id)
                    .FirstOrDefaultAsync();

                SportClubTitle = sportClub.Title;
            }
            
        }
    }
}
