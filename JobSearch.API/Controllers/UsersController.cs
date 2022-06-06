using JobSearch.API.Database;
using JobSearch.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JobSearch.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private JobSearchContext _data;
        public UsersController(JobSearchContext data)
        {
            _data = data;
        }

        // http://seusite.com.br/api/Users?email=teste@hotmail.com&password=123456
        //                          Users é o controller(Ponte), que pega todas as informações na api
        [HttpGet]
        public IActionResult GetUser(string email, string password)
        {
            User userDb = _data.Users.FirstOrDefault(a => a.Email == email && a.Password == password);
            if(userDb == null)
            {
                return NotFound(); // http - 404 - Não foi encontrado!
            }

            return new JsonResult(userDb);
        }  
      
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            //TODO - Validação...
            _data.Users.Add(user);
            _data.SaveChanges(); //Unit of work

            return CreatedAtAction(nameof(GetUser), new { email = user.Email, password = user.Password }, user); //200 - OK 201 - Created
        }
    }
}
