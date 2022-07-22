using Autofac;
using DealershipManagement.Common.Utilities.Interfaces;
using DealershipManagement.DataAccess;
using DealershipManagement.DataAccess.Helpers;
using DealershipManagement.Entities.Account;
using DealershipManagement.Services.Authentication;
using DealershipManagement.Services.Authorization;
using DealershipManagement.Services.Customer;
using DealershipManagement.Services.IdentityManager;
using DealershipManagement.Services.Interfaces;
using DealershipManagement.Services.Mapper;
using DealershipManagement.Services.Notification;
using DealershipManagement.Services.Vehicle;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Template.Repository;
using Template.Repository.Interfaces;

namespace DealershipManagement.Services
{
    public class ServiceDependencyModule : Module
    {
        IConfiguration configuration;

        public ServiceDependencyModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var logger = LoggerFactory.Create(logBuilder =>
            {
                //logBuilder.AddConsole();
            });

            builder.Register(opts =>
            {
                var config = opts.Resolve<IConfiguration>();
                var commandTimeout = config.GetValue<int?>("CommandTimeout") ?? 30;
                var builder = new DbContextOptionsBuilder<DealershipManagementDbContext>()
                                    .UseSqlServer(configuration.GetConnectionString("DealershipManagementDb"),
                                    sqlServerOptions => sqlServerOptions.CommandTimeout(commandTimeout));

                if (config.GetValue<int>("ENABLE_QUERY_LOGGING") == 1)
                    builder = builder.EnableSensitiveDataLogging()
                    .UseLoggerFactory(logger);

                return builder.Options;
            }).AsSelf().InstancePerLifetimeScope();

            RegisterContexts(builder);
            RegisterRepositories(builder);
            RegisterServices(builder);
        }

        //Registering Contexts
        private static void RegisterContexts(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>()
              .As<IHttpContextAccessor>()
              .SingleInstance();
            builder.RegisterType<DealershipManagementDbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<WebWorkContext>().As<IWorkContext<UserAccount, string>>().InstancePerLifetimeScope();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            //Registering Repositories
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,,>)).WithParameter("enableSoftDelete", true).AsSelf().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(AuditableRepository<>)).WithParameter("enableSoftDelete", true).AsSelf().InstancePerLifetimeScope();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClient>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserManagerAccessor>().As<IUserManagerAccessor>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<NotificationSenderService>().As<INotificationSenderService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<NotificationService>().As<INotificationService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TemplateMapper>().As<ITemplateMapper>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<VehicleService>().As<IVehicleService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().AsSelf().InstancePerLifetimeScope();

            builder.Register<IAuthorizationService>(
                (c, p) =>
                {
                    return c.Resolve<SessionAuthorizationService>();
                });
        }
    }
}
