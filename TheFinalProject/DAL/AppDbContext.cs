using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Collections.Generic;
using System.Net;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TheFinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace TheFinalProject.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }


    }

}
