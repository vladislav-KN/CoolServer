using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Messages;
using CoolApiModels.Users;
namespace CoolServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpGet("{number}/{offset}")]
        public ActionResult<MessagesPortionDetails> Get(UserDetails user, int number, int offset)
        {
            return new MessagesPortionDetails();
        }
        [HttpGet("{id}")]
        public ActionResult<MessageShortDetails> GetById(UserDetails user, Guid id)
        {
            return new MessageShortDetails();
        }
    }
}
