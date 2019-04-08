using System;
using System.Threading.Tasks;
using ProdusersUnion;

namespace Exchange.Base
{
    public class Exchange
    {
        private readonly ProdusersUnionService _produsersUnionService;

        public Exchange(ProdusersUnionService produsersUnionService)
        {
            _produsersUnionService = produsersUnionService;
        }



        public async Task Execute()
        {

            try
            {
                var message = "HelloWorold";
                var res = await _produsersUnionService.SendAll(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}