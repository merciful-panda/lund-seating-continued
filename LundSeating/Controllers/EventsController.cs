using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using LundSeating.Data;
using LundSeating.Models;
using Newtonsoft.Json;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;


namespace LundSeating.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public static string EventID;

        public EventsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;

        }

        // GET: Events
        [Authorize(Roles = "ADMIN, MANAGER, VIEWER")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Name,Date,IsLocked")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);

                if (!_context.EventSeats.Where(es => es.EventID == @event.EventId).Any())
                {
                    foreach (Seat seat in _context.Seats)
                    {
                        EventSeat es = new EventSeat { EventID = @event.EventId, SeatID = seat.SeatID };
                        _context.Add(es);
                    }


                }
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Date,IsLocked")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [Authorize(Roles = "ADMIN")]
        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.SingleOrDefaultAsync(m => m.EventId == id);
            foreach (var eventGuest in _context.EventGuests)
            {
                if (eventGuest.EventID == @event.EventId)
                {
                    _context.EventGuests.Remove(eventGuest);
                }
            }
            foreach (var eventSponsor in _context.EventSponsors)
            {
                if (eventSponsor.EventID == @event.EventId)
                {
                    _context.EventSponsors.Remove(eventSponsor);
                }
            }
            foreach (var eventSeat in _context.EventSeats)
            {
                if (eventSeat.EventID == @event.EventId)
                {
                    _context.EventSeats.Remove(eventSeat);
                }
            }
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }

        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> AddSeats(int eventId, List<int> seatIds)
        {
            var @event = await _context.Events
                .SingleOrDefaultAsync(m => m.EventId == eventId);
            if (@event == null)
            {
                return NotFound();
            }

            var @seats =
                from seat in _context.Seats
                where seatIds.Contains(seat.SeatID)
                select seat;

            foreach (Seat seat in @seats)
            {
                EventSeat es = new EventSeat { EventID = @event.EventId, SeatID = seat.SeatID };
                _context.Add(es);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
        }


        [Authorize(Roles = "ADMIN, MANAGER")]   
        public async Task<IActionResult> Seats(int? id)
        {
            ViewBag.EID = id;
            EventID = id.ToString();
            var @events = await _context.Events
                 .Include(e => e.Seats)
                    .ThenInclude(es => es.Seat)
                 .Include (e => e.Sponsors)
                    .ThenInclude(es => es.Sponsor)
                 .Include(e => e.Guests)
                    .ThenInclude(eg => eg.Guest)
                 .ToListAsync();

            return View(@events);
        }


        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> AddGuest2Seat()
        {
            //String guestID = Request.Form["guest"].ToString();
            //String seatID = Request.Form["seat"].ToString();
            //int GID = int.Parse(guestID);
            //int SID = int.Parse(seatID);
            int GID = 4;
            int SID = 88;

            await _context.Database.ExecuteSqlCommandAsync("UPDATE EventGuests SET EventSeatID = {0} WHERE EventGuestID = {1}", SID, GID);


            //return RedirectToAction(nameof(Seats));
            return Ok("Added the guest to the seat");
        }

        public IActionResult AddGuest()
        {
            return View("../Guests/Create");
        }

        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult RemoveGuest(int? id)
        {
            return RedirectToAction("Delete", "GuestsController", new { id = id });
        }

        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        public async Task<IActionResult> AddGuest2Event(String[] guests)
        {
            String[] guestIds = Request.Form["guests[]"].ToArray();
            String EventID = Request.Form["EventID"].ToString();
            int EID = int.Parse(EventID);

            if (guestIds.Length > 0)
            {
                foreach (String guest in guestIds)
                {
                    int guestid = int.Parse(guest);
                   // Console.WriteLine(guestid);
                    EventGuest eg = new EventGuest { GuestID = guestid, EventID = EID, EventSponsorID = _context.EventSponsors.Where(z => z.EventID == EID).First().EventSponsorID };
                    _context.Add(eg);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Ok("You selected no guests.");
            }
        }

        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> GuestToEvent(int? id)
        {
            ViewBag.EID = id;
            var @event = await _context.Guests
                .ToListAsync();
             
            return View(@event);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> AddSponsor2Event(String[] guests)
        {
            String[] sponsorIds = Request.Form["sponsors[]"].ToArray();
            String eventId = Request.Form["eventID"].ToString();
            int EID = int.Parse(eventId);

            if (sponsorIds.Length > 0)
            {
                foreach (String sponsor in sponsorIds)
                {
                    int sponsorid = int.Parse(sponsor);
                   // Console.WriteLine(sponsorid);
                    EventSponsor es = new EventSponsor {EventID = EID, SponsorID = sponsorid,  };
                    _context.Add(es);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Ok("You selected no sponsors.");
            }
        }


        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> SponsorsToEvent(int? id)
        {
            ViewBag.EID = id;
            var @event = await _context.Sponsors
                .ToListAsync();

            return View(@event);
        }

        public class caddi
        {
            public int seatid { get; set; }
            public int guestid { get; set; }
            public string sponsorshiplevel { get; set; }
            //public string sponsorshipLevel { get; set; }
        }
        [HttpPost]
        public caddi seatperson(caddi selectedSeat)
        {
            int selectedseat = selectedSeat.seatid;
            int guestid = selectedSeat.guestid;
            int temp ;
            String sLevel = null;
            if (guestid != -1)
            {
                _context.Database.ExecuteSqlCommand("UPDATE EventGuests SET EventSeatID = {0} WHERE EventGuestID = {1}", selectedseat, guestid);
                _context.Database.ExecuteSqlCommand("UPDATE EventSeats SET EventGuestID = {0} WHERE EventSeatID = {1}", guestid, selectedseat);


                temp = _context.EventGuests.Where(x => x.EventGuestID == guestid).First().EventSponsorID;
                sLevel = _context.EventSponsors.Where(z => z.EventSponsorID == temp).First().SponsorshipLevel.Value.ToString();

                return selectedSeat;

            }
            caddi seatedguest = new caddi();
            seatedguest.guestid = -1;
            seatedguest.seatid = -1;
            //sponsorship level must be added to the caddie and javascript
            seatedguest.sponsorshiplevel = sLevel;
            return seatedguest;
        }




        public JsonResult onHover(int seatID)
        {
            int EID = int.Parse(EventID);
            var eguestid = _context.EventSeats.Where(c => c.EventSeatID == seatID).First().EventGuestID;
            var guestid = _context.EventGuests.Where(x => x.EventGuestID == eguestid).First().GuestID;
            var guestname = _context.Guests.Where(x => x.GuestID == guestid).First().Name;

            var esponsorid = _context.EventGuests.Where(x => x.EventGuestID == eguestid).First().EventSponsorID;
            var sponsorid = _context.EventSponsors.Where(x => x.EventSponsorID == esponsorid).First().SponsorID;
            var sponsorname = _context.Sponsors.Where(x => x.SponsorID == sponsorid).First().Name;

            var seat = _context.EventSeats.Where(x => x.EventSeatID == seatID).First().SeatID;
            var section = _context.Seats.Where(x => x.SeatID == seat).First().Section;
            var row = _context.Seats.Where(x => x.SeatID == seat).First().Row;



            string[] person = new String[6];

            person[0] = seatID.ToString(); //Seat ID
            person[1] = guestid.ToString(); //Guest ID
            person[2] = guestname.ToString(); //Guest Name
            person[3] = sponsorname.ToString(); //Sponsor Name
            person[4] = section.ToString(); //Seat Section
            person[5] = row.ToString(); //Seat Row





            return Json(person);
        }


        public String unseatGuest(int seatID)
        {
            if (seatID != null)
            {
                _context.Database.ExecuteSqlCommand("UPDATE EventGuests SET EventSeatID = {0} WHERE EventSeatID = {1}", null, seatID);
                _context.Database.ExecuteSqlCommand("UPDATE EventSeats SET EventGuestID = {0} WHERE EventSeatID = {1}", null, seatID);

                return "1";
            }

            return "0";
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { _hostingEnvironment.WebRootPath };
        }


        public async Task<IActionResult> ExportExcel()
        {
            int EID = int.Parse(EventID);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Lund Guest Seating List.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Demo");
                IRow row = excelSheet.CreateRow(0);

                excelSheet.SetColumnWidth(1, 7000);


                row.CreateCell(0).SetCellValue("ID");
                row.CreateCell(1).SetCellValue("Name");
                row.CreateCell(2).SetCellValue("Section");
                row.CreateCell(3).SetCellValue("Row");
                row.CreateCell(4).SetCellValue("Number");

                int cntr = 1;
                SqlConnection sqlConnection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-LundSeating-80C59FE2-BE0C-4E74-9E4B-AE3F94E88AAD");
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;



                foreach (var eventGuest in _context.EventGuests.Where(c=>c.EventID == EID))
                {
                    cmd.CommandText = "Select Seats.Section, Seats.Row, Seats.Number FROM Seats " +
                                "INNER JOIN EventSeats ON EventSeats.SeatID =Seats.SeatID " +
                                "INNER JOIN EventGuests ON EventGuests.EventGuestID = EventSeats.EventGuestID " +
                                "WHERE EventGuests.EventGuestID =" + eventGuest.EventGuestID;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = sqlConnection;

                    sqlConnection.Open();
                    row = excelSheet.CreateRow(cntr);
                    row.CreateCell(0).SetCellValue(eventGuest.EventGuestID);
                    row.CreateCell(1).SetCellValue(_context.EventGuests.Select(e => e.Guest).Where(x => x.GuestID == eventGuest.GuestID).First().Name);


                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string Esection = reader[0].ToString();
                        string Erow = reader[1].ToString();
                        string Enum = reader[2].ToString();
                        row.CreateCell(2).SetCellValue(Esection);
                        row.CreateCell(3).SetCellValue(Erow);
                        row.CreateCell(4).SetCellValue(int.Parse(Enum));
                    }


                    cntr++;
                    sqlConnection.Close();
                }




                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }


        
    }
}