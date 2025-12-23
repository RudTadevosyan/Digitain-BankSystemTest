using Business.BankSystem.Services;
using Business.BankSystem.Services.Implementation;
using Data.BankSystem.SQL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.BankSystem.Extensions;

public static class BusinessLayerRegister
{
    public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IBankService, BankService>();
       
        services.AddDataLayerServices(config);
       
        return services;
    }

}