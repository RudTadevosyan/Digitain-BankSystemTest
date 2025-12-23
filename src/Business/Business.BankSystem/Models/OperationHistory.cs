namespace Business.BankSystem.Models;

public class OperationHistory
{
    public int OperationId { get; set; }
    public decimal Amount { get; set; }
    public string OperationType { get; set; } = null!;
    public DateTime OperationTime { get; set; }
}