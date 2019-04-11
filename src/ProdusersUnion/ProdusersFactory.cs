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
        public void FillProduserUnion(ProduserOptionAgregator optionAgregator)
        {
            var prodUnionServ = _scope.Resolve<ProdusersUnion>();
            foreach (var option in optionAgregator.KafkaProduserOptions)
            {
               var prod= Create(option);
               prodUnionServ.AddProduser(option.Name, prod);
            }
            foreach (var option in optionAgregator.SignalRProduserOptions)
            {
                var prod = Create(option);
                prodUnionServ.AddProduser(option.Name, prod);
            }
        }



        /// <summary>
        /// Добавить продюссера
        /// </summary>
        /// <typeparam name="T">тип продюссера</typeparam>
        /// <param name="option">настройки продюссера</param>
        /// <returns></returns>
        public Owned<IProduser> Create<T>(T option)  where T : BaseProduserOption 
        {
            var factory = _scope.ResolveKeyed<Func<T, Owned<IProduser>>>(option.ProduserType);
            var owner = factory(option);
            return owner;
        }
    }
}