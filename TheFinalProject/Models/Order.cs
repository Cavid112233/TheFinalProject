using System;
using System.ComponentModel.DataAnnotations;
using TheFinalProject.Enums;

namespace TheFinalProject.Models
{
    public class Order:BaseEntity
    {
        public int No { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public IEnumerable<OrderItem>? OrderItems { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(100)]
        public string? Surname { get; set; }
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(16)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }
        [StringLength(100)]
        public string? State { get; set; }
        [StringLength(100)]
        public string? City { get; set; }
        [StringLength(100)]
        public string? PostalCode { get; set; }
        [StringLength(100)]
        public string? AddressLine { get; set; }

        public OrderType Status { get; set; }
        public string? Comment { get; set; }
    }
}

