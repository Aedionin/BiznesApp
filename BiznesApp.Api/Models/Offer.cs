namespace BiznesApp.Api.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Location { get; set; }
        
        // Kluczowe: Pole do powiązania oferty z użytkownikiem
        public string UserId { get; set; } = string.Empty;
    }
} 