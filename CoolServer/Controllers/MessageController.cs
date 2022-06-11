using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Messages;
using CoolApiModels.Chats;
using CoolServer.Controllers.CModels;

namespace CoolServer.Controllers
{
    /// <summary>
    /// Контроллер для управления сообщения
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {   /// <summary>
        /// Получает заданное количество сообщений в чате
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Сообщения получены</response>
        /// <response code="400">Ошибки возникшие при попытке получить сообщения</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpGet("{number}/{offset}")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<Message>> Get(Chat chat, int number, int offset)
        {
            return new List<Message>();
        }
    }
}
