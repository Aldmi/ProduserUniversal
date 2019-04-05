using System;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.Helpers;
using CSharpFunctionalExtensions;

namespace AbstractProduser.AbstractProduser
{
    public abstract class BaseProduser : IProduser
    {
        #region field

        private TimeSpan _timeRequest;
        protected IDisposable _owner;

        #endregion



        #region prop

        public TrottlingCounter TrottlingCounter { get; set; }

        #endregion



        #region ctor

        protected BaseProduser(TimeSpan timeRequest, int trottlingQuantity)
        {
            _timeRequest = timeRequest;
            TrottlingCounter= new TrottlingCounter(trottlingQuantity);
        }

        #endregion




        #region Methode

        //public Result<FailRespawn> Send11(string message)
        //{

        //    return Result<Fai;
        //}



        public async Task<Result<string, ErrorWrapper>> Send(string message)
        {
            TrottlingCounter++;
            if(TrottlingCounter.IsTrottle)
               return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.Trottling));

            var cts = new CancellationTokenSource(_timeRequest);
            try
            {
                var res = await SendConcrete(message, cts.Token);
                return res;
            }
            catch (TaskCanceledException)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.Timeout));
            }
            catch (Exception ex)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.SendException, ex));
            }
            finally
            {
                cts.Dispose();
                TrottlingCounter--;
            }   
        }


        public async Task<Result<string, ErrorWrapper>> Send(object obj)
        {
            TrottlingCounter++;
            if (TrottlingCounter.IsTrottle)
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.Trottling));

            var cts = new CancellationTokenSource(_timeRequest);
            try
            {
                var res = await SendConcrete(obj, cts.Token);
                return res;
            }
            catch (TaskCanceledException)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.Timeout));
            }
            catch (Exception ex)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.SendException, ex));
            }
            finally
            {
                cts.Dispose();
                TrottlingCounter--;
            }
        }

        #endregion



        #region AbstractMembers

        protected abstract Task<Result<string, ErrorWrapper>> SendConcrete(string message, CancellationToken ct);
        protected abstract Task<Result<string, ErrorWrapper>> SendConcrete(object message, CancellationToken ct);

        #endregion



        #region Disposable

        public void Dispose()
        {
           _owner?.Dispose();
        }

        #endregion
    }
}