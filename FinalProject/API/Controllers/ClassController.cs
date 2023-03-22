using API.Models;
using API.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private Prn231dbContext dbContext = new Prn231dbContext();
        private readonly IMapper mapper;
        public ClassController()
        {
            var config = new MapperConfiguration(cf => cf.AddProfile(new ClassProfile())); ;
            mapper = config.CreateMapper();
        }
        
        [HttpGet]
        public IActionResult getClass()
        {
            List<Class> cList = dbContext.Classes
                .Include(o => o.Teacher).ToList();
            List<ClassDTO> classesList = new List<ClassDTO>();
            foreach(Class item in cList)
            {
                classesList.Add(mapper.Map<ClassDTO>(item));
            }
            classesList = cList.Select(mapper.Map<Class, ClassDTO>).ToList();
            return Ok(classesList);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult getStudentsByClassId(int id)
        {
            var studentList = dbContext.ClassStudents.
                Where(c => c.ClassId == id).Select(c => new
            {
                studentId = c.StudentId,
                studentName = c.Student.Name,
                /*Account = c.Student.Accounts.
                Select(s => new
                {
                    email = s.Email
                })*/
                }
            );
            return Ok(studentList);
        }
    }
}
