using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public enum ProductHistoryAction
    {
        Create,
        Delete,
        UpdateFull,
        UpdatePrice,
        UpdateQuantity
    }
    public class ProductHistory
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        [StringLength(128)]
        public string AccountId { get; set; }

        public DateTime ActionTime { get; set; }

        public ProductHistoryAction HistoryAction { get; set; }

        [Column(TypeName = "ntext")]
        public string OriginalProduct { get; set; }

        [Column(TypeName = "ntext")]
        public string ModifiedProduct { get; set; }

        // ===========================================================
        // Navigation properties
        // ===========================================================

        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }
    }
}