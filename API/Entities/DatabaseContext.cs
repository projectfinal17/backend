using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class DatabaseContext : IdentityDbContext<UserEntity, RoleEntity, Guid>
    {
        public DatabaseContext(DbContextOptions options)
            : base(options) { }

        public DbSet<DemoEntity> Demos { get; set; }
        public DbSet<AccessiblePageEntity> AccessiblePages { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

     
}
}
