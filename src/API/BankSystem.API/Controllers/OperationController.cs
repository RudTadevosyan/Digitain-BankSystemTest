using Business.BankSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationController : ControllerBase
{
    protected readonly ICustomerService CustomerService;

    public OperationController(ICustomerService customerService)
    {
        CustomerService = customerService;
    }

    [HttpPost("accounts/{accountId}/deposit")]
    public virtual async Task<IActionResult> Deposit([FromRoute] int accountId, decimal amount)
    {
        var response = await CustomerService.DepositAsync(accountId, amount);
        return StatusCode(response.HttpStatusCode, response);
    }
    
    [HttpPost("accounts/{accountId}/withdraw")]
    public virtual async Task<IActionResult> Withdraw([FromRoute] int accountId, decimal amount)
    {
        var response = await CustomerService.WithdrawAsync(accountId, amount);
        return StatusCode(response.HttpStatusCode, response);
    }

    [HttpGet("accounts/{accountId}")]
    public virtual async Task<IActionResult> GetAccount([FromRoute] int accountId)
    {
        var response = await CustomerService.CheckMyAccount(accountId);
        return StatusCode(response.HttpStatusCode, response);
    }
}