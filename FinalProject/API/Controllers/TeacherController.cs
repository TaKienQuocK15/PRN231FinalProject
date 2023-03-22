using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private Prn231dbContext dbContext = new Prn231dbContext();

        [HttpGet]
        public IActionResult getTeachers()
        {
            return Ok(dbContext.Teachers.ToList());
        }
        /*[HttpPost]
        public IActionResult addTeacher(Teacher t)
        {
            var teacher = new Teacher()
            {
                Name = t.Name
            };
            dbContext.Teachers.Add(teacher);
            dbContext.SaveChanges();
            return Ok(teacher);
        }*/
    }
}
