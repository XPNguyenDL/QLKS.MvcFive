using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public enum OrderStatus
    {
        Cancelled,
        Approverd,
        Success,
    }
    public class Order
    {
        public Guid OrderId { get; set; }

        [StringLength(128)]
        public string CustomerId { get; set; } // Mã khách hàng

        [StringLength(128)]
        public string EmployeeId { get; set; } // mã nhân viên

        public DateTime? OrderDate { get; set; } // Ngày đặt phòng

        public DateTime? RequiredDate { get; set; } // Ngày nhận phòng

        [Required, StringLength(500)]
        public string CustomerName { get; set; }

        [Required, StringLength(30)]
        public string CustomerTel { get; set; }

        public OrderStatus Status { get; set; } // Trạng thái đơn hàng

        [StringLength(1000)]
        public string Notes { get; set; } // Ghi chú thêm

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // ===========================================================
        // Navigation properties
        // ===========================================================

        public virtual Account Employee { get; set; }
        public virtual Account Customer { get; set; }
        public virtual IList<OrderDetail> OrderDetails { get; set; }
    }
}