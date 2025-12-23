using Business.BankSystem.Models;
using Data.BankSystem.SQL.Models;
using Data.BankSystem.SQL.Services;

namespace Business.BankSystem.Services.Implementation;

public class BankService : IBankService
{
    protected readonly IAccountRepository AccountRepository;
    protected readonly IOperationRepository OperationRepository;
    
    public BankService(IAccountRepository accountRepository, IOperationRepository operationRepository)
    {
        AccountRepository = accountRepository;
        OperationRepository = operationRepository;
    }

    public virtual async Task<BaseResponse<AccountModel>> CreateAccountAsync(CreateAccountModel accountModel)
    {
        try
        {
            if (string.IsNullOrEmpty(accountModel.FullName) || string.IsNullOrEmpty(accountModel.Email))
                return new BaseResponse<AccountModel>
                {
                    HttpStatusCode = 400,
                    Message = "FullName and Email are required"
                };

            if (accountModel.InitialBalance < 0)
                return new BaseResponse<AccountModel>
                {
                    HttpStatusCode = 400,
                    Message = "Initial balance is negative"
                };

            var account = await AccountRepository.GetByEmailAsync(accountModel.Email);
            if (account != null)
                return new BaseResponse<AccountModel>
                {
                    HttpStatusCode = 400,
                    Message = "Account with this email already exists"
                };

            int accountId = await AccountRepository.CreateAsync(new Account
            {
                FullName = accountModel.FullName,
                Email = accountModel.Email,
                Balance = accountModel.InitialBalance
            });

            AccountModel acc = new AccountModel
            {
                AccountId = accountId,
                FullName = accountModel.FullName,
                Balance = accountModel.InitialBalance
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
                Message = ex.Message
            };
        }
    }

    public virtual async Task<BaseResponse<IEnumerable<OperationHistory>>> GetOperationHistoryAsync(int accountId, int pageNumber = 1, int pageSize = 10)    
    {
        try
        {
            if (pageNumber < 1)
                pageNumber = 1;
            
            if (pageSize < 1)
                pageSize = 10;
            
            if(pageSize > 100)
                pageSize = 100;
            
            var account = await AccountRepository.GetByIdAsync(accountId);
            if (account == null)
                return new BaseResponse<IEnumerable<OperationHistory>>
                {
                    HttpStatusCode = 404,
                    Message = $"Account with {accountId} not found"
                };
            
            var operations = await OperationRepository.GetByAccountIdAsync(accountId, pageNumber, pageSize);
            List<OperationHistory> history = new List<OperationHistory>();

            foreach (var o in operations)
            {
                history.Add(new OperationHistory
                {
                    OperationId = o.OperationId,
                    Amount = o.Amount,
                    OperationType = o.OperationType,
                    OperationTime = o.CreatedAt
                });
            }

            return new BaseResponse<IEnumerable<OperationHistory>>
            {
                Data = history,
                HttpStatusCode = 200,
            };

        }
        catch (Exception ex)
        {
            return new BaseResponse<IEnumerable<OperationHistory>>
            {
                HttpStatusCode = 500,
                Message = ex.Message
            };
        }
    }
}