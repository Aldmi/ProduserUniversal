using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using Autofac.Features.OwnedInstances;
using CSharpFunctionalExtensions;

namespace ProdusersMediator
{
    public class ProdusersUnion : IDisposable
    {
        #region fields

        private readonly ConcurrentDictionary<string, Owned<IProduser>> _produsersDict = new ConcurrentDictionary<string, Owned<IProduser>>();

        #endregion



        #region prop

        public ReadOnlyDictionary<string, IProduser> GetProduserDict => new ReadOnlyDictionary<string, IProduser>(_produsersDict.ToDictionary(d => d.Key, d => d.Value.Value));
        public int GetProdusersCount => _produsersDict.Count;

        #endregion



        #region ctor

        public ProdusersUnion()
        {
        }

        #endregion




        #region Methods

        public void AddProduser(string key, Owned<IProduser> value)
        {
            _produsersDict[key] = value;
        }


        public bool RemoveProduser(string key)
        {
            if (!_produsersDict.ContainsKey(key))
                return false;
                
            var owner= _produsersDict[key];
            var res= _produsersDict.TryRemove(key, out var val);         
            owner.Dispose();
            return res;
        }


        /// <summary>
        /// Отправить всем продюссерам
        /// </summary>
        public async Task<IList<Result<string, ErrorWrapper>>> SendAll(string message, string invokerName = null)
        {
            var tasks = _produsersDict.Values.Select(produser => produser.Value.Send(message, invokerName)).ToList();
            var results= await Task.WhenAll(tasks);
            return results;
        }


        /// <summary>
        /// Отправить продюсеру по ключу
        /// </summary>
        public async Task<Result<string, ErrorWrapper>> Send(string key, string message, string invokerName = null)
        {
           if(!_produsersDict.ContainsKey(key))
               throw new KeyNotFoundException(key);

           var result= await _produsersDict[key].Value.Send(message, invokerName);
           return result;
        }

        #endregion



        #region Disposable

        public void Dispose()
        {
            foreach (var prod in _produsersDict.Values)
            {
                prod.Dispose();
            }
        }

        #endregion
    }
}