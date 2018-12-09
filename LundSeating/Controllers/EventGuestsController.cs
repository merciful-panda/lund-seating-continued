using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LundSeating.Data;
using LundSeating.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;


namespace LundSeating.Controllers
{
    [Authorize]
    public class EventGuestsController : Controller
    {
        private readonly ApplicationDbContext _context;
      

        public EventGuestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventGuests
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EventGuests.Include(e => e.Event).Include(e => e.Guest).Include(e => e.Sponsor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EventGuests/Details/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventGuest = await _context.EventGuests
                .Include(e => e.Event)
                .Include(e => e.Guest)
                .Include(e => e.Sponsor)
                .SingleOrDefaultAsync(m => m.EventGuestID == id);
            if (eventGuest == null)
            {
                return NotFound();
            }

            return View(eventGuest);
        }

        // GET: EventGuests/Create
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name");
            ViewData["GuestID"] = new SelectList(_context.Guests, "GuestID", "EmailAddress");
            ViewData["EventSponsorID"] = new SelectList(_context.EventSponsors, "EventSponsorID", "EventSponsorID");
            return View();
        }

        // POST: EventGuests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Create([Bind("EventGuestID,EventID,GuestID,EventSponsorID,EventSeatID,RegistrationDate,Status")] EventGuest eventGuest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventGuest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name", eventGuest.EventID);
            ViewData["GuestID"] = new SelectList(_context.Guests, "GuestID", "EmailAddress", eventGuest.GuestID);
            ViewData["EventSponsorID"] = new SelectList(_context.EventSponsors, "EventSponsorID", "EventSponsorID", eventGuest.EventSponsorID);
            return View(eventGuest);
        }

        // GET: EventGuests/Edit/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventGuest = await _context.EventGuests.SingleOrDefaultAsync(m => m.EventGuestID == id);
            if (eventGuest == null)
            {
                return NotFound();
            }
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name", eventGuest.EventID);
            ViewData["GuestID"] = new SelectList(_context.Guests, "GuestID", "Name", eventGuest.GuestID);
            ViewData["EventSponsorID"] = new SelectList(_context.EventSponsors, "EventSponsorID", "EventSponsorID", eventGuest.EventSponsorID);
            return View(eventGuest);
        }

        // POST: EventGuests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventGuestID,EventID,GuestID,EventSponsorID,EventSeatID,RegistrationDate,Status")] EventGuest eventGuest)
        {
            if (id != eventGuest.EventGuestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventGuest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventGuestExists(eventGuest.EventGuestID))
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
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name", eventGuest.EventID);
            ViewData["GuestID"] = new SelectList(_context.Guests, "GuestID", "Name", eventGuest.GuestID);
            ViewData["EventSponsorID"] = new SelectList(_context.EventSponsors, "EventSponsorID", "EventSponsorID", eventGuest.EventSponsorID);
            return View(eventGuest);
        }

        // GET: EventGuests/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventGuest = await _context.EventGuests
                .Include(e => e.Event)
                .Include(e => e.Guest)
                .Include(e => e.Sponsor)
                .SingleOrDefaultAsync(m => m.EventGuestID == id);
            if (eventGuest == null)
            {
                return NotFound();
            }

            return View(eventGuest);
        }

        // POST: EventGuests/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventGuest = await _context.EventGuests.SingleOrDefaultAsync(m => m.EventGuestID == id);
            _context.EventGuests.Remove(eventGuest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventGuestExists(int id)
        {
            return _context.EventGuests.Any(e => e.EventGuestID == id);
        }


    }
}