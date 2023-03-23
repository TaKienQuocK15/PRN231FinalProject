using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Client.Controllers
{
	public class TeacherController : Controller
	{
		Teacher? GetTeacherFromSession()
		{
			string str = HttpContext.Session.GetString("AccountSession");
			if (str == null)
				return null;
			Account account = JsonSerializer.Deserialize<Account>(str);
			Teacher teacher = new Prn231dbContext().Teachers.SingleOrDefault(t => t.Id == account.Id);

			return teacher;
		}
		
		public IActionResult Index()
		{
			Teacher? teacher = GetTeacherFromSession();
			if (teacher == null)
				return RedirectToAction("Index", "SignIn");
			return View(teacher);
		}
	}
}
