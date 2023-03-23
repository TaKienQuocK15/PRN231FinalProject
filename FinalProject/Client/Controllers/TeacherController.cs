using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
	public class TeacherController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
