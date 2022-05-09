using CoolServer.Controllers.CModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolServer.MessageTransfer
{
    /// <summary>
    /// Json класс с сообщениями и амортизационными токенами
    /// </summary>
    public class JsonMessages
    {
        /// <summary>
        /// передаваемое сообщение
        /// </summary>
        public Message Message {get;set;}
        /// <summary>
        /// Токен с авторизованными данными
        /// </summary>
        public string token;
    }
}
