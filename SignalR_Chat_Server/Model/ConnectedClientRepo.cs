using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Chat_Server.Model
{
    public class ConnectedClientRepo
    {
        private readonly static ConcurrentDictionary<string, UserDetails> _userOnlineDic = new ConcurrentDictionary<string, UserDetails>();

        public int Count
        {
            get
            {
                return _userOnlineDic.Count;
            }
        }


        public static IEnumerable<KeyValuePair<string, UserDetails>> GetConnections()
        {
            if (_userOnlineDic.Count != 0)
            {
                return _userOnlineDic.Select(o => o);
            }
            else
            {
                return null;
            }

        }

        public static void Add(string username, UserDetails UserDetails)
        {
            if ((!string.IsNullOrEmpty(username)) && UserDetails != null)
            {
                _userOnlineDic[username] = UserDetails;
            }
        }

        public static void Remove(string username)
        {
            UserDetails val;
            if (!string.IsNullOrEmpty(username))
            {
                _userOnlineDic.TryRemove(username, out val);
            }
        }

    }
}
