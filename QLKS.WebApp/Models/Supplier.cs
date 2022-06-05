using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public class Supplier : IEnumerable
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

        [StringLength(30), RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Vui lòng nhập lại số điện thoại!")]
        public string Phone { get; set;}

        [StringLength(100), DataType(DataType.Url)]
        public string HomePage { get; set; }
        public string Image { get; set; }
        [StringLength(5000)]
        public string ShortInfo { get; set; }


        public bool Actived { get; set;}

        [Timestamp]
        public byte[] RowVersion { get; set;}

        // ===========================================================
        // Navigation properties
        // ===========================================================
        public virtual IList<Product> Products { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}