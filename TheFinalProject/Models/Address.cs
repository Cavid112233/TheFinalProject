using System;
using System.ComponentModel.DataAnnotations;

namespace TheFinalProject.Models
{
    public class Address:BaseEntity
    {
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
        public bool IsMain { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}

