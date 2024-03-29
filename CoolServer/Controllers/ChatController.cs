﻿using CoolApiModels.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Users;
using CoolServer.MessageTransfer;
using Microsoft.Extensions.Primitives;
using TransferLibrary.CModels;

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
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Chat>> GetByGuidAsync(Guid id)
        {
            StringValues token;
            if(!Request.Headers.TryGetValue("Token", out token))
            {
                ProblemDetails problem = new ProblemDetails()
                {
                    Detail = "Server didn't receive Token ",
                    Status = 400,
                    Title = "Access denied"
                };
                return new ObjectResult(problem)
                {
                    StatusCode= 400
                };
            }
            var result = await RequestApi<ChatDetails, Guid>.Get($"Chats/{id}", token.ToString()) ;
            if (result.Item1 != null)
            {
                List<User> chatUsers = new List<User>();
                foreach (var us in result.Item1.ChatMembers)
                { 
                    chatUsers.Add(new User() { Id = us.Id, Login = us.Login }); 
                }
                return new Chat() { ChatMembers = chatUsers, Id = result.Item1.Id, CreationTimeLocal = result.Item1.CreationTimeUtc };
            }
            else
            {
                return new ObjectResult(result.Item3)
                {
                    StatusCode = (int)result.Item2
                };
            }
            
        }
        /// <summary>
        /// Создаёт чат пользователя
        /// </summary>
        /// <response code="200">Chat создан</response>
        /// <response code="400">Ошибки возникшие при попытке создать чата</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpPut]
        [ProducesResponseType(typeof(Chat), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Chat>> CreateAsync(Chat Chat)
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
                    StatusCode= 400
                };
            }
            NewChatDetails newChat = new NewChatDetails()
            {
                ReceiverId = Chat.ChatMembers.First().Id
            };
            var result = await RequestApi<ChatDetails, NewChatDetails>.Post(newChat, $"Chats", token.ToString());
            if (result.Item1 != null)
            {
                List<User> chatUsers = new List<User>();
                foreach (var us in result.Item1.ChatMembers)
                    chatUsers.Add(new User() { Id = us.Id, Login = us.Login });
                return new Chat() { ChatMembers = chatUsers, Id = result.Item1.Id, CreationTimeLocal = result.Item1.CreationTimeUtc };
            }
            else
            {
                return new ObjectResult(result.Item3)
                {
                    StatusCode = (int)result.Item2
                };
            }
        }
        /// <summary>
        /// Удалить 
        /// </summary>
        /// <response code="200">Chat создан</response>
        /// <response code="400">Ошибки возникшие при попытке создать чата</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DeleteAsync(Guid chatId)
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
                    StatusCode= 400
                };
            }
           
            var result = await RequestApi<bool, bool>.Delete($"Chats/{chatId}",token.ToString());
            if (result.Item1 == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return new ObjectResult(result.Item2)
                {
                    StatusCode = (int)result.Item1
                };
            }
            
        }
        /// <summary>
        /// Получает список чатов заданного количества
        /// </summary>
        /// <response code="200">Chat получен</response>
        /// <response code="400">Ошибки возникшие при попытке получить чата</response>
        /// <response code="500">Сервер не отвечает</response>
        [HttpGet("{portion}/{offset}")]
        [ProducesResponseType(typeof(IEnumerable<Chat>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Chat>>> GetPortionAsync(int portion, int offset)
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
                    StatusCode= 400
                };
            }
            var result = await RequestApi<ChatsPortionDetails, Guid>.Get($"Chats?offset={offset}&portion={portion}", token.ToString());
            if (result.Item2 == System.Net.HttpStatusCode.OK)
            {
                List<Chat> chats = new List<Chat>();
                if (!(result.Item1.Content is null))
                {

                    if (result.Item1.Content.Count() == 0)
                    {
                        return StatusCode(204);
                    }
                    foreach (var chat in result.Item1.Content)
                    {
                        var resultGet = await RequestApi<ChatDetails, Guid>.Get($"Chats/{chat.Id}", token.ToString());
                        if (resultGet.Item1 != null)
                        {
                            List<User> chatUsers = new List<User>();
                            foreach (var us in resultGet.Item1.ChatMembers)
                            {
                                chatUsers.Add(new User() { Id = us.Id, Login = us.Login });
                            }
                            chats.Add(new Chat() { ChatMembers = chatUsers, Id = resultGet.Item1.Id, CreationTimeLocal = resultGet.Item1.CreationTimeUtc });
                        }
                    }
                    if (chats.Count == 0)
                    {
                        return StatusCode(204);
                    }
                    return chats;
                }
                else
                {
                    return StatusCode(204);
                }
               
            }
            else
            {
                return new ObjectResult(result.Item3)
                {
                    StatusCode = (int)result.Item2
                };
            }
          
        }
    }
}
