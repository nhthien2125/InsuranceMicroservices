using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PricingService.Commands;
using PricingService.Domain;
using Steeltoe.Discovery.Client;

namespace PricingService;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDiscoveryClient(Configuration);
        services.AddControllers().AddNewtonsoftJson();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
        services.AddSingleton<IDataStore, InMemoryDataStore>();
        services.AddScoped<IRequestHandler<Api.Commands.CalculatePriceCommand, Api.Commands.CalculatePriceResult>, CalculatePriceHandler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

