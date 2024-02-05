using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
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
        public DbSet<Settings> Setting { get; set; }
        public DbSet<GetInTouch> GetInTouches { get; internal set;}
        public DbSet<FastestRepairService> FastestRepairServices { get; internal set; }
        public DbSet<FastestRepairServiceMain> FastestRepairServiceMains { get; internal set; }
        public DbSet<ServiceAbout> ServiceAbouts { get; internal set; }
        public DbSet<ExperiencedStaff> ExperiencedStaffs { get; internal set; }
        public DbSet<BlogPost> BlogPosts { get; internal set; }
        public DbSet<ServicePageMain> ServicePageMains { get; internal set;}
    }
}
