// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

// app.Run();

using WorkerAndApiPoc;

IHostBuilder builder = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>{
        webBuilder.UseStartup<Startup>();
    });

IHost host = builder.Build();
await host.RunAsync();