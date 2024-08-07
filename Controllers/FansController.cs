using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5NET.Data;
using Lab5NET.Models;
using Lab5NET.Models.ViewModels;

namespace Lab5NET.Controllers
{
    public class FansController : Controller
    {
        private readonly SportsDbContext _context;

        public FansController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: Fans
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new NewsViewModel
            {
                Fans = await _context.Fans.ToListAsync()
            };

            if (id != null)
            {
                viewModel.SportClubs = await _context.Subscriptions
                    .Where(s => s.FanId == id)
                    .Select(s => s.SportClub)
                    .ToListAsync();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> EditSubscriptions(int id)
        {
            var fan = await _context.Fans
                .Include(f => f.Subscriptions)
                .ThenInclude(s => s.SportClub)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fan == null)
            {
                return NotFound();
            }

            var subscribedSportClubIds = fan.Subscriptions.Select(s => s.SportClubId).ToList();

            // Fetch all sport clubs
            var allSportClubs = await _context.SportClubs
                .ToListAsync();

            // Sort clubs not subscribed to first, then subscribed clubs
            var sortedSportClubs = allSportClubs
                .OrderBy(sc => subscribedSportClubIds.Contains(sc.Id))  // Clubs not subscribed to come first
                .ThenBy(sc => sc.Title)  // Then sort alphabetically
                .ToList();

            var viewModel = new FanSubscriptionsViewModel
            {
                Fan = fan,
                SportClubs = sortedSportClubs,
                SelectedSportClubIds = subscribedSportClubIds
            };

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubscriptions(FanSubscriptionsViewModel viewModel)
        {
            var fan = await _context.Fans
                .Include(f => f.Subscriptions)
                .FirstOrDefaultAsync(f => f.Id == viewModel.Fan.Id);

            if (fan == null)
            {
                return NotFound();
            }

            var currentSubscriptionIds = fan.Subscriptions.Select(s => s.SportClubId).ToList();
            var selectedSportClubIds = viewModel.SelectedSportClubIds;

            // Determine which subscriptions need to be added or removed
            var subscriptionsToAdd = selectedSportClubIds.Except(currentSubscriptionIds).ToList();
            var subscriptionsToRemove = currentSubscriptionIds.Except(selectedSportClubIds).ToList();

            // Remove subscriptions that are not selected
            var subscriptionsToRemoveEntities = fan.Subscriptions
                .Where(s => subscriptionsToRemove.Contains(s.SportClubId))
                .ToList();
            _context.Subscriptions.RemoveRange(subscriptionsToRemoveEntities);

            // Add new subscriptions that are selected
            foreach (var sportClubId in subscriptionsToAdd)
            {
                var subscription = new Subscription
                {
                    FanId = fan.Id,
                    SportClubId = sportClubId
                };
                _context.Subscriptions.Add(subscription);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        // GET: Fans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // GET: Fans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fan);
        }

        // GET: Fans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans.FindAsync(id);
            if (fan == null)
            {
                return NotFound();
            }
            return View(fan);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (id != fan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanExists(fan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fan);
        }

        // GET: Fans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                _context.Fans.Remove(fan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FanExists(int id)
        {
            return _context.Fans.Any(e => e.Id == id);
        }
    }
}
