using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Portfolio.Api.Middlewares;
using Portfolio.Infrastructure.Dependencies;
using Portfolio.Persistence.Dependencies;
using Portfolio.Application.Dependencies;
using System;
using System.Threading.Tasks;

namespace Portfolio.Api;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Serilog Configuration
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder().Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Portfolio API Host");

            var builder = WebApplication.CreateBuilder(args);

            // Add Serilog
            builder.Host.UseSerilog((context, services, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

            // Add Services from Layers
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);

            // Custom Middlewares & Config
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHealthChecks();
            
            // CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseSerilogRequestLogging();
            app.UseCors("AllowAll");
            
            // Security Middlewares
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RateLimitMiddleware>();
            app.UseMiddleware<AuditLogMiddleware>();

            app.MapControllers();
            app.MapHealthChecks("/health");

            // Database Seeding & Migration
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PortfolioDbContext>();
                    await context.Database.MigrateAsync();
                    await DbSeeder.SeedAsync(context, services);
                    Log.Information("Database migration and seeding completed successfully.");
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "An error occurred while migrating or seeding the database.");
                }
            }

            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
