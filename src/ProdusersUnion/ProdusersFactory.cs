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
    /// </summary>
    public class ProdusersFactory<TIn>
    {
        private readonly ProdusersUnion<TIn> _produsersUnion;
        private readonly Func<SignalRProduserOption, Owned<IProduser<SignalRProduserOption>>> _signalRFactory;
        private readonly Func<KafkaProduserOption, Owned<IProduser<KafkaProduserOption>>> _kafkaFactory;
        private readonly Func<WebClientProduserOption, Owned<IProduser<WebClientProduserOption>>> _webClientFactory;


        #region ctor

        public ProdusersFactory(ProdusersUnion<TIn> produsersUnion,
                                Func<SignalRProduserOption, Owned<IProduser<SignalRProduserOption>>> signalRFactory,
                                Func<KafkaProduserOption, Owned<IProduser<KafkaProduserOption>>> kafkaFactory,
                                Func<WebClientProduserOption, Owned<IProduser<WebClientProduserOption>>> webClientFactory)
        {
            _produsersUnion = produsersUnion;
            _signalRFactory = signalRFactory;
            _kafkaFactory = kafkaFactory;
            _webClientFactory = webClientFactory;
        }

        #endregion



        #region Methode

        /// <summary>
        /// Добавляет созданные на базе опций продюссеры к ProdusersUnion
        /// </summary>
        /// <param name="unionOption"></param>
        public void FillProduserUnionByOptionAgregator(ProduserUnionOption unionOption)
        {
            foreach (var option in unionOption.KafkaProduserOptions)
            {
                var prod = _kafkaFactory(option);
                _produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }
            foreach (var option in unionOption.SignalRProduserOptions)
            {
                var prod = _signalRFactory(option);
                _produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }
            foreach (var option in unionOption.WebClientProduserOptions)
            {
                var prod = _webClientFactory(option);
                _produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }
        }

        /// <summary>
        /// Верннуть все опции продюссеров
        /// </summary>
        /// <returns></returns>
        public ProduserUnionOption GetProduserUnionOptionAgregator()
        {
            var agregator = new ProduserUnionOption();
            var produsers = _produsersUnion.GetProduserDict.Values;
            foreach (var prod in produsers)
            {
                switch (prod.Option)
                {
                    case SignalRProduserOption signalROption:
                        agregator.SignalRProduserOptions.Add(signalROption);
                        break;

                    case KafkaProduserOption kafkaOption:
                        agregator.KafkaProduserOptions.Add(kafkaOption);
                        break;

                    case WebClientProduserOption webClientOption:
                        agregator.WebClientProduserOptions.Add(webClientOption);
                        break;

                        //case ProduserType.SerilogLoger:
                        //    break;
                }
            }
            return agregator;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteProduserUnionByOptionAgregator(string key)
        {
            var res = _produsersUnion.RemoveProduser(key);
            return res;
        }

        #endregion
    }
}