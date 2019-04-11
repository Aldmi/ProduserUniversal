using System;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using AbstractProduser.Options;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Produsers
{
    public class SignalRProduserWrapper : BaseProduser
    {
        private readonly IHubContext<ProviderHub> _hubProxy;
        private readonly SignalRProduserOption _option;



        #region ctor

        public SignalRProduserWrapper(IHubContext<ProviderHub> hubProxy, SignalRProduserOption option) 
            : base(option.TimeRequest, option.TrottlingQuantity)
        {
            _hubProxy = hubProxy;
            _option = option;
        }

        #endregion

        
    
        #region OvverideMembers

        protected override IDisposable Owner => null;


        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                //_hubProxy.Clients.
                 invokerName = invokerName ?? _option.MethodeName;
                await _hubProxy.Clients.All.SendCoreAsync(invokerName, new object[] { message }, ct);
                return Result.Ok<string, ErrorWrapper>("Ok");
            }
            catch (Exception ex)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.RespawnProduserError, ex));
            }
        }


        protected override Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}