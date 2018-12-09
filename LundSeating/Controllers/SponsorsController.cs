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
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;

namespace LundSeating.Controllers
{
    [Authorize]
    public class SponsorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SponsorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sponsors
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sponsors.ToListAsync());
        }

        // GET: Sponsors/Details/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors
                .SingleOrDefaultAsync(m => m.SponsorID == id);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // GET: Sponsors/Create
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sponsors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SponsorID,Name,PhoneNumber,EmailAddress")] Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sponsor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }

        // GET: Sponsors/Edit/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors.SingleOrDefaultAsync(m => m.SponsorID == id);
            if (sponsor == null)
            {
                return NotFound();
            }
            return View(sponsor);
        }

        // POST: Sponsors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SponsorID,Name,PhoneNumber,EmailAddress")] Sponsor sponsor)
        {
            if (id != sponsor.SponsorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorExists(sponsor.SponsorID))
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
            return View(sponsor);
        }

        // GET: Sponsors/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors
                .SingleOrDefaultAsync(m => m.SponsorID == id);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // POST: Sponsors/Delete/5
        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsor = await _context.Sponsors.SingleOrDefaultAsync(m => m.SponsorID == id);
            _context.Sponsors.Remove(sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorExists(int id)
        {
            return _context.Sponsors.Any(e => e.SponsorID == id);
        }


        [Authorize(Roles = "ADMIN, MANAGER")]
        [HttpPost("UploadRazorsEdgeExport2")]
        public async Task<IActionResult> UploadRazorsEdgeExport2(List<IFormFile> files)
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
