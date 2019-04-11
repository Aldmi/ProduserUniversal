using System;
using System.Threading.Tasks;
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
                var message = "HelloWorold";
                var res = await _produsersUnion.SendAll(message); // TODO: Задавать invokerName= deviceName 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}