﻿namespace TheFinalProject.ViewModels.BlogAdmin
{
    public class CreateBlogVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Img { get; set; }
    }
}
