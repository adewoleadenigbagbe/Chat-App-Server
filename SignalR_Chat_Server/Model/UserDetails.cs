using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Chat_Server.Model
{
    public class UserDetails
    {
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public bool OnlineStatus { get; set; }
        public string GroupId { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }

    public class DataBaseSettings
    {
        public string ConnectionString { get; set; }
    }
}
