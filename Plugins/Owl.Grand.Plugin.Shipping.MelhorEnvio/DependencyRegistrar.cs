using Autofac;
using Grand.Core.Configuration;
using Grand.Core.Infrastructure;
using Grand.Core.Infrastructure.DependencyManagement;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Controllers;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Services;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Widgets;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, GrandConfig config)
        {
            builder.RegisterType<ShippingMelhorEnvioService>().InstancePerLifetimeScope();
            //base shipping controller
            builder.RegisterType<ShippingMelhorEnvioController>();

            // Plugins
            builder.RegisterType<MelhorEnvioShippingComputationMethod>().InstancePerLifetimeScope();
            builder.RegisterType<MelhorEnvioProductEstimateWidget>().InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 10; }
        }
    }
}
