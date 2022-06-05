using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; } // Mã loại phòng

        public string Name { get; set; } // Tên loại phòng

        [Required, StringLength(100)]
        public string Alias { get; set; } //Tên gọi khác

        public int Size { get; set; }

        public string Description { get; set; } // Mô tả sơ lược

        public string IconPath { get; set; } // link hình ảnh
        public Guid? ParentId { get; set; } // Mã loại sản phẩm cha
        public int? ParentLevel { get; set; } // 
        public int OrderNo { get; set; }
        

        public bool Actived { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // ===========================================================
        // Navigation properties
        // ===========================================================

        public virtual Category Parent { get; set; }
        public virtual IList<Category> ChildCates { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}