using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LundSeating.Data;
using LundSeating.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace LundSeating.Controllers
{
    public class ControllerUtilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ControllerUtilController(ApplicationDbContext context) {
            _context = context;
        }

        public Boolean UploadRazorsEdgeExport(List<IFormFile> files)
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
                        formFile.CopyToAsync(stream);
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
                    _context.SaveChangesAsync();
                }

                return true;

        }

    }
}