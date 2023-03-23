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
        public IActionResult GetClasses()
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
        public IActionResult GetStudentsByClassId(int id)
        {
            var studentList = dbContext.Classes.Include(c => c.Students).
                SingleOrDefault(c => c.Id == id).Students.Select(s => new Student
                {
                    Id = s.Id,
                    Name = s.Name,
                });
            return Ok(studentList);
        }
        
        [HttpPost]
        [Route("{studentId}/{classId}")]
        public IActionResult AddStudentToClass(string studentId, int classId)
        {
            var student = dbContext.Students.SingleOrDefault(s => s.Id.Equals(studentId));
            var clss = dbContext.Classes.SingleOrDefault(c => c.Id == classId);
            clss.Students.Add(student);
            student.Classes.Add(clss);
            dbContext.SaveChanges();
            return Ok(student + " " + clss);
        }

        [HttpPost]
        public IActionResult AddClass(Class c)
        {
            var newClass = new Class()
            {
                Name = c.Name,
                TeacherId = c.TeacherId
            };
            dbContext.Classes.Add(newClass);
            dbContext.SaveChanges();
            return Ok(newClass);
        }
    }
}
