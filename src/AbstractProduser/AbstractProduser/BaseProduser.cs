using System;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.Helpers;
using AbstractProduser.Options;
using CSharpFunctionalExtensions;

namespace AbstractProduser.AbstractProduser
{
    public abstract class BaseProduser : IProduser
    {
        #region field

        private readonly BaseProduserOption _baseOption;
        private TimeSpan _timeRequest;
   
        #endregion



        #region prop

        public TrottlingCounter TrottlingCounter { get; set; }

        #endregion



        #region ctor

        protected BaseProduser(BaseProduserOption baseOption)
        {
            _baseOption = baseOption;
            _timeRequest = baseOption.TimeRequest;
            TrottlingCounter= new TrottlingCounter(baseOption.TrottlingQuantity);
        }

        #endregion



        #region Methode

        /// <summary>
        /// Вернуть настройки провайдера
        /// </summary>
        /// <typeparam name="T">тип опций провайдера</typeparam>
        public T GetProduserOption<T>() where T: BaseProduserOption
        {
            return _baseOption as T;
        }

        public async Task<Result<string, ErrorWrapper>> Send(string message, string invokerName = null)
        {
            TrottlingCounter++;
            if(TrottlingCounter.IsTrottle)
               return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.Trottling));

            var cts = new CancellationTokenSource(_timeRequest);
            try
            {
                var res = await SendConcrete(message, invokerName, cts.Token);
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


        public async Task<Result<string, ErrorWrapper>> Send(Object obj, string invokerName = null)
        {
            TrottlingCounter++;
            if (TrottlingCounter.IsTrottle)
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(ResultError.Trottling));

            var cts = new CancellationTokenSource(_timeRequest);
            try
            {
                var res = await SendConcrete(obj, invokerName, cts.Token);
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

        protected abstract IDisposable Owner { get; }
        protected abstract Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken));
        protected abstract Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken));

        #endregion



        #region Disposable

        public void Dispose()
        {
           Owner?.Dispose();
        }

        #endregion
    }
}