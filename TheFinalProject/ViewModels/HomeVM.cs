using TheFinalProject.Entities;

namespace TheFinalProject.ViewModels
{
    public class HomeVM
    {
        public List<ServiceRepair> ServiceRepairs { get; set; }
        public List<Services2> Services2s { get; set; }
        public List<ServiceProcess> ServiceProcesss { get; set; }
        public List<Products> Productss { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<LatestBlog> LatestBlogs { get; set; }

    }
}
