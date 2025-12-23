using System.Data;
using Data.BankSystem.SQL.Services;
using Data.BankSystem.SQL.Services.Implementation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.BankSystem.SQL.Extensions;

public static class DataLayerRegister
{
    public static IServiceCollection AddDataLayerServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IDbConnection>(sp =>
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        });
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        
        return services;
    }
}