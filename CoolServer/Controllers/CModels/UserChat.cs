using CoolApiModels.Chats;
using CoolApiModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolServer.Controllers.CModels
{
    public class UserChat
    {
        public ChatShortDetails chat { get; set; }
        public NewUserDetails user { get; set; }
    }
}
