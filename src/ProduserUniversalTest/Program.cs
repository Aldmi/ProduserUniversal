using System;
using System.Threading.Tasks;
using ProduserUniversal.AbstractProduser;
using ProduserUniversal.ConcreteProdusers;
using ProduserUniversal.Services;

namespace ProduserUniversalTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IProduser kaffkaProd1 = new KaffkaProduser(TimeSpan.FromSeconds(3), 10);
            IProduser kaffkaProd2 = new KaffkaProduser(TimeSpan.FromSeconds(0.5), 10);
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
