﻿using System;
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

        protected abstract Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken));
        protected abstract Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken));

        #endregion



        #region Disposable

        public void Dispose()
        {
           _owner?.Dispose();
        }

        #endregion
    }
}