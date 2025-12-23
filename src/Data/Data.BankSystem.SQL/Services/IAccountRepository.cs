using Data.BankSystem.SQL.Models;

namespace Data.BankSystem.SQL.Services;

public interface IAccountRepository
{
    Task<Account?> GetByEmailAsync(string email);
    Task<Account?> GetByIdAsync(int accountId);
    Task<int> CreateAsync(Account account);
}