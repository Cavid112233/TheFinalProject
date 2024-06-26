using System;
namespace TheFinalProject.ViewModels.BasketViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public double DiscountedPrice { get; set; }

        public double Price { get; set; }
        public int Count { get; set; }
    }
}

