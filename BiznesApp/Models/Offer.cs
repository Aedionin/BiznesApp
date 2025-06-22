using SQLite;

namespace BiznesApp.Models
{
    public class Offer
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
    }
} 