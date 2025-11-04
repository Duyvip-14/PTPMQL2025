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
      }
}