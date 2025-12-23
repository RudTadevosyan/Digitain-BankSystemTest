namespace Data.BankSystem.SQL.Models
{
    public class Account
    {
        public int AccountId {  get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Decimal Balance { get; set; }
    }
}
