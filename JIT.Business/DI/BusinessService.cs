using JIT.Business.Interfaces;
using JIT.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIT.Business.DI
{
    public static class BusinessService
    {
        public static IServiceCollection AddBusinessServicesCollection(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            
            services.TryAdd(ServiceDescriptor.Scoped<IJitService, JitService>());
            services.TryAdd(ServiceDescriptor.Scoped<IEmailService, EmailService>());

            return services;
        }
    }
}

