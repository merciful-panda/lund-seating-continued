using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LundSeating.Data;
using LundSeating.Models;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using LundSeating.Services;
using LundSeating.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace LundSeating.Controllers
{
    [Authorize]
    public class GuestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private ControllerUtilController _util = new ControllerUtilController();
        

        public GuestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Guests
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Guests.ToListAsync());
        }

        // GET: Guests/Details/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .SingleOrDefaultAsync(m => m.GuestID == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // GET: Guests/Create
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuestID,Name,Address,PhoneNumber,EmailAddress")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: Guests/Edit/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.SingleOrDefaultAsync(m => m.GuestID == id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuestID,Name,Address,PhoneNumber,EmailAddress")] Guest guest)
        {
            if (id != guest.GuestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestExists(guest.GuestID))
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
            return View(guest);
        }

        // GET: Guests/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .SingleOrDefaultAsync(m => m.GuestID == id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // POST: Guests/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guest = await _context.Guests.SingleOrDefaultAsync(m => m.GuestID == id);
            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestExists(int id)
        {
            return _context.Guests.Any(e => e.GuestID == id);
        }

        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost("UploadRazorsEdgeExport")]
        public async Task<IActionResult> UploadRazorsEdgeExport(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePath = Path.GetTempFileName();

            // save the uploaded file to a temp file
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            // process the file
            FileInfo file = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(file))
            {

                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int numRows = worksheet.Dimension.Rows;
                int numCols = worksheet.Dimension.Columns;
                for (int row = 2; row <= numRows; row++)
                {
                    string[] cells = new string[numCols];
                    for (int col = 1; col <= numCols; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Value;
                        string cell;
                        if (cellValue == null) { cell = ""; } else { cell = cellValue.ToString(); }
                        cells[col - 1] = cell;
                    }
                    if (!cells[0].Equals(""))
                    {
                        Guest g = new Guest { Name = cells[0] + " " + cells[1] };
                        _context.Add(g);
                    }
                    else if (!cells[5].Equals(""))
                    {
                        Sponsor s = new Sponsor { Name = cells[5] };
                        _context.Add(s);
                    }
                }
               await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
