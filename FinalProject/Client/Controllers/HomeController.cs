using Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SignIn([Bind]Account account)
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
					if (a.Role == 1)
						return RedirectToAction("Index", "Teacher");
					else return RedirectToPage("/student");
				}
			}
		}
	}
}
