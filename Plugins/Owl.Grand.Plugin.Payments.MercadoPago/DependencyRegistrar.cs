﻿using Autofac;
using Grand.Core.Configuration;
using Grand.Core.Infrastructure;
using Grand.Core.Infrastructure.DependencyManagement;
using Owl.Grand.Plugin.Payments.MercadoPago.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.MercadoPago
{
    class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, GrandConfig config)
        {
            builder.RegisterType<MercadoPagoPaymentProcessor>().InstancePerLifetimeScope();
            builder.RegisterType<MercadoPagoPaymentService>().InstancePerLifetimeScope();
        }
    }
}
