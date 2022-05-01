using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public class Picture
    {
        public Guid PictureId { get; set; }

        public string Caption { get; set; } // Tiêu đề

        public string Path { get; set; } // Đường dẫn

        public int OrderNo { get; set; } // Thứ tự hiển thị

        public bool Active { get; set; } // Đánh dấu xóa 
        public Guid ProductId { get; set; } // Mã phòng

        // ===========================================================
        // Navigation properties
        // ===========================================================
        public virtual Product Product { get; set; }
    }
}