using System;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using KafkaProduser;
using ProdusersUnion;
using SignalRWebApiProduser.ProduserWrappers;

namespace ProduserUniversalTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IProduser kaffkaProd1 = new KafkaProduserWrapper(TimeSpan.FromSeconds(3), 10, new KaffkaProduserOption {Key= "Kafka1", BrokerEndpoints = ""});
            IProduser kaffkaProd2 = new KafkaProduserWrapper(TimeSpan.FromSeconds(0.5), 10, new KaffkaProduserOption { Key = "Kafka2", BrokerEndpoints = "" });
            IProduser signalRProd1 = new SignalRProduserWrapper();


            using (var prodUnionServ = new ProdusersUnionService())
            {
                prodUnionServ.AddProduser("kaffka1", kaffkaProd1);
                prodUnionServ.AddProduser("kaffka2", kaffkaProd2);

               var message = "Hello World!";
               var res= await prodUnionServ.SendAll(message);
            }

            Console.ReadKey();
        }
    }
}
