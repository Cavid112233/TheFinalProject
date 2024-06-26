using System;
using TheFinalProject.Models;

namespace TheFinalProject.ViewModels.ShopViewModels
{
    public class ShopVM
    {
        public IEnumerable<Product> Products { get; set; }
        
        public IEnumerable<Category> Categories { get; set; }
        
        public IEnumerable<Material> Materials { get; set; }



    }
}

