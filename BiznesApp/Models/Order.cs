using SQLite;

namespace BiznesApp.Models;

public class Order
{
    [PrimaryKey]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? PhotoPath { get; set; }
    public string? Location { get; set; }
} 