using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.DAL
{
    public class CategorySeeder
    {
        public static void Seed(HotelDbContext context)
        {
            var topCases = new Category[]
            {
                new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = "Room Standard",
                    Alias = "room-standard",
                    Size = 22,
                    Description = "Standard là hạng phòng tiêu chuẩn trong khách sạn, có diện tích nhỏ," +
                                  " thường nằm ở tầng thấp, không có view hoặc view không đẹp.",
                    IconPath = "~/Images/room-standard.jpg",
                },
                new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = "Room Superior",
                    Alias = "room-superior",
                    Size = 22,
                    Description = "So với Standard thì Superior là hạng phòng có chất lượng tốt hơn - diện tích rộng " +
                                  " - trang thiết bị tiện nghi - view đẹp. Cũng vì thế mà mức giá thuê phòng SUP sẽ cao hơn",
                    IconPath = "~/Images/room-superior.jpg",

                },
                new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = "Room Deluxe",
                    Alias = "room-deluxe",
                    Size = 22,
                    Description = "Đây là loại phòng cao cấp trong các khách sạn, chủ yếu nằm trên tầng cao với không gian rộng, " +
                                  "nhiều thiết bị tiện nghi - view ngắm cảnh đẹp.",
                    IconPath = "~/Images/room-deluxe.jpg",

                },
                new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = "Room Suite",
                    Alias = "room-suite",
                    Size = 22,
                    Description = "SUT là hạng phòng cao cấp nhất của mỗi khách sạn. Và với mục đích tăng thêm mức độ VIP, " + 
                                  " phòng SUT hay được đặt tên là: phòng Hoàng gia (Royal Suite Room), phòng Tổng Thống (President Room)… ",
                    IconPath = "~/Images/room-suite.jpg",

                }

            };
            context.Categories.AddOrUpdate(x => x.Alias, topCases);
            context.SaveChanges();
        }
    }
}