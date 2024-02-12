namespace TheFinalProject.ViewModels.BlogAdmin
{
    public class CreateServiceVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Img { get; set; }
    }
}
