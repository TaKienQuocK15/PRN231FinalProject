using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
