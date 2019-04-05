using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProduserUniversal.AbstractProduser;
using ProduserUniversal.Helpers;

namespace ProduserUniversal.Services
{
    public class ProdusersUnionService : IDisposable
    {
        #region fields

        private readonly ConcurrentDictionary<string, IProduser> _produsersDict = new ConcurrentDictionary<string, IProduser>();

        #endregion



        #region prop

        public ReadOnlyDictionary<string, IProduser> GetProduserDict => new ReadOnlyDictionary<string, IProduser>(_produsersDict);
        public int GetProdusersCount => _produsersDict.Count;

        #endregion



        #region ctor

        public ProdusersUnionService()
        {
           
        }

        #endregion




        #region Methods

        public void AddProduser(string key, IProduser value)
        {
            _produsersDict[key] = value;
        }


        public bool RemoveProduser(string key)
        {
            return _produsersDict.TryRemove(key, out var val);
        }


        /// <summary>
        /// Отправить всем продюссерам
        /// </summary>
        public async Task<IList<Result<string, ErrorWrapper>>> SendAll(string message)
        {
            var tasks = _produsersDict.Values.Select(produser => produser.Send(message)).ToList();
            var results= await Task.WhenAll(tasks);
            return results;
        }


        /// <summary>
        /// Отправить продюсеру по ключу
        /// </summary>
        public async Task<Result<string, ErrorWrapper>> Send(string key, string message)
        {
           if(!_produsersDict.ContainsKey(key))
               throw new KeyNotFoundException(key);

           var result= await _produsersDict[key].Send(message);
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