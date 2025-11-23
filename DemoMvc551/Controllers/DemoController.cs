using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
namespace DemoMvc551.Controllers
{
    public class DemoController : Controller
    {
        // GET: /DemoController/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GoToGoogle()
        {
            return Redirect("https://www.google.com");
        }
        public IActionResult GoToWelcome()
        {
            return RedirectToAction("Welcome");
        }
        public IActionResult GetData()
        {
            var data = new { Name = "Duy", Course = "MVC Basic", Lesson = 3 };
            return Json(data);
        }
        public IActionResult ShowError()
        {
            return StatusCode(404);
        }
        // Bài thực hành 4 
         public IActionResult DemoData()
        {
            ViewBag.Message = "Dữ liệu từ Viewbag";
            ViewData["Note"] = "Đây là dữ liệu từ ViewData";
            TempData["Alert"] = "Đây là dữ liệu từ TempData";
            return View();
        }
public IActionResult ShowInfo()
{
    // Gửi dữ liệu sang View
    ViewBag.Name = "Duy";
    ViewData["Course"] = "Demo MvC";
    TempData["Message"] = "Dữ liệu đã được gửi từ Controller sang View!";

    return View();
}
public IActionResult InputStudent()
{
    return View();
}
[HttpPost]
public IActionResult InputStudent(Student student)
{
    if (ModelState.IsValid)
    {
        // Truyền dữ liệu sang View khác để hiển thị
        return View("ShowStudent", student);
    }
    return View();
}

      }
}