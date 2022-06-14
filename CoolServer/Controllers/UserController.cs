using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Users;
using CoolServer.Controllers.CModels;
using Microsoft.Extensions.Primitives;
using CoolServer.MessageTransfer;

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
        [HttpGet("{login}/{portion}/{offset}")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<User>>> SearchAsync(string login, int portion, int offset)
        {
            StringValues token;
            if (!Request.Headers.TryGetValue("Token", out token))
            {
                ProblemDetails problem = new ProblemDetails()
                {
                    Detail = "Server didn't receive Token ",
                    Status = 400,
                    Title = "Access denied"
                };
                return new ObjectResult(problem)
                {
                    StatusCode = 400
                };
            }
            var result = await RequestApi<UsersPortionDetails, Guid>.Get($"Users?offset={offset}&portion={portion}&searchString={login}", token.ToString());
            if (result.Item2 == System.Net.HttpStatusCode.OK)
            {
                List<User> users = new List<User>();
                if (!(result.Item1.Content is null))
                {
                    if (result.Item1.Content.Count() == 0)
                    {
                        return StatusCode(204);
                    }
                    foreach (var user in result.Item1.Content)
                    {
                        users.Add(new User() { Id = user.Id, Login = user.Login });

                    }
                    return users;
                }
                else
                {
                    return StatusCode(204);
                }
            }
            else
            {
                return new ObjectResult(result.Item2)
                {
                    StatusCode = 400
                };
            }
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
        public async Task<ActionResult<User>> LoginAsync(string login, string password)
        {
            var result = await RequestApi<AuthenticatedResult, Guid>.Get($"Users/auth?login={login}&password={password}");
            if (result.Item2 == System.Net.HttpStatusCode.OK)
            {
                Response.Headers.Add("Token",result.Item1.Token);
                User user = new User()
                {
                    Id = result.Item1.UserId,
                    Login = login
                };
                return user;
            }
            else
            {
                return new ObjectResult(result.Item2)
                {
                    StatusCode = 400
                };
            }
        }
        /// <summary>
        ///Регистрация
        /// </summary>
        /// <response code="200">Пользователь зарегистрирован</response>
        /// <response code="400">Ошибки возникшие при попытке зарегистрироваться</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpPost("{login}/{password}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> RegistrationAsync(string login, string password)
        {
           var result = await RequestApi<UserDetails, NewUserDetails>.Post(new NewUserDetails() { Login = login, Password = password},$"Users?login={login},password={password}");
            if (result.Item2 == System.Net.HttpStatusCode.OK)
            {
                User user = new User()
                {
                    Id = result.Item1.Id,
                    Login = result.Item1.Login
                };
                return user;
            }
            else
            {
                return new ObjectResult(result.Item2)
                {
                    StatusCode = 400
                };
            }
        }
        /// <summary>
        ///Новый пароль
        /// </summary>
        /// <response code="200">Пользователь обновил пароль</response>
        /// <response code="400">Ошибки возникшие при попытке сменить пароль</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpPut("{password}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<User>> NewPasswordAsync(User user, string password)
        {
            StringValues token;
            if (!Request.Headers.TryGetValue("Token", out token))
            {
                ProblemDetails problem = new ProblemDetails()
                {
                    Detail = "Server didn't receive Token ",
                    Status = 400,
                    Title = "Access denied"
                };
                return new ObjectResult(problem)
                {
                    StatusCode = 400
                };
            }
            var result = await RequestApi<UserDetails, UserNewDetails>.Put(new UserNewDetails() { NewLogin = user.Login, CurrentPassword = user.Password, NewPassword = password},$"Users?password={password}", token);
            if (result.Item2 == System.Net.HttpStatusCode.OK)
            {
                User newuser = new User()
                {
                    Id = result.Item1.Id,
                    Login = result.Item1.Login
                };
                return newuser;
            }
            else
            {
                return new ObjectResult(result.Item2)
                {
                    StatusCode = 400
                };
            }
 
        }
    }
}
