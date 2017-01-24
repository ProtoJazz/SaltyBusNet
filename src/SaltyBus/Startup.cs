using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SaltyBus.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Workers;

namespace SaltyBus
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            
            var connection = Environment.GetEnvironmentVariable("saltyConnection");
            services.AddDbContext<SaltyContext>(options => options.UseSqlServer(connection));
            services.AddHangfire(x => x.UseSqlServerStorage(connection));
            services.AddScoped<ISaltyRepository, SaltyRepository>();
            services.AddTransient<BusChecker>();
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, BusChecker checker)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }
           
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate(
                () => checker.GetBuses(), Cron.MinuteInterval(1));
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
