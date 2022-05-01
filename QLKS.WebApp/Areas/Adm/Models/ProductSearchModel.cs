using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Areas.Adm.Models
{
    public class ProductSearchModel
    {
        // Từ khóa người nhập vào để tìm kiếm
        // Tên khách sạn, giới thiệu hay mô tả chi tiết
        public string Keyword { get; set; }

        // Số tt trang hiện tại và số mẫu tin trang
        public int? PageIndex { get; set; }
        public int? PageSize  { get; set; }

        // Trạng thái sản phẩm
        public bool? Actived { get; set; }


    }
}