using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
   
    public class TeacherController : Controller
	{
        private readonly HttpClient client = null;

		Teacher? GetTeacherFromSession()
		{
			string str = HttpContext.Session.GetString("AccountSession");
			if (str == null)
				return null;
			Account account = JsonSerializer.Deserialize<Account>(str);
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
			else classDetail = null;
			
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
			else students = null;
			
			return students;
		}

		List<Student>? GetStudents()
		{
			HttpResponseMessage response = client
				.GetAsync("api/Student/GetStudents")
				.GetAwaiter()
				.GetResult();
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
			else students = null;
			
			return students;
		}
		Student? GetStudentById(string id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Student/GetStudentById/" + id)
				.GetAwaiter()
				.GetResult();
			Student? student;
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
			else student = null;
			
			return student;

		}

		public TeacherController()
		{
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.BaseAddress = new Uri("http://localhost:5143/");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IActionResult Index()
        {
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
			if (c == null)
				return NotFound();

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

		public IActionResult AddStudentToClass(string id, int id2)
		{
			Teacher? teacher = GetTeacherFromSession();
			if (teacher == null)
				return Unauthorized();

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
		}

		public IActionResult DeleteClass(int id)
		{
            Teacher? teacher = GetTeacherFromSession();
            if (teacher == null)
                return Unauthorized();
			Class? clss = GetClassById(id);
			if (clss == null)
				return Unauthorized();
			return View(clss);
        }

        [HttpPost, ActionName("DeleteClass")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = client
				.PostAsync("api/Class/DeleteClass/" + id, null)
				.GetAwaiter()
				.GetResult();
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            else return BadRequest();
        }
        public IActionResult AddClass()
		{
            Teacher? teacher = GetTeacherFromSession();
            if (teacher == null)
                return Unauthorized();
			ViewData["Teacher"] = new Teacher()
			{
				Id = teacher.Id,
				Name = teacher.Name
			};
			return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddClass([Bind("Name")] Class c)
        {
            Teacher? teacher = GetTeacherFromSession();
            if (teacher == null)
                return Unauthorized();
			Class clss = new Class()
			{
				Id = c.Id,
				Name = c.Name,
				TeacherId = c.TeacherId,
				Teacher = new Teacher
				{
					Id = 0,
					Name = ""
				}
			};
			HttpResponseMessage response = client
				.PostAsJsonAsync("api/Class/AddClass/" + teacher.Id, clss )
				.GetAwaiter()
				.GetResult();
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			else return BadRequest();
        }

        public IActionResult RemoveStudentFromClass(string id, int id2)
        {
			Teacher? teacher = GetTeacherFromSession();
			if (teacher == null)
				return Unauthorized();

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
        }
    }
}
