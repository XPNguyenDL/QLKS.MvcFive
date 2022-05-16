using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public class Supplier
    {
        public Guid SupplierId { get; set; }

        [DisplayName("Hotel Name"), StringLength(500)]
        public string Name { get; set;}

        [Required, StringLength(100)]
        public string Alias { get; set; } //Tên gọi khác

        [StringLength(2000)]
        public string Description { get; set;} // Mô tả sơ lược về khách sạn

        [Required, StringLength(200)]
        public string ContactName { get; set;} // Tên người đại diện

        [StringLength(200)]
        public string ContactTitle { get; set;}

        [Required, StringLength(500)]
        public string Address { get; set;}

        [DataType(DataType.EmailAddress)]
        public string Email { get; set;}

        [StringLength(30), RegularExpression(@"\d{3,4}-\d{3}-\d{4,5}")]
        

        public string Phone { get; set;}

        [StringLength(100), DataType(DataType.Url)]
        public string HomePage { get; set;}
        

        public bool Actived { get; set;}

        [Timestamp]
        public byte[] RowVersion { get; set;}

        // ===========================================================
        // Navigation properties
        // ===========================================================
        public virtual IList<Product> Products { get; set; }
    }
}