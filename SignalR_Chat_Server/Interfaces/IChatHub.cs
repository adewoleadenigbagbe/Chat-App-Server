using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_Chat_Server.Model;

namespace SignalR_Chat_Server.Interfaces
{
    public interface IChatHub
    {
        Task Broadcast(MessageInfo messageInfo);
        Task OnTyping(string nav = "");

    }
}
