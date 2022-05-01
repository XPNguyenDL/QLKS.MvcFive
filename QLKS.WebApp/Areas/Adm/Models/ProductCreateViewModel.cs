using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using QLKS.Core.DataAnnotations;

namespace QLKS.WebApp.Areas.Adm.Models
{
    [Bind(Exclude = "Categories, Suppliers")]
    public class ProductCreateViewModel
    {
        [Required, StringLength(100), Display(Name = "Tên khách sạn")]
        public string Name { get; set; }

        [Required, StringLength(100), Display(Name = "Tên định danh")]
        [Remote("CheckUniqueAlias", "Product", AdditionalFields = "ProductId")]
        public string Alias { get; set; } // Tên định danh

        [Required, StringLength(100), Display(Name = "Số hiệu khách sạn")]
        [Remote("CheckUniqueCode", "Product", AdditionalFields = "ProductId",
            ErrorMessage = "{0} này đã được sử dụng cho sản phẩm khác")]
        public string ProductCode { get; set; } // Số hiệu khách sạn

        [Display(Name = "Hình đại diện")]
        [FileType("jpg,jpeg,png,gif"), FileSize(1)]
        [ImageSize(Width = 400, Height = 500)]
        public HttpPostedFileBase Upload { get; set; }

        [StringLength(500), DataType(DataType.MultilineText)]
        [Display(Name = "Giới thiệu")]
        public string ShortIntro { get; set; }

        [AllowHtml, Display(Name = "Mô tả chi tiết")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [StringLength(50), Display(Name = "Đơn vị tính")]
        public string QtyPerUnit { get; set; }

        [Display(Name = "Giá bán"), Range(1000, 100000000)]
        public float Price { get; set; }

        [Range(0, 100000000), Display(Name = "Giảm giá")]
        public float Discount { get; set; }
        
    }
}