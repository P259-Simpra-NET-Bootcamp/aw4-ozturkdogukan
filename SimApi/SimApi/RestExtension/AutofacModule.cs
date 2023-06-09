using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using SimApi.Data.Context;
using SimApi.Data.Repository;
using SimApi.Data.Uow;
using SimApi.Operation;
using SimApi.Operation.Dapper.Category;
using SimApi.Schema;
using SimApi.Service.CustomService;
using SimApi.Service.Middleware;
using SimApi.Service.RestExtension;

namespace SimApi.Service.RestExtension
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register DbContextExtension
            builder.Register(ctx =>
            {
                var configuration = ctx.Resolve<IConfiguration>();
                var services = new ServiceCollection();
                services.AddDbContextExtension(configuration);
                return services.BuildServiceProvider().GetRequiredService<SimEfDbContext>();
            }).As<SimEfDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<SimDapperDbContext>().InstancePerLifetimeScope();



            // Register UnitOfWork  
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // Register MapperExtension
            builder.Register(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MapperProfile());
                });
                return config.CreateMapper();
            }).InstancePerLifetimeScope();

            // Register RepositoryExtension
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

            // Register ServiceExtension
            builder.RegisterType<UserLogService>().As<IUserLogService>().InstancePerLifetimeScope();
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionService>().As<ITransactionService>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionReportService>().As<ITransactionReportService>().InstancePerLifetimeScope();
            builder.RegisterType<DapperAccountService>().As<IDapperAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();

            // Register middleware
            builder.RegisterType<HeartBeatMiddleware>().InstancePerLifetimeScope();
            builder.RegisterType<ErrorHandlerMiddleware>().InstancePerLifetimeScope();
            builder.RegisterType<RequestLoggingMiddleware>().InstancePerLifetimeScope();

            // Register JwtExtension
            builder.Register(ctx =>
            {
                var services = new ServiceCollection();
                services.AddJwtExtension();
                var serviceProvider = services.BuildServiceProvider();
                return serviceProvider.GetRequiredService<IAuthenticationService>();
            }).InstancePerLifetimeScope();

            // Register other dependencies
            builder.RegisterType<ScopedService>().InstancePerLifetimeScope();
            builder.RegisterType<TransientService>().InstancePerDependency();
            builder.RegisterType<SingletonService>().SingleInstance();
        }
    }
}
