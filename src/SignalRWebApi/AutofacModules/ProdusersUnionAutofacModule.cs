using Autofac;
using ProdusersUnion;
using WebApi.Produsers;

namespace WebApi.AutofacModules
{
    public class ProdusersUnionAutofacModule : Module
    {    
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProdusersUnionService>().SingleInstance();
            builder.RegisterType<SignalRProduser>().SingleInstance();

        }       
    }
}