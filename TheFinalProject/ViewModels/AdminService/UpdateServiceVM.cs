namespace TheFinalProject.ViewModels.BlogAdmin
{
    public class UpdateServiceVM
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
