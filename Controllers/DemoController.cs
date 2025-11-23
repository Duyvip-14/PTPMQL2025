using Microsoft.AspNetCore.Mvc;
using DemoMvc551.Models;

namespace DemoMvc551.Controllers
{
    public class DemoController : Controller
    {
        // ACTION CHO BÀI THỰC HÀNH 3
    
        
        public IActionResult Index()        {
            return View();
        }
        public IActionResult RedirectAction()
        {
            // RedirectToActionResult
            return RedirectToAction("Success");
        }

        public IActionResult GotoGoogle()
        {
            // RedirectResult
            return Redirect("https://www.google.com");
        }

        public IActionResult Success()
        {
            ViewData["RedirectMessage"] = "Nguyễn Khánh Duy - PTPMQL";
            return View();
        }

        public IActionResult GetProduct()
        {
            // JsonResult
            var data = new
            {
                ProductId = 101,
                Name = "Laptop",
                Price = 599.99m,
                IsAvailable = true
            };
            return Json(data);
        }

        public IActionResult NotFoundAction()
        {
            // StatusCodeResult (404)
            return NotFound();
        }

        // ACTION CHO BÀI THỰC HÀNH 4
        public IActionResult ShowInfo()
        {
            // Gửi dữ liệu sang View
            ViewBag.Name = "Nguyễn Khánh Duy";
            ViewData["Age"] = 22;
            TempData["Message"] = "Đây là dữ liệu được gửi từ Controller sang View!";
            return View(); // Views/Demo/ShowInfo.cshtml
        }

        public IActionResult SendData()
        {
            ViewData["TenSV"] = "Nguyễn Khánh Duy";
            ViewBag.MaSV = "2121050551";
            TempData["ThongBao"] = "Bạn đã truy cập trang SendData!";
            return View(); // Views/Demo/CheckTempData.cshtml (Hoặc SendData.cshtml)
        }
        

    }
}