using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
	public class SignInController : Controller
	{
		private readonly HttpClient client = null;
		
		public SignInController()
		{
			client = new HttpClient();
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.BaseAddress = new Uri("http://localhost:5281/");
			client.DefaultRequestHeaders.Accept.Add(contentType);
		}
		
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Index([Bind]Account account)
		{
			if (!ModelState.IsValid)
				return View(account);
			
			using (var db = new Prn231dbContext())
			{
				Account a = db.Accounts.SingleOrDefault(a1 => a1.Email.Equals(account.Email) 
					&& a1.Password.Equals(account.Password));
				
				if (a == null)
				{
					ViewData["msg"] = "Wrong email or password";
					return View(account);
				}
				else
				{
					HttpContext.Session.SetString("AccountSession", JsonSerializer.Serialize(a));
					if (a.Role == 1)
						return RedirectToAction("Index", "Teacher");
					else return RedirectToAction("Index", "Student");
				}
			}
		}
	}
}
