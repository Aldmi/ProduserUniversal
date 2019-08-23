using AbstractProduser.AbstractProduser;
using AbstractProduser.Enums;
using AbstractProduser.Options;
using Autofac;
using Exchange.Base;
using KafkaProduser;
using ProdusersMediator;
using WebApi.Produsers;
using WebApi.SignalRClients;
using WebClientProduser;

namespace WebApi.AutofacModules
{
    public class ProdusersUnionAutofacModule : Module
    {    
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProdusersUnionFactory<AdInputTypeFake>>().InstancePerDependency();//для CWS
            builder.RegisterType<ProdusersFactory<AdInputTypeFake>>().InstancePerDependency();

            builder.RegisterType<ProdusersUnion<AdInputTypeFake>>().SingleInstance();

            builder.RegisterType<SignaRProduserClientsStorage>().SingleInstance();
            builder.RegisterType<SignalRProduserWrapper>().As<IProduser<SignalRProduserOption>>().InstancePerDependency();

            builder.RegisterType<KafkaProduserWrapper>().As<IProduser<KafkaProduserOption>>().InstancePerDependency();

            builder.RegisterType<WebClientProduserWrapper>().As<IProduser<WebClientProduserOption>>().InstancePerDependency();

           // builder.RegisterType<HttpClientSupport>().As<IHttpClientSupport>().InstancePerDependency();//DEBUG
        }       
    }
}