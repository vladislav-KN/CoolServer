using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolServer.Controllers.CModels
{
    /// <summary>
    /// Сообщение пользователя
    /// </summary>
    public class Message
    {
        /// <summary>
        /// ID сообщения
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ID отправителя
        /// </summary>
        public Guid SenderId { get; set; }
        /// <summary>
        /// Просмотрено
        /// </summary>
        public bool? IsViewed { get; set; }
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Количество прикреплённых файлов
        /// </summary>
        public int AttachmentsCount { get; set; }
        /// <summary>
        /// Прикреплённые файлы
        /// </summary>
        public IEnumerable<string> Attachments { get; set; }
    }
}
