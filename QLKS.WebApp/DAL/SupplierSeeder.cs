using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using QLKS.WebApp.Models;

namespace QLKS.WebApp.DAL
{
    public class SupplierSeeder
    {
        public static void Seed(HotelDbContext context)
        {
            context.Suppliers.AddOrUpdate(
                s => s.Name,
                new Supplier()
                {
                    SupplierId = Guid.NewGuid(),
                    Name = "Công Ty TNHH TM DV Và Sản Xuất Tiến Dũng - New Lotus Hotel",
                    ContactName = "Bà Lê Thị Thiệu",
                    Description = "NEW LOTUS HOTEL với 30 phòng rộng rãi sẽ đem đến " + 
                                  "những trải nghiệm thư thái trong suốt kỳ nghỉ.",
                    Address = "B24-D7 Khu Đô Thị Mới, P. Dịch Vọng, Q. Cầu Giấy,Hà Nội",
                    Email = "info@newlotushotel.com.vn",
                    HomePage = "www.facebook.com/newlotushotel.hanoi",
                    Phone = "02437925359",
                    Actived = true,
                    Alias = "New-Lotus-Hotel"
                });
            context.SaveChanges();
        }
    }
}