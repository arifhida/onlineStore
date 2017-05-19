using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlineStore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Store> Stores { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().Property(e => e.UserName).HasMaxLength(150);
            modelBuilder.Entity<User>().Property(e => e.FullName).HasMaxLength(250);
            modelBuilder.Entity<User>().Property(e => e.Password).HasMaxLength(256);
            modelBuilder.Entity<User>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<User>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<User>().Property(e => e.Salt).HasMaxLength(256);
            modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique(true);
            modelBuilder.Entity<User>().HasIndex(e => e.UserName).IsUnique(true);
            modelBuilder.Entity<User>().Ignore(e => e.Delete);

            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Customer>().Property(e => e.Address).HasMaxLength(550);
            modelBuilder.Entity<Customer>().Property(e => e.City).HasMaxLength(250);
            modelBuilder.Entity<Customer>().Property(e => e.Province).HasMaxLength(250);
            modelBuilder.Entity<Customer>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<Customer>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<Customer>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<Customer>().HasOne(e => e.User).WithOne(r => r.Customer)
                .HasForeignKey<Customer>(r => r.UserId);
            modelBuilder.Entity<Customer>().Ignore(e => e.Delete);

            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Role>().Property(e => e.RoleName).HasMaxLength(150);
            modelBuilder.Entity<Role>().Property(e => e.Description).HasMaxLength(256);
            modelBuilder.Entity<Role>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<Role>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<Role>().Ignore(e => e.Delete);

            modelBuilder.Entity<UserInRole>().ToTable("UserInRole");
            modelBuilder.Entity<UserInRole>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserInRole>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<UserInRole>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<UserInRole>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<UserInRole>().HasKey(ui => new { ui.UserId, ui.RoleId });
            modelBuilder.Entity<UserInRole>().HasOne(u => u.Role)
                .WithMany(ui => ui.UserInRole).HasForeignKey(ui => ui.RoleId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserInRole>().HasOne(u => u.User)
                .WithMany(ui => ui.UserRole).HasForeignKey(ui => ui.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserInRole>().Ignore(e => e.Delete);

            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Category>().Property(e => e.CategoryName).HasMaxLength(256);
            modelBuilder.Entity<Category>().Property(e => e.Logo).HasMaxLength(550);
            modelBuilder.Entity<Category>().Property(e => e.CategoryDescription).HasMaxLength(550);
            modelBuilder.Entity<Category>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<Category>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<Category>().HasOne(x => x.Parent).WithMany(e => e.SubCategory)
                .HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            modelBuilder.Entity<Category>().Ignore(e => e.Delete);

            modelBuilder.Entity<Brand>().ToTable("Brand");
            modelBuilder.Entity<Brand>().Property(x => x.BrandName).HasMaxLength(250);
            modelBuilder.Entity<Brand>().Property(x => x.Description).HasMaxLength(550);
            modelBuilder.Entity<Brand>().Property(x => x.Logo).HasMaxLength(550);
            modelBuilder.Entity<Brand>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<Brand>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<Brand>().Ignore(e => e.Delete);

            modelBuilder.Entity<Store>().ToTable("Store");
            modelBuilder.Entity<Store>().Property(e => e.StoreName).HasMaxLength(150);
            modelBuilder.Entity<Store>().Property(e => e.Motto).HasMaxLength(256);
            modelBuilder.Entity<Store>().Property(e => e.Description).HasMaxLength(550);
            modelBuilder.Entity<Store>().Property(e => e.Address).HasMaxLength(550);
            modelBuilder.Entity<Store>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<Store>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<Store>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<Store>().Property(e => e.PostalCode).HasMaxLength(10);
            modelBuilder.Entity<Store>().HasOne(u => u.User).WithOne(r => r.Store)
                .HasForeignKey<Store>(x => x.UserId);
            modelBuilder.Entity<Store>().Ignore(e => e.Delete);

            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Product>().Property(x => x.SKU).HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(e => e.ProductName).HasMaxLength(256);
            modelBuilder.Entity<Product>().Property(e => e.ProductDescription).HasMaxLength(550);            
            modelBuilder.Entity<Product>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<Product>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<Product>().HasOne(e => e.Store).WithMany(r => r.Products)
                .HasForeignKey(k => k.StoreId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(e => e.Product)
                .HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.Cascade).IsRequired(true);
            modelBuilder.Entity<Product>().HasOne(x => x.Brand).WithMany(e => e.Product)
                .HasForeignKey(e => e.BrandId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Product>().HasOne(x => x.Store).WithMany(p => p.Products)
                .HasForeignKey(r => r.StoreId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>().Ignore(e => e.Delete);

            modelBuilder.Entity<ProductImage>().ToTable("ProductImage");
            modelBuilder.Entity<ProductImage>().Property(e => e.ImageUrl).HasMaxLength(550);
            modelBuilder.Entity<ProductImage>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductImage>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<ProductImage>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<ProductImage>().HasOne(x => x.Product).WithMany(r => r.Image)
                .HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductImage>().Ignore(e => e.Delete);

            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Order>().Property(e => e.CustomerName).HasMaxLength(256);
            modelBuilder.Entity<Order>().Property(e => e.CustomerEmail).HasMaxLength(256);
            modelBuilder.Entity<Order>().Property(e => e.CustomerAddress).HasMaxLength(550);
            modelBuilder.Entity<Order>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<Order>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<Order>().HasOne(u => u.Customer).WithMany(x => x.Orders)
                .HasForeignKey(e => e.CustomerId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Order>().Ignore(e => e.Delete);

            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            modelBuilder.Entity<OrderDetail>().Property(e => e.CreatedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderDetail>().Property(e => e.ModifiedDate).ForNpgsqlHasDefaultValueSql("current_timestamp").ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<OrderDetail>().Property(e => e.isActive).ForNpgsqlHasDefaultValue(true);
            modelBuilder.Entity<OrderDetail>().HasOne(e => e.Order).WithMany(x => x.OrderDetails)
                .HasForeignKey(r => r.OrderId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>().HasOne(x => x.Product).WithMany(r => r.OrderDetails)
                .HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<OrderDetail>().Ignore(e => e.Delete);

            base.OnModelCreating(modelBuilder);

        }

    }
}
