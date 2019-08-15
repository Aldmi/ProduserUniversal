﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebApi.SignalRClients;

namespace WebApi.Hubs
{
    public class ProviderHub : Hub
    {
        private readonly SignaRProduserClientsStorage _clientsStorage;

        #region ctor

        public ProviderHub(SignaRProduserClientsStorage clientsStorage)
        {
            _clientsStorage = clientsStorage;
        }

        #endregion




        #region OvverideMembers

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            _clientsStorage.AddClient(Context.ConnectionId, new SignaRClientsInfo(Context.ConnectionId, "SignalR Users"));
            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            _clientsStorage.RemoveClient(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        #endregion




        #region Client2ServerCall
        //DEBUG
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