using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheFinalProject.Entities;

namespace TheFinalProject.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ServiceRepair> ServiceRepairs { get; set; }
        public DbSet<Services2> Services2s { get; internal set; }
        public DbSet<ServiceProcess> ServiceProcesss { get; internal set; }
        public DbSet<Products> Productss { get; internal set; }
        public DbSet<Feedback> Feedbacks { get; internal set; }
        public DbSet<LatestBlog> LatestBlogs { get; internal set; }

    }
}
