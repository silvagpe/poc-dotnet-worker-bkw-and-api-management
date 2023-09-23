using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using WorkerAndApiPoc.Worker;


namespace WorkerAndApiPoc;


public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        this.configuration = configuration;
        var builder = new ConfigurationBuilder()
            .SetBasePath(hostEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddHttpContextAccessor();
        services.AddHealthChecks();
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Worker and api management", Version = "v1" });
        });

        services.AddSingleton<WorkerManager>();
        services.AddHostedService(sp => sp.GetRequiredService<WorkerManager>());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseEndpoints(ep =>
        {
            ep.MapControllers();
            ep.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
    }
}