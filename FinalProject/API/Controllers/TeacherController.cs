﻿using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private Prn231dbContext dbContext = new Prn231dbContext();

        [HttpGet]
        public IActionResult GetTeachers()
        {
            return Ok(dbContext.Teachers.ToList());
        }
        [HttpPost]
        public IActionResult AddTeacher(Teacher t)
        {
            var teacher = new Teacher()
            {
                Name = t.Name
            };
            dbContext.Teachers.Add(teacher);
            dbContext.SaveChanges();
            return Ok(teacher);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTeacherById(int id)
        {
            return Ok(dbContext.Teachers.FirstOrDefault(t => t.Id == id));
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetClassesByTeacherId(int id)
        {
            var classList = dbContext.Teachers.
                Include(s => s.Classes).
                SingleOrDefault(c => c.Id.Equals(id)).
                Classes.Select(s => new Class
                {
                    Id = s.Id,
                    Name = s.Name
                });
            return Ok(classList);
        }
    }
}
