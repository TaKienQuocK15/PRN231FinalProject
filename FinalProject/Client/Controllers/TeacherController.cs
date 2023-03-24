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
			List<Class>? classes = GetClassesByTeacherId(teacher.Id);
            if (teacher == null)
                return RedirectToAction("Index", "SignIn");
            var viewModel = new TeacherViewModel
			{
                Teacher = teacher,
                Classes = classes
			};
			
            return View(viewModel);
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
		List<Class>? GetClassesByTeacherId(int id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Teacher/GetClassesByTeacherId/" + id)
				.GetAwaiter()
				.GetResult();
			List<Class> classes;

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
			if (classes == null)
			{
				ViewData["msg"] = "No Classes";
				return null;
			}
			else
				return classes;
		}
        Class? GetClassById(int id)
        {
            HttpResponseMessage response = client
                .GetAsync("api/Class/GetClassById/" + id)
                .GetAwaiter()
                .GetResult();
            Class classDetail;
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
            if (classDetail == null)
            {
                ViewData["msg"] = "class doesn't exist";
                return null;
            }
            else return classDetail;
        }
		List<Student>? GetStudentsByClassId(int id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Class/GetStudentsByClassId/" + id)
				.GetAwaiter().GetResult();
			List<Student> students;
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
			if (students == null)
			{
				ViewData["msg"] = "class is empty";
				return null;
			}
			else return students;
		}
		List<Student>? GetStudents()
		{
			HttpResponseMessage response = client
				.GetAsync("api/Student/GetStudents")
				.GetAwaiter()
				.GetResult();
			List<Student> students;
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
            if (students == null)
            {
                ViewData["msg"] = "All Students are added";
                return null;
            }
            else return students;
        }
		Student? GetStudentById(string id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Student/GetStudentById/" + id)
                .GetAwaiter()
                .GetResult();
			Student student;
            if (response.IsSuccessStatusCode)
            {
                string strData = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                student = JsonSerializer.Deserialize<Student>(strData, options);
                return student;
            }
            else
                student = null;
            if (student == null)
            {
                ViewData["msg"] = "Unable to find students";
                return null;
            }
            else return student;

        }
		public async Task<IActionResult> ClassDetails(int id)
		{

			Class? c = GetClassById(id);
			/*if (c == null) return NotFound();*/
			List<Student> students = GetStudentsByClassId(c.Id);
			List<Student> newStudents = GetStudents();
			foreach (Student s in students)
			{
				foreach (Student ns in newStudents)
				{
					if (s.Id.Equals(ns.Id))
					{
						newStudents.Remove(ns);
						break;
					}
				}
			}
			var viewModel = new ClassViewModel
			{
				Class = c,
				Students = students,
				newStudents = newStudents
			
            };
            return View(viewModel);
		}

		public async Task<IActionResult> AddStudentToClass(string id, int id2)
		{
			string studentId = id;
			int classId = id2;

            HttpResponseMessage response = client
                .PostAsync("api/Class/AddStudentToClass/" + studentId + "/" + classId, null)
                .GetAwaiter()
                .GetResult();
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("ClassDetails", new { id = classId });
			}
			else return BadRequest();
            /*return View();*/
		}
        public async Task<IActionResult> RemoveStudentFromClass(string id, int id2)
        {
            string studentId = id;
            int classId = id2;

            HttpResponseMessage response = client
                .DeleteAsync("api/Class/RemoveStudentFromClass/" + studentId + "/" + classId)
                .GetAwaiter()
                .GetResult();
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ClassDetails", new { id = classId });
            }
            else return BadRequest();
            /*return View();*/
        }
    }
}
