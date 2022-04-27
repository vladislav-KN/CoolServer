using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Chats;
using CoolServer.Interfaces;

namespace CoolServer.MessageTransfer.DataModels
{
    public class Chat:IData
    {
        byte[] buffer;
        public byte[] Buffer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Chat(int buffSize = 1024)
        {
            buffer = new byte[buffSize];
        }
        ChatDetails ChatDet { get; set; }
 
        ChatShortDetails ChatShort { get; set; }
        ChatsPortionDetails ChatPortionc { get; set; }
        public void ToByte(object data)
        {
            throw new NotImplementedException();
        }

        public object ToObject()
        {
            throw new NotImplementedException();
        }
    }
}
