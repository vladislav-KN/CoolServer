using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolApiModels.Messages;
using CoolServer.Interfaces;

namespace CoolServer.MessageTransfer.DataModels
{
    public class Message:IData
    {
        byte[] buffer;
        public byte[] Buffer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Message(int buffSize = 1024)
        {
            buffer = new byte[buffSize];
        }
        MessageDetails messageDet { get; set; }
        MessageNewDetails messageDetNew { get; set; }
        MessageShortDetails messageShortDet { get; set; }
        MessagesPortionDetails portion { get; set; }

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
