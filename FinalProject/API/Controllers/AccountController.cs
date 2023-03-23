using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private Prn231dbContext dbContext = new Prn231dbContext();

        [HttpGet]
        [Route("{email}")]
        public IActionResult GetAccountByEmail(string email)
        {
            var account = dbContext.Accounts
                .SingleOrDefault(a => a.Email.Equals(email));
            return Ok(account);
        }
        [HttpGet]
        public IActionResult GetStudentByAccountId(int id)
        {
            var account = dbContext.Accounts.SingleOrDefault(a => a.Id == id);
            var student = dbContext.Students
                .Select(s => new Student
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .SingleOrDefault(s => s.Id.Equals(account.StudentId))
                ;
            return Ok(student);
        }
    }
}
