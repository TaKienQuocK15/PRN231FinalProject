using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private Prn231dbContext dbContext = new Prn231dbContext();
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = dbContext.Students;
            return Ok(students);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetStudentById(string id)
        {
            var student = dbContext.Students.SingleOrDefault(s => s.Id.Equals(id));
            return Ok(student);
        }

        [HttpPost]
        public IActionResult AddStudent(Student s)
            {
                var student = new Student()
                {
                    Id = s.Id,
                    Name = s.Name
                };
                dbContext.Students.Add(student);
                dbContext.SaveChanges();
                return Ok(student);
            }
    }
}
