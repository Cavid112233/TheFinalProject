using System;
using System.ComponentModel.DataAnnotations;

namespace TheFinalProject.Models
{
    public class Material:BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}

