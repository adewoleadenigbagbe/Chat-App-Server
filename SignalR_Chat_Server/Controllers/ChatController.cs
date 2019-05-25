using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR_Chat_Server.Concrete;
using SignalR_Chat_Server.Model;

namespace SignalR_Chat_Server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _hub;
        public ChatController(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }


    }
}