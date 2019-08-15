﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using AbstractProduser.Options;
using Autofac.Features.OwnedInstances;
using CSharpFunctionalExtensions;

namespace ProdusersMediator
{
    //TODO: Нужно разбить на 2 интерфейса:
    //1.IControlProdusersUnion (AddProduser, RemoveProduser) - Использовать в контроллере и при инициализации системы для редактирования списка Продюссеров.
    //2.ISenderProdusersUnion (SendAll, Send) - использовать в Device для отправки ответов.


    /// <summary>
    /// 
    /// </summary>
    public class ProdusersUnion : IDisposable
    {
        #region fields

        private readonly ProduserUnionOption _unionOption;
        private readonly ConcurrentDictionary<string, ProduserOwner> _produsersDict = new ConcurrentDictionary<string, ProduserOwner>();

        #endregion



        #region prop

        public ReadOnlyDictionary<string, IProduser<BaseProduserOption>> GetProduserDict => new ReadOnlyDictionary<string, IProduser<BaseProduserOption>>(_produsersDict.ToDictionary(d => d.Key, d => d.Value.Produser));
        public int GetProdusersCount => _produsersDict.Count;
        public string GetKey => _unionOption.Key;

        #endregion



        #region ctor

        //TODO: нужно только для DI, можно удалить 
        public ProdusersUnion()
        {
            
        }

        public ProdusersUnion(ProduserUnionOption unionOption)
        {
            _unionOption = unionOption;
        }

        #endregion



        #region Methods

        public void AddProduser(string key, IProduser<BaseProduserOption> value, IDisposable owner)
        {
              _produsersDict[key] = new ProduserOwner {Produser = value, Owner = owner};
        }


        public bool RemoveProduser(string key)
        {
            if (!_produsersDict.ContainsKey(key))
                return false;

            var owner = _produsersDict[key].Owner;
            var res = _produsersDict.TryRemove(key, out var val);
            owner.Dispose();
            return res;
        }


        /// <summary>
        /// Отправить всем продюссерам
        /// </summary>
        public async Task<IList<Result<string, ErrorWrapper>>> SendAll(string message, string invokerName = null)
        {
            //TODO: используя _unionOption.InterpreterTypeName интерпретировать message в этот тип и пережать как ответ.
            var tasks = _produsersDict.Values.Select(produserOwner => produserOwner.Produser.Send(message, invokerName)).ToList();
            var results = await Task.WhenAll(tasks);
            return results;
        }


        /// <summary>
        /// Отправить продюсеру по ключу
        /// </summary>
        public async Task<Result<string, ErrorWrapper>> Send(string key, string message, string invokerName = null)
        {
            if (!_produsersDict.ContainsKey(key))
                throw new KeyNotFoundException(key);

            var result = await _produsersDict[key].Produser.Send(message, invokerName);
            return result;
        }

        #endregion



        #region Disposable

        public void Dispose()
        {
            foreach (var produserOwner in _produsersDict.Values)
            {
                produserOwner.Owner.Dispose();
            }
        }

        #endregion



        #region NestedClasses

        private class ProduserOwner
        {
            public IProduser<BaseProduserOption> Produser { get; set; }
            public IDisposable Owner { get; set; }
        }

        #endregion
    }
}