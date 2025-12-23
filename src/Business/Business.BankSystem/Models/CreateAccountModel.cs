namespace Business.BankSystem.Models;

public class CreateAccountModel
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public decimal InitialBalance { get; set; } = 0;
}