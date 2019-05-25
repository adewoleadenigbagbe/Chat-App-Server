using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SignalR_Chat_Server.DAO;
using SignalR_Chat_Server.Interfaces;
using SignalR_Chat_Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Chat_Server.Concrete
{
    public class ChatHub : Hub
    {
        private readonly ChatRepository _chatRepository;
        public ChatHub(IOptions<DataBaseSettings> options)
        {
            _chatRepository = new ChatRepository(options);
        }
        public async Task Broadcast(string messagetoSend, string userName)
        {
            var connectionId = Context.ConnectionId;
            var httpContext = Context.GetHttpContext();
            var queryStrings = httpContext.Request.Query;

            var category = queryStrings["group"];           
            var date = DateTime.Now.ToString("hh:mm tt");
            var senderName = userName;

            await Clients.Group(category).SendAsync("OnSendtoClient", senderName, date, messagetoSend, connectionId);
        }
        public async Task OnTyping(string name)
        {
            var httpContext = Context.GetHttpContext();
            var queryStrings = httpContext.Request.Query;

            var category = queryStrings["group"];

            await Clients.GroupExcept(category, Context.ConnectionId).SendAsync("GetWhoTypes", name);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();

                var queryStrings = httpContext.Request.Query;

                var userName = queryStrings["username"];
                var category = queryStrings["group"];

                var userDetails = new UserDetails
                {
                    UserName = userName,
                    ConnectionId = Context.ConnectionId,
                    GroupId = category,
                    OnlineStatus = false
                };
                var date = DateTime.Now.ToString("hh:mm tt");

                var result = _chatRepository.CreateConnectionDetails(userDetails).Result;

                if (result == true)
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, category);
                    var message = string.Format("{0} entered the group", userName);
                    
                    Clients.GroupExcept(category, Context.ConnectionId).SendAsync("ConnectedAlert", message,userName,date);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return base.OnConnectedAsync();

        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();

            var queryStrings = httpContext.Request.Query;

            var userName = queryStrings["username"];
            var category = queryStrings["group"];


            var result = _chatRepository.UpdateOnlineStatus(userName).Result;
            if(result == true)
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, category);
                var message = string.Format("{0} left the group", userName);
                Clients.GroupExcept(category, Context.ConnectionId).SendAsync("ConnectedAlert", message);
            }

            return base.OnDisconnectedAsync(exception);

        }

    }
}
