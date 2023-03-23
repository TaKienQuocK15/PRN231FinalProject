using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
    
    public class TeacherController : Controller
	{
        private readonly HttpClient client = null;
		public TeacherController()
		{
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.BaseAddress = new Uri("http://localhost:5143/");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
		Teacher? GetTeacherFromSession()
		{
			string str = HttpContext.Session.GetString("AccountSession");
			if (str == null)
				return null;
			Account account = JsonSerializer.Deserialize<Account>(str);
			Teacher teacher;
			HttpResponseMessage response = client
				.GetAsync("api/Teacher/GetTeacherById/" + account.TeacherId)
				.GetAwaiter()
				.GetResult();
			if (response.IsSuccessStatusCode)
			{
				string strData = response.Content.ReadAsStringAsync()
					.GetAwaiter()
					.GetResult();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				teacher = JsonSerializer.Deserialize<Teacher>(strData, options);
			}
            else teacher = null;
			if(teacher == null)
			{
				ViewData["msg"] = "wtf bro there's nothing";
				return null;
			}else
            return teacher;
		}
		public async Task<IActionResult> Index()
		{
			/*HttpResponseMessage response = await client.GetAsync(teacherApiUrl);
			string strData = await response.Content.ReadAsStringAsync();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			List<Teacher> teachers = JsonSerializer.Deserialize<List<Teacher>>(strData, options);*/
			Teacher? teacher = GetTeacherFromSession();
			if (teacher == null)
				return RedirectToAction("Index", "SignIn");
			return View(teacher);
		}
	}
}
