﻿using CoolServer.MessageTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferLibrary;

namespace CoolServer.Interfaces
{
    public interface IConnection
    {
        public abstract void SendData(Transfer data);
    }
 
}
