using Business.BankSystem.Models;

namespace Business.BankSystem.Services;

public interface ICustomerService
{
    Task<BaseResponse<bool>> DepositAsync(int accountId, decimal amount);
    Task<BaseResponse<bool>> WithdrawAsync(int accountId, decimal amount);
    
    Task<BaseResponse<AccountModel>> CheckMyAccount(int accountId);
}