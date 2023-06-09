using Autofac.Extensions.DependencyInjection;
using Autofac;
using Serilog;
using SimApi.Base;
using SimApi.Data.Uow;
using SimApi.Service.Middleware;
using SimApi.Service.RestExtension;
using SimApi.Data.Context;

namespace SimApi.Service;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public static JwtConfig JwtConfig { get; private set; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));


        services.AddOptions();

        services.AddCustomSwaggerExtension();
        //services.AddDbContextExtension(Configuration);
        //services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddMapperExtension();
        //services.AddRepositoryExtension();
        //services.AddServiceExtension();
        services.AddJwtExtension();

    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new AutofacModule());
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

        }
        IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
        SimEfDbContext dbContext = serviceScope.ServiceProvider.GetService<SimEfDbContext>();
        dbContext.Database.EnsureCreated();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DefaultModelsExpandDepth(-1);
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sim Company");
            c.DocumentTitle = "SimApi Company";
        });

        //DI
        app.AddExceptionHandler();
        app.AddDIExtension();

        app.UseMiddleware<HeartBeatMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
        {
            Log.Information("-------------Request-Begin------------");
            Log.Information(requestProfilerModel.Request);
            Log.Information(Environment.NewLine);
            Log.Information(requestProfilerModel.Response);
            Log.Information("-------------Request-End------------");
        };
        app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

        app.UseHttpsRedirection();

        // add auth 
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
