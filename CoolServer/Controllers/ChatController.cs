using CoolApiModels.Chats;
using CoolServer.MessageTransfer.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Users;
using CoolServer.Controllers.CModels;

namespace CoolServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ChatDetails> Get(NewUserDetails user)
        {
            return new ChatDetails();
        }
        [HttpGet("{id}")]
        public ActionResult<ChatShortDetails> GetByGuid(NewUserDetails user, Guid id)
        {
            return new ChatShortDetails();
        }
        [HttpPost("{user}")]
        public ActionResult<ChatShortDetails> Create(UserChat userChat)
        {
            return new ChatShortDetails();
        }
        [HttpGet("{number}/{offset}")]
        public ActionResult<ChatsPortionDetails> GetPortion(NewUserDetails user, int number, int offset)
        {
            return new ChatsPortionDetails();
        }
    }
}
