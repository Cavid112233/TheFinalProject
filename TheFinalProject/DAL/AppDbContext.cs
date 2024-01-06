using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheFinalProject.Entities;

namespace TheFinalProject.DAL
{
    public class AppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<RepairService> RepairServices { get; set; }
    }
}
