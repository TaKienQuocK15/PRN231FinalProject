using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
	public class StudentController : Controller
	{
        private readonly HttpClient client = null;

		public StudentController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.BaseAddress = new Uri("http://localhost:5143/");
			client.DefaultRequestHeaders.Accept.Add(contentType);
		}

		List<Class>? GetClassesByStudentId(string id)
        {
            HttpResponseMessage response = client
                .GetAsync("api/Student/GetClassesByStudentId/" + id)
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
        
        Student? GetStudentFromSession()
        {
            string str = HttpContext.Session.GetString("AccountSession");
            if (str == null) return null;
            Account account = JsonSerializer.Deserialize<Account>(str);
            Student? student;
            HttpResponseMessage response = client
                .GetAsync("api/Student/GetStudentById/" + account.StudentId)
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
                student = JsonSerializer.Deserialize<Student>(strData, options);
            }
            else student = null;
            return student;
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

		List<Resource>? GetResourcesFromClass(int classId)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Resource/GetResourcesFromClass/" + classId)
				.Result;
			List<Resource>? resources;
			if (response.IsSuccessStatusCode)
			{
				string strData = response.Content.ReadAsStringAsync().Result;
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				resources = JsonSerializer.Deserialize<List<Resource>>(strData, options);
			}
			else resources = null;

			return resources;
		}

		Resource? GetResourceDataFromId(int id)
		{
			HttpResponseMessage response = client
				.GetAsync("api/Resource/GetResourceDataById/" + id)
				.Result;
			Resource? resource;
			if (response.IsSuccessStatusCode)
			{
				string strData = response.Content.ReadAsStringAsync().Result;
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				resource = JsonSerializer.Deserialize<Resource>(strData, options);
			}
			else resource = null;

			return resource;
		}
        
        public IActionResult Index()
		{
            Student? student = GetStudentFromSession();
            if (student == null)
                return Unauthorized();
            List<Class>? classes = GetClassesByStudentId(student.Id);
            var viewModel = new StudentViewModel
            {
                Student = student,
                Classes = classes
            };
            return View(viewModel);
		}

        public IActionResult ClassDetails(int id)
        {
            Student? student = GetStudentFromSession();
            if (student == null)
                return Unauthorized();
            
            Class? clss = GetClassById(id);
            if (clss == null)
                return NotFound();

			List<Resource> resources = GetResourcesFromClass(id);
			ViewData["resources"] = resources;

			return View(clss);
        }

		public IActionResult DownloadResource(int id)
		{
			Student? student = GetStudentFromSession();
			if (student == null)
				return Unauthorized();

			Resource? data = GetResourceDataFromId(id);
			if (data == null)
				return NotFound();

			HttpResponseMessage response = client
				.GetAsync("api/Resource/GetResourceFileById/" + id).Result;
			if (response.IsSuccessStatusCode)
			{
				var stream = response.Content.ReadAsStream();
				string name = data.Path;
				string contentType = data.ContentType;
				return File(stream, contentType, name);
			}
			else return NotFound();
		}
	}
}
