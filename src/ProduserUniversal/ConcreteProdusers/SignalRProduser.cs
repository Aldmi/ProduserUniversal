using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProduserUniversal.AbstractProduser;
using ProduserUniversal.Helpers;

namespace ProduserUniversal.ConcreteProdusers
{
    public class SignalRProduser : BaseProduser
    {



        #region ctor

        public SignalRProduser(TimeSpan timeRequest, int trottlingQuantity) : base(timeRequest, trottlingQuantity)
        {
        }

        #endregion



        #region OvverideMembers

        protected override Task<Result<string, ErrorWrapper>> SendConcrete(string message, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        protected override Task<Result<string, ErrorWrapper>> SendConcrete(object message, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}