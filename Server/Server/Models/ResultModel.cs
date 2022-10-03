namespace Server.Models;

public class ResultModel
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public string TestId { get; set; } = null!;
    
    public double Result { get; set; }
    
    public Guid IdempotencyKey { get; set; }
}