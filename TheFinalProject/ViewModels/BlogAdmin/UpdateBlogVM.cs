﻿namespace TheFinalProject.ViewModels.BlogAdmin
{
    public class UpdateBlogVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Img { get; set; }
    }
}
