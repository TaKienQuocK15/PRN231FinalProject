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
			if (account.TeacherId == null)
				return null;

			Teacher? teacher;
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
			
			return teacher;
		}

		List<Class>? GetClassesByTeacherId(int id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Teacher/GetClassesByTeacherId/" + id)
				.GetAwaiter()
				.GetResult();
			List<Class>? classes;

			if (response.IsSuccessStatusCode)
			{
				string strData = response.Content.ReadAsStringAsync()
					.GetAwaiter()
					.GetResult();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				classes = JsonSerializer.Deserialize<List<Class>>(strData, options);
				return classes;
			}
			else classes = null;
			
			return classes;
		}

		Class? GetClassById(int id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Class/GetClassById/" + id)
				.GetAwaiter()
				.GetResult();
			Class? classDetail;
			if (response.IsSuccessStatusCode)
			{
				string strData = response.Content.ReadAsStringAsync()
					.GetAwaiter()
					.GetResult();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				classDetail = JsonSerializer.Deserialize<Class>(strData, options);
				return classDetail;
			}
			else
				classDetail = null;
			
			return classDetail;
		}

		List<Student>? GetStudentsByClassId(int id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Class/GetStudentsByClassId/" + id)
				.GetAwaiter().GetResult();
			List<Student>? students;
			if (response.IsSuccessStatusCode)
			{
				string strData = response.Content.ReadAsStringAsync()
					.GetAwaiter()
					.GetResult();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				students = JsonSerializer.Deserialize<List<Student>>(strData, options);
				return students;
			}
			else
				students = null;
			
			return students;
		}

		public IActionResult Index()
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
				return Unauthorized();

			List<Class>? classes = GetClassesByTeacherId(teacher.Id);
            var viewModel = new TeacherViewModel
			{
                Teacher = teacher,
                Classes = classes
			};
			
            return View(viewModel);
        }
        
		public IActionResult ClassDetails(int id)
		{
			Teacher? teacher = GetTeacherFromSession();
			if (teacher == null)
				return Unauthorized();

			Class? c = GetClassById(id);
			if (c == null) return NotFound();

			List<Student> students = GetStudentsByClassId(c.Id);
			var viewModel = new ClassViewModel
			{
				Class = c,
				Students = students
            };
            return View(viewModel);
		}

    }
}
