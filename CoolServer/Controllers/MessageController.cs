﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Messages;
using CoolApiModels.Chats;
using Microsoft.Extensions.Primitives;
using CoolServer.MessageTransfer;
using CoolApiModels.Users;
using TransferLibrary.CModels;

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
        [HttpGet("{chatId}/{portion}/{offset}")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Message>>> GetAsync(Guid chatId, int portion, int offset)
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
            var result = await RequestApi<MessagesPortionDetails, Guid>.Get($"Messages?chatId={chatId}&offset={offset}&portion={portion}", token.ToString());
            if (result.Item2 == System.Net.HttpStatusCode.OK)
            {
                List<Message> messages = new List<Message>();
                if (!(result.Item1.Content is null))
                {
                    if (result.Item1.Content.Count() == 0)
                    {
                        return StatusCode(204);
                    }
                    foreach (var message in result.Item1.Content)
                    {
                        var user = await RequestApi<UserDetails, Guid>.Get($"Users/{message.SenderId}", token.ToString());
                        if (user.Item2 == System.Net.HttpStatusCode.OK)
                        {
                            messages.Add(new Message()
                            {
                                Id = message.Id,
                                AttachmentsCount = message.AttachmentsCount,
                                IsViewed = message.IsViewed,
                                Sender = new User() { Id = user.Item1.Id, Login = user.Item1.Login },
                                Text = message.Text
                            }
                        );
                        }

                    }
                    return messages;
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
                    StatusCode = (int)result.Item2
                };
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Message), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Message>> GetMessageAsync(Guid id)
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
            var result = await RequestApi<MessageDetails, Guid>.Get($"Messages?id={id}", token.ToString());
            if (result.Item2 == System.Net.HttpStatusCode.OK)
            {
                var user = await RequestApi<UserDetails, Guid>.Get($"Users/{ result.Item1.SenderId}", token.ToString());
                if (user.Item2 == System.Net.HttpStatusCode.OK)
                {

                    Message message = new Message()
                    {
                        Id = result.Item1.Id,
                        Attachments = result.Item1.Attachments,
                        IsViewed = result.Item1.IsViewed,
                        Sender = new User() { Id = user.Item1.Id, Login = user.Item1.Login },
                        Text = result.Item1.Text,
                    };
                    return message;
                }
                return StatusCode(204);
            }
            
            else
            {
                return new ObjectResult(result.Item2)
                {
                    StatusCode = (int)result.Item2
                };
            }
        }
    }
}
