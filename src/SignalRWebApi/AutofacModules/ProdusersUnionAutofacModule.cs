using AbstractProduser.AbstractProduser;
using AbstractProduser.Enums;
using Autofac;
using KafkaProduser;
using ProdusersMediator;
using WebApi.Produsers;

namespace WebApi.AutofacModules
{
    public class ProdusersUnionAutofacModule : Module
    {    
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProdusersFactory>().InstancePerDependency();
            builder.RegisterType<ProdusersUnion>().SingleInstance();             
            builder.RegisterType<SignalRProduserWrapper>().Keyed<IProduser>(ProduserType.SignalR).InstancePerDependency();
            builder.RegisterType<KafkaProduserWrapper>().Keyed<IProduser>(ProduserType.Kafaka).InstancePerDependency();
        }       
    }
}