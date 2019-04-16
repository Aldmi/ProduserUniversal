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
    public class ProdusersFactory
    {
        private readonly ProdusersUnion _produsersUnion;
        private readonly Func<SignalRProduserOption, Owned<IProduser<SignalRProduserOption>>> _signalRFactory;
        private readonly Func<KafkaProduserOption, Owned<IProduser<KafkaProduserOption>>> _kafkaFactory;


        #region ctor

        public ProdusersFactory(ProdusersUnion produsersUnion,
                                Func<SignalRProduserOption, Owned<IProduser<SignalRProduserOption>>> signalRFactory,
                                Func<KafkaProduserOption, Owned<IProduser<KafkaProduserOption>>> kafkaFactory)
        {
            _produsersUnion = produsersUnion;
            _signalRFactory = signalRFactory;
            _kafkaFactory = kafkaFactory;
        }

        #endregion



        #region Methode

        /// <summary>
        /// Добавляет созданные на базе опций продюссеры к ProdusersUnion
        /// </summary>
        /// <param name="optionAgregator"></param>
        public void FillProduserUnionByOptionAgregator(ProduserOptionAgregator optionAgregator)
        {
            foreach (var option in optionAgregator.KafkaProduserOptions)
            {
                var prod = _kafkaFactory(option);
                _produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }
            foreach (var option in optionAgregator.SignalRProduserOptions)
            {
                var prod = _signalRFactory(option);
                _produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }
        }

        /// <summary>
        /// Верннуть все опции продюссеров
        /// </summary>
        /// <returns></returns>
        public ProduserOptionAgregator GetProduserUnionOptionAgregator()
        {
            var agregator = new ProduserOptionAgregator();
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