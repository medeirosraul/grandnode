using Autofac;
using Grand.Core.Configuration;
using Grand.Core.Infrastructure;
using Grand.Core.Infrastructure.DependencyManagement;
using Grand.Services.Catalog;
using Owl.Grand.Plugin.Misc.Essentials.Controllers;
using Owl.Grand.Plugin.Misc.Essentials.Services;

namespace Owl.Grand.Plugin.Misc.Essentials
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, GrandConfig config)
        {
            builder.RegisterType<OwlEssentials>().InstancePerLifetimeScope();
            builder.RegisterType<OwlEssentialsController>();

            // Admin services enhancements
            builder.RegisterType<OwlProductService>().As<IProductService>().InstancePerLifetimeScope();

            // Admin addons
            builder.RegisterType<MercadoLivreService>().InstancePerLifetimeScope();

            // Analytics
            builder.RegisterType<AnalyticsService>().SingleInstance();

        }

        public int Order
        {
            get { return 10; }
        }
    }
}
