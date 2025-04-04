// Copyright © 2025 Always Active Technologies PTY Ltd

using Serilog;
using TechAptV1.Client.Components;

using Microsoft.EntityFrameworkCore;
using TechAptV1.Client.Services;
using TechAptV1.Client.Data;

namespace TechAptV1.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.Title = "Tech Apt V1";

                var builder = WebApplication.CreateBuilder(args);

                // Add configuration binding
                builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                builder.Services.AddSerilog(lc => lc
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                    .ReadFrom.Configuration(builder.Configuration));

                // Add services to the container.
                builder.Services.AddRazorComponents().AddInteractiveServerComponents();

                builder.Services.AddDbContext<DataContext>(options =>
                        options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

                builder.Services.AddScoped<IDataService, DataService>();

                builder.Services.AddScoped<IThreadingService, ThreadingService>();
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                }

                app.UseStaticFiles();
                app.UseAntiforgery();

                app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

                app.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Fatal exception in Program");
                Console.WriteLine(exception);
            }
        }
    }
}
