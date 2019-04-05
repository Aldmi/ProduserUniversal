using System;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using Autofac;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.SignalR;
using SignalRWebApiProduser.Hubs;


namespace SignalRWebApiProduser.ProduserWrappers
{
    public class SignalRProduserWrapper : IProduser
    {
        //[AutofacResolve]
        public IHubContext<BaseHub> BaseHubContext { get; set; }



        public SignalRProduserWrapper()
        {
            try
            {
              var gg=  Startup.ApplicationContainer.Resolve<IHubContext<BaseHub>>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
         
            }

        }




        Task<Result<string, ErrorWrapper>> IProduser.Send(string message)
        {
            try
            {
              //  Startup.ApplicationContainer.Resolve<IHubContext<BaseHub>>();

               // var gg = St;

                //await _baseHubContext.Clients.All.SendAsync(methode, "UserRX", message);
            }
            catch (Exception e)
            {

            }

            throw new NotImplementedException();
        }


        Task<Result<string, ErrorWrapper>> IProduser.Send(object message)
        {
            throw new NotImplementedException();
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}