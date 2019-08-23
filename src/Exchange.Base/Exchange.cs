using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProdusersMediator;
using Shared;

namespace Exchange.Base
{
    public class Exchange
    {
        private readonly ProdusersUnion<AdInputTypeFake> _produsersUnion;

        public Exchange(ProdusersUnion<AdInputTypeFake> produsersUnion)
        {
            _produsersUnion = produsersUnion;
        }



        public async Task Execute()
        {

            try
            {
                var resp = new ResponsePieceOfDataWrapper<AdInputTypeFake>()
                {
                    DataAction = DataAction.CycleAction,
                    DeviceName = "Device_1",
                    ExceptionExchangePipline = null,
                    IsValidAll = true,
                    KeyExchange = "Exchange111",
                    TimeAction = 2000,
                    MessageDict = new Dictionary<string, string>()
                    {
                        {"key1", "value1"},
                        {"key2", "value2"},
                        {"key3", "value3"}
                    },
                    ResponsesItems = new List<ResponseDataItem<AdInputTypeFake>>()
                    {
                        new ResponseDataItem<AdInputTypeFake>
                        {
                            RequestId = "10",
                            ResponseInfo = new ResponseInfo
                            {
                                Encoding = "Utf-8",
                                IsOutDataValid = true,
                                ResponseData = "56345653455654353453453",
                                StronglyTypedResponse = null
                            }, 
                            Status = StatusDataExchange.End,
                            TransportException = null,
                            MessageDict = null
                        }
                    }
                };

                var res = await _produsersUnion.SendAll(resp); // TODO: Задавать invokerName= deviceName 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}