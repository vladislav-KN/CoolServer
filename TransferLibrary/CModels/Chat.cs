using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransferLibrary.CModels
{
    /// <summary>
    /// Чат пользователя
    /// </summary>
    [Serializable]
    public class Chat
    {
        /// <summary>
        /// ID чата
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime CreationTimeLocal { get; set; }
        /// <summary>
        /// Участники чата
        /// </summary>
        public IEnumerable<User> ChatMembers { get; set; }
    }
}
