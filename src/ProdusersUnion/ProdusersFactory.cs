using System;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Enums;
using AbstractProduser.Options;
using Autofac;
using Autofac.Features.OwnedInstances;

namespace ProdusersMediator
{
    /// <summary>
    /// Фабрика по созданию продюссеров из опций.
    /// ДОЛЖНА БЫТЬ ЗАРЕГИСТРИРОВННА В DI AUTOFAC ДЛЯ ПЕРЕДАЧИ ЗАВИСТМОСТИ ILifetimeScope
    /// </summary>
    public class ProdusersFactory
    {
        private readonly ILifetimeScope _scope;


        #region ctor

        public ProdusersFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        #endregion



        /// <summary>
        /// Добавляет созданные на базе опций продюссеры к ProdusersUnion
        /// </summary>
        /// <param name="optionAgregator"></param>
        public void FillProduserUnionByOptionAgregator(ProduserOptionAgregator optionAgregator)
        {
            var prodUnionServ = _scope.Resolve<ProdusersUnion>();
            foreach (var option in optionAgregator.KafkaProduserOptions)
            {
                var prod = Create(option);
                prodUnionServ.AddProduser(option.Key, prod);
            }
            foreach (var option in optionAgregator.SignalRProduserOptions)
            {
                var prod = Create(option);
                prodUnionServ.AddProduser(option.Key, prod);
            }
        }


        public ProduserOptionAgregator GetProduserUnionOptionAgregator()
        {
            var agregator = new ProduserOptionAgregator();
            var prodUnionServ = _scope.Resolve<ProdusersUnion>();
            var produsers = prodUnionServ.GetProduserDict.Values;
            foreach (var prod in produsers)
            {
                var option = prod.GetProduserOption<BaseProduserOption>();
                switch (option.ProduserType)
                {
                    case ProduserType.Kafaka:
                        var kafkaOption = option as KafkaProduserOption;
                        agregator.KafkaProduserOptions.Add(kafkaOption);
                        break;

                    case ProduserType.SerilogLoger:
                        break;

                    case ProduserType.SignalR:
                        var signalROption = option as SignalRProduserOption;
                        agregator.SignalRProduserOptions.Add(signalROption);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return agregator;
        }



        public bool DeleteProduserUnionByOptionAgregator(string key)
        {
            var prodUnionServ = _scope.Resolve<ProdusersUnion>();
            var res = prodUnionServ.RemoveProduser(key);
            return res;
        }



        /// <summary>
        /// Добавить продюссера
        /// </summary>
        /// <typeparam name="T">тип продюссера</typeparam>
        /// <param name="option">настройки продюссера</param>
        /// <returns></returns>
        public Owned<IProduser> Create<T>(T option) where T : BaseProduserOption
        {
            var factory = _scope.ResolveKeyed<Func<T, Owned<IProduser>>>(option.ProduserType);
            var owner = factory(option);
            return owner;
        }
    }
}