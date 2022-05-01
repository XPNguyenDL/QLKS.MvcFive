using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace QLKS.WebApp.Models
{
    public class Product
    {

        public Guid ProductID { get; set; } // Mã phòng

        [Required, StringLength(300), Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required, StringLength(300)]
        public string Alias { get; set; } // Tên không dấu

        [Required, StringLength(300)]
        public string ThumbImage { get; set; }

        [StringLength(500)]
        public string ShortIntro { get; set; } // Giới thiệu

        [Column(TypeName = "ntext"), AllowHtml]
        public string Description { get; set; }

        public float Price { get; set; } // giá phòng

        public bool CheckRoom { get; set; } // Kiểm tra còn phòng hay không

        [Range(0, 1), DisplayFormat(DataFormatString = "{0:P1}")]
        public float Discount { get; set; } // % giảm giá
        
        public Guid SupplierId { get; set; } // mã nhà cung cấp

        public bool Actived { get; set; }

        [Timestamp, JsonIgnore]
        public byte[] RowVersion { get; set; }

        // ===========================================================
        // Navigation properties
        // ===========================================================

        [JsonIgnore]
        public virtual Supplier Supplier { get; set; }

        [JsonIgnore]
        public virtual IList<Category> Categories { get; set; }

        [JsonIgnore]
        public virtual IList<OrderDetail> OrderDetails { get; set; }

        [JsonIgnore]
        public virtual IList<Comment> Comments { get; set; }

        [JsonIgnore]
        public virtual IList<Picture> Pictures { get; set; }

        [JsonIgnore]
        public virtual IList<ProductHistory> ProductHistories { get; set; }
    }
}