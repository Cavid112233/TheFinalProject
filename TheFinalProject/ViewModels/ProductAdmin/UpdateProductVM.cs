namespace TheFinalProject.ViewModels.ProductAdmin
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public string Img { get; set; }
    }
}
