using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid OrderId { get; set; } // Mã đơn hàng

        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ProductId { get; set; } // Mã phòng
                                            
        public float Price { get; set; }
        public float Discount { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        public float Total
        {
            get
            {
                return (float) Math.Round(Price * (1 - Discount), 0);
            }
        }

        // ===========================================================
        // Navigation properties
        // ===========================================================

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}