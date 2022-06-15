 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferLibrary.CModels;

namespace TransferLibrary
{
    [Serializable]
    public abstract class Transfer
    {
        public string Token { get; private set; }
        public ACTION Action { get; set; }
        public void SetToken(string t) 
        {
            Token = t;
        }

    }
    /// <summary>
    /// Вариации действий
    /// </summary>
    [Serializable]
    public enum ACTION
    {
        SEND,
        CHNG,
        DEL,
        GET
    }
    /// <summary>
    /// Json класс с сообщениями и амортизационными токенами
    /// </summary>
    [Serializable]
    public class TransferMessages: Transfer
    {
        /// <summary>
        /// передаваемое сообщение
        /// </summary>
        public Message Message {get;set;}

        public bool? ForAll { get; set; }
    }
}
