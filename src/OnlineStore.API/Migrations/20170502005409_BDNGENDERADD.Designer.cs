using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OnlineStore.Data;
using OnlineStore.Model;

namespace OnlineStore.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170502005409_BDNGENDERADD")]
    partial class BDNGENDERADD
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("OnlineStore.Model.Entities.Brand", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrandName")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Description")
                        .HasMaxLength(550);

                    b.Property<string>("Logo")
                        .HasMaxLength(550);

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<bool>("isActive");

                    b.HasKey("Id");

                    b.ToTable("Brand");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryDescription")
                        .HasMaxLength(550);

                    b.Property<string>("CategoryName")
                        .HasMaxLength(256);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Logo")
                        .HasMaxLength(550);

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<long?>("ParentId");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(550);

                    b.Property<string>("City")
                        .HasMaxLength(250);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Province")
                        .HasMaxLength(250);

                    b.Property<long>("UserId");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("CustomerAddress")
                        .HasMaxLength(550);

                    b.Property<string>("CustomerEmail")
                        .HasMaxLength(256);

                    b.Property<long>("CustomerId");

                    b.Property<string>("CustomerName")
                        .HasMaxLength(256);

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.OrderDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<long>("OrderId");

                    b.Property<decimal>("Price");

                    b.Property<long>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("BrandId");

                    b.Property<long>("CategoryId");

                    b.Property<int>("Condition");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("ProductDescription")
                        .HasMaxLength(550);

                    b.Property<string>("ProductName")
                        .HasMaxLength(256);

                    b.Property<string>("SKU")
                        .HasMaxLength(100);

                    b.Property<long>("StoreId");

                    b.Property<decimal>("UnitPrice");

                    b.Property<decimal>("Weight");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StoreId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.ProductImage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(550);

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<long>("ProductId");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("RoleName")
                        .HasMaxLength(150);

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Store", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(550);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Description")
                        .HasMaxLength(550);

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Motto")
                        .HasMaxLength(256);

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10);

                    b.Property<string>("StoreName")
                        .HasMaxLength(150);

                    b.Property<long>("UserId");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Store");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDate");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Email");

                    b.Property<string>("FullName")
                        .HasMaxLength(250);

                    b.Property<int>("Gender");

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<string>("Password")
                        .HasMaxLength(256);

                    b.Property<string>("Salt")
                        .HasMaxLength(256);

                    b.Property<string>("UserName")
                        .HasMaxLength(150);

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.UserInRole", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("Npgsql:DefaultValueSql", "current_timestamp");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:DefaultValue", true);

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserInRole");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Category", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.Category", "Parent")
                        .WithMany("SubCategory")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Customer", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Order", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.OrderDetail", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlineStore.Model.Entities.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Product", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.Brand", "Brand")
                        .WithMany("Product")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("OnlineStore.Model.Entities.Category", "Category")
                        .WithMany("Product")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlineStore.Model.Entities.Store", "Store")
                        .WithMany("Products")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.ProductImage", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.Product", "Product")
                        .WithMany("Image")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.Store", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.User", "User")
                        .WithOne("store")
                        .HasForeignKey("OnlineStore.Model.Entities.Store", "UserId");
                });

            modelBuilder.Entity("OnlineStore.Model.Entities.UserInRole", b =>
                {
                    b.HasOne("OnlineStore.Model.Entities.Role", "Role")
                        .WithMany("UserInRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnlineStore.Model.Entities.User", "User")
                        .WithMany("UserRole")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
