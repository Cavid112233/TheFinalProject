namespace Backend.ViewModels.BlogAdmin
{
    public class UpdateBlogVM
    {

        public int Id { get; set; }

        public string TitleName { get; set; }

        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
