using System;
using System.ComponentModel.DataAnnotations;
using TheFinalProject.Models;

namespace TheFinalProject.ViewModels.AccountViewModels
{
    public class ProfileVM
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Surname { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? Username { get; set; }
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }

        public IEnumerable<Address>? Addresses { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
    }
}

