using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
