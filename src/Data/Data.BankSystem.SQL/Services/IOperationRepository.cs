using Data.BankSystem.SQL.Models;

namespace Data.BankSystem.SQL.Services;

public interface IOperationRepository
{
    Task<IEnumerable<Operation>> GetByAccountIdAsync(int accountId, int pageNumber = 1, int pageSize = 10);
    Task DepositAsync(int accountId, decimal amount);
    Task WithdrawAsync(int accountId, decimal amount);
}