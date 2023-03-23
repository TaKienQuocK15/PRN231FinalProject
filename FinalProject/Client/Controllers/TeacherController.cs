using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
	public class TeacherController : Controller
	{
		private readonly HttpClient client = null;
		private string teacherApiUrl = "";
		public TeacherController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			teacherApiUrl = "http://localhost:5143/api/Teacher";
        }
		Teacher? GetTeacherFromSession()
		{
			string str = HttpContext.Session.GetString("AccountSession");
			if (str == null)
				return null;
			Account account = JsonSerializer.Deserialize<Account>(str);
			Teacher teacher = new Prn231dbContext().Teachers.SingleOrDefault(t => t.Id == account.Id);
			return teacher;
		}
		public async Task<IActionResult> Index()
		{
			HttpResponseMessage response = await client.GetAsync(teacherApiUrl);
			string strData = await response.Content.ReadAsStringAsync();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			List<Teacher> teachers = JsonSerializer.Deserialize<List<Teacher>>(strData, options);
			Teacher? teacher = GetTeacherFromSession();
			if (teacher == null)
				return RedirectToAction("Index", "SignIn");
			return View(teacher);
		}
	}
}
