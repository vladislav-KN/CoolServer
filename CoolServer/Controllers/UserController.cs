using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Users;
namespace CoolServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{portion}/{offset}")]
        public ActionResult<UsersPortionDetails> Contacts(UserDetails user, int portion, int offset)
        {
            return new UsersPortionDetails();
        }
        [HttpGet("{login}/{password}")]
        public ActionResult<NewUserDetails> Login(string login, string password)
        {
            return new NewUserDetails();
        }
        [HttpPut("{login}/{password}")]
        public ActionResult<NewUserDetails> Registration(string login, string password)
        {
            return new NewUserDetails();
        }
        [HttpPost("{password}")]
        public ActionResult<UserDetails> NewPassword(UserNewDetails user, string password)
        {
            return new UserDetails();
        }
    }
}
