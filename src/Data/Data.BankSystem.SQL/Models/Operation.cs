namespace Data.BankSystem.SQL.Models
{
    public class Operation
    {
        public int OperationId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string OperationType { get; set; } = null!;
        public DateTime CreatedAt {  get; set; }
    }
}
