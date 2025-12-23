using System.Data;
using Data.BankSystem.SQL.Models;
using Microsoft.Data.SqlClient;

namespace Data.BankSystem.SQL.Services.Implementation;

public class OperationRepository : IOperationRepository
{
    protected readonly IDbConnection Connection;

    public OperationRepository(IDbConnection connection)
    {
        Connection = connection;
    }

    protected virtual async Task OpenConnAsync()
    {
        if (Connection.State != ConnectionState.Open)
            await ((SqlConnection)Connection).OpenAsync();
    }
    public virtual async Task<IEnumerable<Operation>> GetByAccountIdAsync(int accountId, int pageNumber = 1, int pageSize = 10)
    {
        await OpenConnAsync();
        
        using var cmd = new SqlCommand("core.Operation_GetByAccountId", (SqlConnection)Connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        cmd.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int) { Value = accountId });
        cmd.Parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber });
        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize });
        
        List<Operation> operations = new List<Operation>();
        using var reader  = await cmd.ExecuteReaderAsync();
        
        int ordOperationId = reader.GetOrdinal("OperationId");
        int ordAmount = reader.GetOrdinal("Amount");
        int ordOperationType = reader.GetOrdinal("OperationType");
        int ordCreatedAt = reader.GetOrdinal("CreatedAt");
        
        while (await reader.ReadAsync())
        {
            operations.Add(new Operation
            {
                OperationId = reader.GetInt32(ordOperationId),
                Amount = reader.GetDecimal(ordAmount),
                OperationType = reader.GetString(ordOperationType),
                CreatedAt = reader.GetDateTime(ordCreatedAt),
                AccountId = accountId,
            });
        }
        
        return operations;
    }

    public virtual async Task DepositAsync(int accountId, decimal amount)
    {
        await OpenConnAsync();
        
        using var cmd = new SqlCommand("core.Operation_Deposit", (SqlConnection)Connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        cmd.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int) { Value = accountId });
        cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Decimal)
        {
            Precision = 10,
            Scale = 2,
            Value = amount
        });
        
        await cmd.ExecuteNonQueryAsync();
    }

    public virtual async Task WithdrawAsync(int accountId, decimal amount)
    {
        await OpenConnAsync();
        
        using var cmd = new SqlCommand("core.Operation_Withdraw", (SqlConnection)Connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        cmd.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.Int) { Value = accountId });
        cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Decimal)
        {
            Precision = 10,
            Scale = 2,
            Value = amount
        });
        
        await cmd.ExecuteNonQueryAsync();
    }
}