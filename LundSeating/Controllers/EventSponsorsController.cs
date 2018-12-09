using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LundSeating.Data;
using LundSeating.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace LundSeating.Controllers
{
    [Authorize]
    public class EventSponsorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static int? publicEventID;

        public EventSponsorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventSponsors
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Index(int? id)
        {
            publicEventID = id;
            ViewBag.EID = id;
            var applicationDbContext = _context.EventSponsors.Where(ei => ei.EventID == id).Include(e => e.Event).Include(e => e.Host).Include(e => e.Sponsor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EventSponsors/Details/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSponsor = await _context.EventSponsors
                .Include(e => e.Event)
                .Include(e => e.Host)
                .Include(e => e.Sponsor)
                .SingleOrDefaultAsync(m => m.EventSponsorID == id);
            if (eventSponsor == null)
            {
                return NotFound();
            }

            return View(eventSponsor);
        }

        // GET: EventSponsors/Create
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name");
            ViewData["HostID"] = new SelectList(_context.Guests, "GuestID", "EmailAddress");
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "Name");
            return View();
        }

        // POST: EventSponsors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventSponsorID,EventID,SponsorID,HostID,SponsorshipLevel,NumTicketsAllocated")] EventSponsor eventSponsor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventSponsor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name", eventSponsor.EventID);
            ViewData["HostID"] = new SelectList(_context.Guests, "GuestID", "EmailAddress", eventSponsor.HostID);
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "Name", eventSponsor.SponsorID);
            return View(eventSponsor);
        }

        // GET: EventSponsors/Edit/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSponsor = await _context.EventSponsors.SingleOrDefaultAsync(m => m.EventSponsorID == id);
            if (eventSponsor == null)
            {
                return NotFound();
            }
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name", eventSponsor.EventID);
            ViewData["HostID"] = new SelectList(_context.Guests, "GuestID", "EmailAddress", eventSponsor.HostID);
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "Name", eventSponsor.SponsorID);
            return View(eventSponsor);
        }

        // POST: EventSponsors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventSponsorID,EventID,SponsorID,HostID,SponsorshipLevel,NumTicketsAllocated")] EventSponsor eventSponsor)
        {
            if (id != eventSponsor.EventSponsorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventSponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventSponsorExists(eventSponsor.EventSponsorID))
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
            ViewData["EventID"] = new SelectList(_context.Events, "EventId", "Name", eventSponsor.EventID);
            ViewData["HostID"] = new SelectList(_context.Guests, "GuestID", "EmailAddress", eventSponsor.HostID);
            ViewData["SponsorID"] = new SelectList(_context.Sponsors, "SponsorID", "Name", eventSponsor.SponsorID);
            return View(eventSponsor);
        }

        // GET: EventSponsors/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSponsor = await _context.EventSponsors
                .Include(e => e.Event)
                .Include(e => e.Host)
                .Include(e => e.Sponsor)
                .SingleOrDefaultAsync(m => m.EventSponsorID == id);
            if (eventSponsor == null)
            {
                return NotFound();
            }

            return View(eventSponsor);
        }

        // POST: EventSponsors/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventSponsor = await _context.EventSponsors.SingleOrDefaultAsync(m => m.EventSponsorID == id);
            _context.EventSponsors.Remove(eventSponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventSponsorExists(int id)
        {
            return _context.EventSponsors.Any(e => e.EventSponsorID == id);
        }
        
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> AddGuest(int? id) {
            ViewBag.ESID = id;
            //String eventId = Request.Form["EID"].ToString();
            //int EID = int.Parse(eventId);
            var @eventsponsor = await _context.EventGuests.Where(e => e.EventID == publicEventID).Include(g => g.Guest).Include(f => f.Event)
                .ToListAsync();
            return View(@eventsponsor);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> AddGuest2Sponsor(String[] eGuests) {
            String[] eGuestIds = Request.Form["eGuests[]"].ToArray();
            String eSponsorId = Request.Form["ESID"].ToString();
            int ESID = int.Parse(eSponsorId);

            if (eGuestIds.Length > 0)
            {
                foreach (String eGuestId in eGuestIds)
                {
                    int egid = int.Parse(eGuestId);
                    await _context.Database.ExecuteSqlCommandAsync("UPDATE EventGuests SET EventSponsorID = {0} WHERE EventGuestID = {1}", ESID, egid);
                    
                }
                await _context.SaveChangesAsync();
            }
            
            else {
                return Ok("No guests selected");
            }
            return RedirectToAction("Index", "Events");
        }
    }
}
