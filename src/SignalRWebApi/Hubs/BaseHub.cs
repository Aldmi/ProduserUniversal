using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRWebApi.Hubs
{
    public class BaseHub : Hub
    {



        #region OvverideMembers

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        #endregion




        #region Client2ServerCall

        //RPC---------------------------------------------------------------------------
        public async Task<string> SendMessage(string user, string message)
        {
            //throw new Exception("dsdsd");   //в JS клиенте перехватывается .catch(function(err).......)
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            return "454545454545";
        }

        #endregion
    }
}