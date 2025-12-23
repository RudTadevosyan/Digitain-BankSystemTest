using System.Data;
using Data.BankSystem.SQL.Models;
using Microsoft.Data.SqlClient;

namespace Data.BankSystem.SQL.Services.Implementation;

public class AccountRepository : IAccountRepository
{
    protected readonly IDbConnection Connection;

    public AccountRepository(IDbConnection connection)
    {
        Connection = connection;
    }
    
    protected virtual async Task OpenConnAsync()
    {
        if (Connection.State != ConnectionState.Open)
            await ((SqlConnection)Connection).OpenAsync();
    }

    public virtual async Task<Account?> GetByEmailAsync(string email)
    {
        await OpenConnAsync();
        
        using var cmd = new SqlCommand("core.Account_GetByEmail", (SqlConnection)Connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = email });
        
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Account
            {
                AccountId = reader.GetInt32(reader.GetOrdinal("AccountId")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Balance = reader.GetDecimal(reader.GetOrdinal("Balance")),
            };
        }

        return null;
    }

    public virtual async Task<Account?> GetByIdAsync(int accountId)
    {
        await OpenConnAsync();
        
        using var cmd = new SqlCommand("core.Account_GetById", (SqlConnection)Connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        cmd.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int ) { Value = accountId });
        
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Account
            {
                AccountId = accountId,
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Balance = reader.GetDecimal(reader.GetOrdinal("Balance")),
            };
        }

        return null;
    }

    public virtual async Task<int> CreateAsync(Account account)
    {
        await OpenConnAsync();
        
        using var cmd = new SqlCommand("core.Account_Create", (SqlConnection)Connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        var output = new SqlParameter("@AccountId", SqlDbType.Int ) { Direction = ParameterDirection.Output };
        
        cmd.Parameters.Add(output);
        cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar, 200) { Value = account.FullName });
        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = account.Email });
        cmd.Parameters.Add(new SqlParameter("@Balance", SqlDbType.Decimal)
        {
            Precision = 10,
            Scale = 2,
            Value = account.Balance
        });
        
        await cmd.ExecuteNonQueryAsync();
        return Convert.ToInt32(output.Value);
    }
}