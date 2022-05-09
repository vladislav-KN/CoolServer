using CoolApiModels.Chats;
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
    /// <summary>
    /// Контроллер для управления чатами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        /// <summary>
        /// Передаёт чат пользователя по id
        /// </summary>
        /// <response code="200">Chat получен</response>
        /// <response code="400">Ошибки возникшие при попытке получить чата</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Chat), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<Chat> GetByGuid(Guid id)
        {
            return new Chat();
        }
        /// <summary>
        /// Создаёт чат пользователя
        /// </summary>
        /// <response code="200">Chat создан</response>
        /// <response code="400">Ошибки возникшие при попытке создать чата</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpPut]
        [ProducesResponseType(typeof(Chat), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<Chat> Create(Chat Chat)
        {
            return new Chat();
        }
        /// <summary>
        /// Удалить 
        /// </summary>
        /// <response code="200">Chat создан</response>
        /// <response code="400">Ошибки возникшие при попытке создать чата</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<bool> Delete(Chat Chat)
        {
            return true;
        }
        /// <summary>
        /// Получает список чатов заданного количества
        /// </summary>
        /// <response code="200">Chat получен</response>
        /// <response code="400">Ошибки возникшие при попытке получить чата</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpGet("{number}/{offset}")]
        [ProducesResponseType(typeof(Chat), 200)]
        [ProducesResponseType(typeof(IDictionary<string,string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<Chat>> GetPortion(User user, int number, int offset)
        {
            return new List<Chat>();
        }
    }
}
