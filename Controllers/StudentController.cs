using Microsoft.AspNetCore.Mvc;
using DemoMvc551.Models;
using Microsoft.EntityFrameworkCore;
using DemoMvc551.Data;

namespace DemoMvc551.Controllers
{
    public class StudentController : Controller
    {
private readonly ApplicationDbContext _context;
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            
            var model = await _context.Student.ToListAsync(); 
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 

        public async Task<IActionResult> Create([Bind("StudentCode,FullName")] Student student)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(student); 
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

    
        public async Task<IActionResult> Edit(string id)
        {
           
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }
          
            var student = await _context.Student.FindAsync(id); 
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Edit(string id, [Bind("StudentCode,FullName")] Student student)
        {
        
            if (id != student.StudentCode) 
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student); 
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentCode)) 
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
            return View(student);
        }

        public async Task<IActionResult> Delete(string id)
        {
         
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }
          
            var student = await _context.Student 
                .FirstOrDefaultAsync(m => m.StudentCode == id); 
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
          
            if (_context.Student == null)
            {
                
                return Problem("Entity set 'ApplicationDbContext.Student' is null."); 
            }
            
            var student = await _context.Student.FindAsync(id); 
            if (student != null)
            {
                
                _context.Student.Remove(student); 
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool StudentExists(String id) 
        {
            
            return (_context.Student?.Any(e => e.StudentCode == id)).GetValueOrDefault(); 
        }
    }
}
