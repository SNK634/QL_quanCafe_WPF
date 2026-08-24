using System;
using Microsoft.EntityFrameworkCore;

using QL_quanCafe.Models;

namespace QL_quanCafe.Data
{
    public class CoffeeDbContext : DbContext
    {
        // ==========================================
        // 1. DÁNH SÁCH CÁC BẢNG (TABLES)
        // ==========================================
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<MonAn> MonAns { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }

        public CoffeeDbContext()
        {
            // Tự động kiểm tra và khởi tạo CSDL + Các Bảng trên SQL Server
            Database.EnsureCreated();
            //Database.EnsureDeleted();
        }

        // ==========================================
        // 2. CẤU HÌNH KẾT NỐI SQL SERVER
        // ==========================================
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Chuỗi kết nối tới SQL Server SQLEXPRESS01
                string connectionString = @"Server=.\SQLEXPRESS01;Database=QuanCafeDB;Trusted_Connection=True;TrustServerCertificate=True;";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        // ==========================================
        // 3. MỒI DỮ LIỆU BAN ĐẦU (SEED DATA)
        // ==========================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin
            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "Tam123@",
                    FullName = "Quản trị viên",
                    Email = "admin@cafe.com",
                    Phone = "0909123456",
                    Gender = "Nam",
                    Education = "Đại học",
                    Address = "Hệ thống trung tâm",
                    Role = "Admin",
                    NgaySinh = new DateTime(2000, 1, 1)
                }
            );

            // Seed Product
            modelBuilder.Entity<Product>().HasData(

                new Product
                {
                    Id = 1,
                    Name = "AMERICANO",
                    SubName = "Americano",
                    Price = 35000,
                    CategoryCode = "Cafe",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 2,
                    Name = "CAFE CỐT DỪA",
                    SubName = "Vietnamese Coconut Coffee",
                    Price = 55000,
                    CategoryCode = "Cafe",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 3,
                    Name = "ESPRESSO",
                    SubName = "Espresso Shot",
                    Price = 30000,
                    CategoryCode = "Cafe",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 4,
                    Name = "CAFE SỮA ĐÁ",
                    SubName = "Iced Coffee with Milk",
                    Price = 29000,
                    CategoryCode = "Cafe",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 5,
                    Name = "BẠC XỈU",
                    SubName = "White Coffee",
                    Price = 32000,
                    CategoryCode = "Cafe",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 6,
                    Name = "CAPPUCCINO",
                    SubName = "Italian Cappuccino",
                    Price = 45000,
                    CategoryCode = "Cafe",
                    IsAvailable = true
                },


                new Product
                {
                        Id = 7,
                        Name = "MATCHA ĐÁ XAY",
                        SubName = "Matcha Ice Blended",
                        Price = 49000,
                        CategoryCode = "DaXay",
                        IsAvailable = true
                },

                new Product
                {
                    Id = 8,
                    Name = "CHOCOLATE ĐÁ XAY",
                    SubName = "Chocolate Ice Blended",
                    Price = 45000,
                    CategoryCode = "DaXay",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 9,
                    Name = "COOKIE KEM ĐÁ XAY",
                    SubName = "Cookies & Cream Blended",
                    Price = 52000,
                    CategoryCode = "DaXay",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 10,
                    Name = "TRÀ ĐÀO CAM SẢ",
                    SubName = "Peach Orange Lemongrass Tea",
                    Price = 45000,
                    CategoryCode = "Tra",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 11,
                    Name = "TRÀ SEN VÀNG",
                    SubName = "Golden Lotus Tea",
                    Price = 49000,
                    CategoryCode = "Tra",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 12,
                    Name = "TRÀ VẢI",
                    SubName = "Lychee Tea",
                    Price = 43000,
                    CategoryCode = "Tra",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 13,
                    Name = "TRÀ CHANH",
                    SubName = "Lemon Tea",
                    Price = 30000,
                    CategoryCode = "Tra",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 14,
                    Name = "TRÀ SỮA TRÂN CHÂU",
                    SubName = "Boba Milk Tea",
                    Price = 42000,
                    CategoryCode = "TraSua",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 15,
                    Name = "TRÀ SỮA Ô LONG",
                    SubName = "Oolong Milk Tea",
                    Price = 45000,
                    CategoryCode = "TraSua",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 16,
                    Name = "TRÀ SỮA MATCHA",
                    SubName = "Matcha Milk Tea",
                    Price = 47000,
                    CategoryCode = "TraSua",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 17,
                    Name = "NƯỚC ÉP CAM",
                    SubName = "Fresh Orange Juice",
                    Price = 40000,
                    CategoryCode = "NuocEp",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 18,
                    Name = "NƯỚC ÉP DƯA HẤU",
                    SubName = "Watermelon Juice",
                    Price = 38000,
                    CategoryCode = "NuocEp",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 19,
                    Name = "NƯỚC ÉP TÁO",
                    SubName = "Apple Juice",
                    Price = 39000,
                    CategoryCode = "NuocEp",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 20,
                    Name = "YOGURT DÂU",
                    SubName = "Strawberry Yogurt",
                    Price = 42000,
                    CategoryCode = "Yogurt",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 21,
                    Name = "YOGURT XOÀI",
                    SubName = "Mango Yogurt",
                    Price = 43000,
                    CategoryCode = "Yogurt",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 22,
                    Name = "TOPPING TRÂN CHÂU ĐEN",
                    SubName = "Black Boba",
                    Price = 10000,
                    CategoryCode = "Topping",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 23,
                    Name = "TOPPING TRÂN CHÂU TRẮNG",
                    SubName = "White Boba",
                    Price = 10000,
                    CategoryCode = "Topping",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 24,
                    Name = "TOPPING KEM CHEESE",
                    SubName = "Cream Cheese",
                    Price = 12000,
                    CategoryCode = "Topping",
                    IsAvailable = true
                },

                new Product
                {
                    Id = 25,
                    Name = "TOPPING THẠCH TRÁI CÂY",
                    SubName = "Fruit Jelly",
                    Price = 8000,
                    CategoryCode = "Topping",
                    IsAvailable = true
                }







            );
        }
    }
}