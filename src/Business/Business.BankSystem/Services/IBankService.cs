using Business.BankSystem.Models;

namespace Business.BankSystem.Services;

public interface IBankService
{
    Task<BaseResponse<AccountModel>> CreateAccountAsync(CreateAccountModel accountModel);
    Task<BaseResponse<IEnumerable<OperationHistory>>> GetOperationHistoryAsync(int accountId, int pageNumber = 1, int pageSize = 10);
}