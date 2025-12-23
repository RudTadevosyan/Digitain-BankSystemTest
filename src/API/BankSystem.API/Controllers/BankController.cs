using Business.BankSystem.Models;
using Business.BankSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankController : ControllerBase
{
    protected readonly IBankService BankService;

    public BankController(IBankService bankService)
    {
        BankService = bankService;
    }

    [HttpPost("create/account")]
    public virtual async Task<IActionResult> CreateAccount([FromBody] CreateAccountModel model)
    {
        var response = await BankService.CreateAccountAsync(model);
        return StatusCode(response.HttpStatusCode, response);
    }

    [HttpGet("accounts/{accountId}/operations")]
    public virtual async Task<IActionResult> GetAccountOperations([FromRoute] int accountId, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await BankService.GetOperationHistoryAsync(accountId, pageNumber, pageSize);
        return StatusCode(response.HttpStatusCode, response);
    }
}