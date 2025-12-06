using DemoMvc551.Data;
using DemoMvc551.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoMvc551.Models.Process;
using System.IO;

namespace DemoMvc551.Controllers

{
    public class PersonController : Controller
    {
        
        private readonly ApplicationDbContext _context ;
        private readonly ExcelProcess _excelProcess;
        public PersonController (ApplicationDbContext context)
        {
            _context = context ;
            _excelProcess = new ExcelProcess();
        }
        
        public async Task<IActionResult> Index()
        {
            var model = await _context.Persons.ToListAsync();
            return View(model);
        }
        public IActionResult Create ()
        {
            return View();
    
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("PersonId,FullName,Address")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound ();
            }
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();

            }
            return View(person);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (string id, [Bind("PersonId,FullName,Address")] Person person )
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
                    {
                        return NotFound ();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            return View(person);

        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound ();
            }
            var person = await _context.Persons
            .FirstOrDefaultAsync(m => m.PersonId == id);
            if(person == null)
            {
                return NotFound();
            }
            return View(person);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Persons == null )
            {
                return Problem("Entity set 'ApplicationDbContext.Person' is null." );
            }
            var person = await _context.Persons.FindAsync(id);
            if (person!= null)
            {
                _context.Persons.Remove(person);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool PersonExists(String id)
        {
            return (_context.Persons?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
        public async Task< IActionResult> UpLoad()
        {
            return View();
        }
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpLoad(IFormFile file)
{
    if (file == null || file.Length == 0)
    {
        ModelState.AddModelError("", "Please select an excel file to upload!");
        return View();
    }

    var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
    if (fileExtension != ".xls" && fileExtension != ".xlsx")
    {
        ModelState.AddModelError("", "Please select an excel file (.xls or .xlsx)!");
        return View();
    }

    try
    {
        // --- đọc trực tiếp từ IFormFile (không cần lưu file lên disk) ---
        var dt = DemoMvc551.Models.Process.ExcelProcess.ExcelToDataTable(file, hasHeader: true);

        var persons = new List<Person>();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            var row = dt.Rows[i];

            // lấy dữ liệu an toàn
            var id = dt.Columns.Contains("PersonId") ? Convert.ToString(row["PersonId"]) : (row.ItemArray.Length > 0 ? Convert.ToString(row[0]) : null);
            var name = dt.Columns.Contains("FullName") ? Convert.ToString(row["FullName"]) : (row.ItemArray.Length > 1 ? Convert.ToString(row[1]) : null);
            var addr = dt.Columns.Contains("Address") ? Convert.ToString(row["Address"]) : (row.ItemArray.Length > 2 ? Convert.ToString(row[2]) : null);

            if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(name))
                continue; // bỏ dòng rỗng

            persons.Add(new Person
            {
                PersonId = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id,
                FullName = name,
                Address = addr
            });
        }

        if (persons.Any())
        {
            await _context.Persons.AddRangeAsync(persons);
            await _context.SaveChangesAsync();
            TempData["Message"] = $"Imported {persons.Count} records.";
        }
        else
        {
            TempData["Message"] = "No valid records to import.";
        }

        return RedirectToAction(nameof(Index));
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", "Error importing Excel: " + ex.Message);
        return View();
    }
}
}
}
 