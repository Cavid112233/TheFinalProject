using System;
using TheFinalProject.Models;
using TheFinalProject.ViewModels.BasketViewModels;

namespace TheFinalProject.ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<BasketVM> BasketVMs { get; set; }

    }
}

