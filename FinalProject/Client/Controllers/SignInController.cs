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
			client.BaseAddress = new Uri("http://localhost:5143/");
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

			Account? a;
			HttpResponseMessage response = client
				.GetAsync("api/account/GetAccountByEmail/" + account.Email)
				.GetAwaiter().GetResult();
			if (response.IsSuccessStatusCode)
			{
				string strData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				a = JsonSerializer.Deserialize<Account>(strData, options);
			}
			else a = null;

			if (a == null)
			{
				ViewData["msg"] = "Account does not exist";
				return View(account);
			}
			else
			{
				if (!a.Password.Equals(account.Password))
				{
					ViewData["msg"] = "Wrong password";
					return View(account);
				}
				
				HttpContext.Session.SetString("AccountSession", JsonSerializer.Serialize(a));
				if (a.Role == 1)
					return RedirectToAction("Index", "Teacher");
				else return RedirectToAction("Index", "Student");
			}
		}
	}
}
