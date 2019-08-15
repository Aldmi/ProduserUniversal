using System;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using AbstractProduser.Options;
using CSharpFunctionalExtensions;

namespace WebClientProduser
{
    public class WebClientProduserWrapper : BaseProduser<WebClientProduserOption>
    {
        #region field


        #endregion



        #region ctor

        public WebClientProduserWrapper(WebClientProduserOption option) : base(option)
        {
        }

        #endregion



        #region OvverideMembers

        protected override IDisposable Owner { get; }

        #endregion



        protected override Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        protected override Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}