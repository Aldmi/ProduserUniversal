using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRWebApi.Hubs;

namespace SignalRWebApi.HubsWrappers
{
    public class HubWrapper
    {
        private readonly IHubContext<BaseHub> _baseHubContext;



        public HubWrapper(IHubContext<BaseHub> baseHubContext)
        {
            _baseHubContext = baseHubContext;
        }


        public async Task SendMessage(string methode, string message)
        {
            try
            {
                await _baseHubContext.Clients.All.SendAsync(methode, "UserRX", message);
            }
            catch (Exception e)
            {

            }
        }
    }
}