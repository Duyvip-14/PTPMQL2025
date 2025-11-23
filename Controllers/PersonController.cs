using DemoMvc551.Data;
using DemoMvc551.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMvc551.Controllers

{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context ;
        public PersonController (ApplicationDbContext context)
        {
            _context = context ;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _context.Persons.ToListAsync();
            return View(model);
        }
        public IActionResult
        public IActionResult Index ()
        public IActionResult Create ()
        {
            return View();
    
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("PersonId,FullName,Address")] Person person)
        {
            if (ModelState.IsVaLid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        public async Task<IActionResult> Edit(string id)
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,FullName,Address")] Person person
        public async Task<IActionResult> Delete(string id)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        private bool PersonExists(String id) 

        
        }
} 



 