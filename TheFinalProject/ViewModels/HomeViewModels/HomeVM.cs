using System;
using TheFinalProject.Models;

namespace TheFinalProject.ViewModels.HomeViewModels
{
    public class HomeVM
    {
        public IDictionary<string, string> Settings { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}

