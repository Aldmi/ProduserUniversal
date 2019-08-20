using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProdusersMediator;

namespace Exchange.Base
{
    public class Exchange
    {
        private readonly ProdusersUnion _produsersUnion;

        public Exchange(ProdusersUnion produsersUnion)
        {
            _produsersUnion = produsersUnion;
        }



        public async Task Execute()
        {

            try
            {
                var resp= new RespWrapper
                {
                    Id=10,
                    StatusCode = "Привет-пока"
                };

                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.None,                 //Отступы дочерних элементов 
                    NullValueHandling = NullValueHandling.Ignore  //Игнорировать пустые теги
                };
                var jsonResp = JsonConvert.SerializeObject(resp, settings);
                var res = await _produsersUnion.SendAll(jsonResp); // TODO: Задавать invokerName= deviceName 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}