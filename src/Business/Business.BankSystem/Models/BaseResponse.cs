namespace Business.BankSystem.Models;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    
    public int HttpStatusCode { get; set; }
    public string? Message { get; set; }
}