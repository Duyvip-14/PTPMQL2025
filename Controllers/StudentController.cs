using Microsoft.AspNetCore.Mvc;
using DemoMvc551.Models;

namespace DemoMvc551.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Form()
        {
            return View(new Student());
        }

        [HttpPost]
        public IActionResult Form(Student student)
        {
            return View("Result", student);
        }
    }
}
