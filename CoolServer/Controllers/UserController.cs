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
    /// Контроллер для управления пользователь
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Поиск пользователей по логину
        /// </summary>
        /// <remarks>Логин может быть не полным</remarks>
        /// <response code="200">Пользователь найден</response>
        /// <response code="204">Пользователь не найден</response>
        /// <response code="400">Ошибки возникшие при попытке найти пользователей</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpGet("{portion}/{offset}")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(typeof(IEnumerable<User>), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<User>> Search(string login, int portion, int offset)
        {
            return new List<User>();
        }
        /// <summary>
        /// Вход пользователя
        /// </summary>
        /// <response code="200">Доступ предоставлен</response>
        /// <response code="400">Ошибки возникшие при попытке войти</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpGet("{login}/{password}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<User> Login(string login, string password)
        {
            return new User();
        }
        /// <summary>
        ///Регистрация
        /// </summary>
        /// <response code="200">Пользователь зарегистрирован</response>
        /// <response code="400">Ошибки возникшие при попытке зарегистрироваться</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpPut("{login}/{password}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<User> Registration(string login, string password)
        {
            return new User();
        }
        /// <summary>
        ///Новый пароль
        /// </summary>
        /// <response code="200">Пользователь обновил пароль</response>
        /// <response code="400">Ошибки возникшие при попытке сменить пароль</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpPost("{password}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult<User> NewPassword(User user, string password)
        {
            return new User();
        }
    }
}
