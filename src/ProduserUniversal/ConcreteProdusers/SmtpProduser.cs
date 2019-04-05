using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProduserUniversal.AbstractProduser;
using ProduserUniversal.Helpers;

namespace ProduserUniversal.ConcreteProdusers
{
    /// <summary>
    /// Отправка на Email.
    /// </summary>
    public class SmtpProduser : BaseProduser
    {
        public SmtpProduser(TimeSpan timeRequest, int trottlingQuantity) : base(timeRequest, trottlingQuantity)
        {
        }

        protected override Task<Result<string, ErrorWrapper>> SendConcrete(string message, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        protected override Task<Result<string, ErrorWrapper>> SendConcrete(object message, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}