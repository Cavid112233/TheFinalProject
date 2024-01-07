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
        public DbSet<RepairService> RepairService { get; set; }
        public DbSet<ServiceRepair> ServiceRepairs { get; set; }
    }
}
