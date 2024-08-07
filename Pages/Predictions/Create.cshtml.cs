using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab5NET.Data;
using Lab5NET.Models;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;

namespace Lab5NET.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";
        private readonly SportsDbContext _context;

        public CreateModel(SportsDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            SportClubId = id;
            var sportClub = await _context.SportClubs
                    .Where(sc => sc.Id == id)
                    .FirstOrDefaultAsync();

            SportClubTitle = sportClub.Title;
            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; } = default!;

        [BindProperty]
        public IFormFile Upload { get; set; }

        public string SportClubId { get; set; }
        public string SportClubTitle { get; set; }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            SportClubId = id;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            

            //string containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
            string containerName = earthContainerName;
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(Upload.FileName);
            using (var stream = Upload.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = Upload.ContentType });
            }

            Prediction.FileName = Upload.FileName;
            Prediction.Url = blobClient.Uri.ToString();
            Prediction.SportClubId = SportClubId;

            _context.Predictions.Add(Prediction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = SportClubId});
        }
    }
}
