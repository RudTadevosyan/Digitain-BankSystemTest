using Business.BankSystem.Models;
using Data.BankSystem.SQL.Services;

namespace Business.BankSystem.Services.Implementation;

public class CustomerService : ICustomerService
{
    protected readonly IOperationRepository OperationRepository;
    protected readonly IAccountRepository AccountRepository;

    public CustomerService(IOperationRepository operationRepository, IAccountRepository accountRepository)
    {
        OperationRepository = operationRepository;
        AccountRepository = accountRepository;
    }

    public virtual async Task<BaseResponse<bool>> DepositAsync(int accountId, decimal amount)
    {
        try
        {
            if (amount < 0)
                return new BaseResponse<bool>
                {
                    Data = false,
                    Message = "Deposit amount can't be negative."
                };

            var account = await AccountRepository.GetByIdAsync(accountId);
            if (account == null)
                return new BaseResponse<bool>
                {
                    Data = false,
                    Message = $"Account with {accountId} not found."
                };
            
            await OperationRepository.DepositAsync(accountId, amount);

            return new BaseResponse<bool>
            {
                Data = true,
                Message = "Success"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>
            {
                Data = false,
                Message = ex.Message,
            };
        }
    }

    public virtual async Task<BaseResponse<bool>> WithdrawAsync(int accountId, decimal amount)
    {
        try
        {
            if (amount < 0)
                return new BaseResponse<bool>
                {
                    Data = false,
                    Message = "Withdraw amount can't be negative."
                };
            
            var account = await AccountRepository.GetByIdAsync(accountId);
            if (account == null)
                return new BaseResponse<bool>
                {
                    Data = false,
                    Message = $"Account with {accountId} not found."
                };

            if (account.Balance < amount)
                return new BaseResponse<bool>
                {
                    Data = false,
                    Message = "Insufficient balance."
                };
            
            await OperationRepository.WithdrawAsync(accountId, amount);

            return new BaseResponse<bool>
            {
                Data = true,
                Message = "Success"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>
            {
                Data = false,
                Message = ex.Message,
            };
        }
    }

    public virtual async Task<BaseResponse<AccountModel>> CheckMyAccount(int accountId)
    {
        try
        {
            var account = await AccountRepository.GetByIdAsync(accountId);
            if (account == null)
                return new BaseResponse<AccountModel>
                {
                    HttpStatusCode = 404,
                    Message = $"Account with {accountId} not found."
                };

            AccountModel acc = new AccountModel
            {
                AccountId = accountId,
                FullName = account.FullName,
                Balance = account.Balance,
            };

            return new BaseResponse<AccountModel>
            {
                Data = acc,
                HttpStatusCode = 200,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<AccountModel>
            {
                HttpStatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}