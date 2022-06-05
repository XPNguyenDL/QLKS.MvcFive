using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.DAL
{
    public class HotelDbContext : IdentityDbContext
    {
        public HotelDbContext() : base("DefaultConnection")
        {

        }

        public static HotelDbContext Create()
        {
            return new HotelDbContext();
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ProductHistory> ProductHistories { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<IdentityUserRole> UserInRoles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Mã cấu hình CSDL
            // thiết lập tên cho các bảng lưu trữ thông tin người dùng và quyền
            modelBuilder.Entity<IdentityUser>().ToTable("Accounts");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserInRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");

            
            
            /*
             * Thiết lập mối quan hệ nhiều nhiều giữa loại/Nhóm sản phẩm
             * và sản phẩm. Việc này sẽ tạo ra thêm một bảng mới có tên là
             * ProductCategory. Mỗi sản phẩm có thể thuộc vào nhiều nhóm khác nhau.
             * **/
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Categories)
                .Map(m => m.MapLeftKey("CategoryId")
                    .MapRightKey("ProductId")
                    .ToTable("ProductCategory"));

            // Thiết lập mối quan hệ giữa Đơn hàng và chi tiết đơn hàng
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithRequired(d => d.Order)
                .WillCascadeOnDelete();

            //Thiết lập mối quan hệ giữa sản phẩm và chi tiết đơn hàng
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithRequired(d => d.Product)
                .WillCascadeOnDelete();

            // Thiết lập mối quan hệ giữa nhà cung cấp và sản phẩm
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithRequired(o => o.Supplier)
                .WillCascadeOnDelete();

            // Thiết lập mối quan hệ giữa tài khoản (khách hàng) và đơn hàng
            modelBuilder.Entity<Account>()
                .HasMany(a => a.BuyOrders)
                .WithRequired(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .WillCascadeOnDelete(false);

            // Thiết lập mối quan hệ giữa tài khoản (nhân viên) và đơn hàng
            modelBuilder.Entity<Account>()
                .HasMany(a => a.HandleOrder)
                .WithOptional(o => o.Employee)
                .HasForeignKey(o => o.EmployeeId)
                .WillCascadeOnDelete(false);

            // Thiết Lập mối quan hệ giữa Product và ProductHistory
            modelBuilder.Entity<Product>()
                .HasMany(a => a.ProductHistories)
                .WithRequired(c => c.Product)
                .HasForeignKey(c => c.ProductId)
                .WillCascadeOnDelete();

            // Thiết lập mới quan hệ giữa sản phẩm và Bình luận
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Comments)
                .WithRequired(d => d.Product)
                .WillCascadeOnDelete();

            // Thiết lập quan hệ giữa Sản phẩm và hình ảnh
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Pictures)
                .WithRequired(d => d.Product)
                .WillCascadeOnDelete();

            // Thiết lập giữa mối quan hệ giữa tài khoản và bình luận
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Comments)
                .WithOptional(c => c.Replier)
                .HasForeignKey(c => c.AccountId)
                .WillCascadeOnDelete();

            // Thiết lập giữa mối quan hệ giữa tài khoản và ProductHistory
            modelBuilder.Entity<Account>()
                .HasMany(a => a.ProductHistories)
                .WithOptional(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .WillCascadeOnDelete();


            modelBuilder.Entity<Category>()
                .HasMany(c => c.ChildCates)
                .WithOptional(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Account>()
            //    .HasMany(a => a.Suppliers)
            //    .WithOptional(c => c.Create)
            //    .HasForeignKey(c => c.AccountID)
            //    .WillCascadeOnDelete();
            #endregion

        }

        public System.Data.Entity.DbSet<QLKS.WebApp.Models.Account> IdentityUsers { get; set; }
    }
}