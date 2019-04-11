using Autofac;

namespace WebApi.AutofacModules
{
    public class ExchangeAutofacModule : Module
    {    
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Exchange.Base.Exchange>().InstancePerDependency();
        }       
    }
}