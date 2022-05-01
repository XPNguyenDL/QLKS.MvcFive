using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public enum CommentStatus
    {
        Unread, 
        Violate // Vi phạm nội dung
    }
    public class Comment
    {
        public Guid CommentId { get; set; }

        [StringLength(200), Required]
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress), StringLength(300), Required]
        public string Email { get; set; }

        [StringLength(300), Required]
        public string Subject { get; set; } // Tiêu đề bài viết

        [Required, StringLength(4000)]
        public string Content { get; set; } // Nội dung bình luận

        public DateTime PostedTime { get; set; } // Thời gian gửi

        public bool Active { get; set; } // Đánh dấu xóa

        public CommentStatus Status { get; set; }

        [StringLength(4000)]
        public string ReplyContent { get; set; } // Nội dung trả lời

        public DateTime? ReplyTime { get; set; } // Thời gian trả lời

        [StringLength(128)]
        public string AccountId { get; set; } // Mã Nv trả lời

        public Guid ProductId { get; set; } // Mã phòng

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // ===========================================================
        // Navigation properties
        // ===========================================================

        public virtual Account Replier { get; set; }
        public virtual Product Product { get; set; }
    }
}