namespace Backend.ViewModels.BlogAdmin
{
    public class CreateBlogVM
    {
        public string TitleName { get; set; }

        public string Description { get; set; }
        public IFormFile Photo { get; set; }
    }
}
