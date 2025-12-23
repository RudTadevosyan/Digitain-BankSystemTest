namespace Business.BankSystem.Models;

public class AccountModel
{
    public int AccountId {  get; set; }
    public string FullName { get; set; } = null!;
    public Decimal Balance { get; set; }
}