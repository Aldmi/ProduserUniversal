using AbstractProduser.AbstractProduser;
using AbstractProduser.Enums;
using AbstractProduser.Options;
using Autofac;
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
            builder.RegisterType<ProdusersUnionFactory>().InstancePerDependency();//для CWS

            builder.RegisterType<ProdusersFactory>().InstancePerDependency();
            builder.RegisterType<ProdusersUnion>().SingleInstance();

            builder.RegisterType<SignaRProduserClientsStorage>().SingleInstance();
            builder.RegisterType<SignalRProduserWrapper>().As<IProduser<SignalRProduserOption>>().InstancePerDependency();

            builder.RegisterType<KafkaProduserWrapper>().As<IProduser<KafkaProduserOption>>().InstancePerDependency();

            builder.RegisterType<WebClientProduserWrapper>().As<IProduser<WebClientProduserOption>>().InstancePerDependency();

           // builder.RegisterType<HttpClientSupport>().As<IHttpClientSupport>().InstancePerDependency();//DEBUG
        }       
    }
}