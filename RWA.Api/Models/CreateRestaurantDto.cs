using System.ComponentModel.DataAnnotations;

namespace RWA.Api.Models
{
    public class CreateRestaurantDto
    {
        [Required]
        [MaxLength(25)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool HasDelivery { get; set; }
        [EmailAddress]
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string? City { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }
}
