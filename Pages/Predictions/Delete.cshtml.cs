using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab5NET.Data;
using Lab5NET.Models;
using Azure.Storage.Blobs;

namespace Lab5NET.Pages.Predictions
{
    public class DeleteModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";
        private readonly SportsDbContext _context;

        public DeleteModel(SportsDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;

        public string SportClubId { get; set; }
        public string SportClubTitle { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id, string sportClubId)
        {
            SportClubId = sportClubId;
            var sportClub = await _context.SportClubs
                    .Where(sc => sc.Id == sportClubId)
                    .FirstOrDefaultAsync();

            SportClubTitle = sportClub.Title;
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _context.Predictions.FirstOrDefaultAsync(m => m.PredictionId == id);

            if (prediction == null)
            {
                return NotFound();
            }
            else
            {
                Prediction = prediction;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string sportClubId)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction != null)
            {
                Prediction = prediction;


                //string containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
                string containerName = earthContainerName;
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);


                var blobClient = containerClient.GetBlobClient(Prediction.FileName);
                await blobClient.DeleteIfExistsAsync();


                _context.Predictions.Remove(Prediction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { id = sportClubId});
        }
    }
}
