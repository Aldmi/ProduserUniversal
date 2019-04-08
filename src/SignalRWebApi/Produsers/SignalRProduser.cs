using System;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Produsers
{
    public class SignalRProduser : BaseProduser
    {
        private readonly IHubContext<BaseHub> _hubContext;



        public SignalRProduser(TimeSpan timeRequest, int trottlingQuantity, IHubContext<BaseHub> hubContext) 
            : base(timeRequest, trottlingQuantity)
        {
            _hubContext = hubContext;
        }

 


        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(string message, CancellationToken ct)
        {
            try
            {
                await _hubContext.Clients.All.SendCoreAsync("ReceiveMessage", new object[] { "user", message }, ct);
                return Result.Ok<string, ErrorWrapper>("");
            }
            catch (Exception ex)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.RespawnProduserError, ex));
            }
        }


        protected override Task<Result<string, ErrorWrapper>> SendConcrete(object message, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {

        }
    }
}