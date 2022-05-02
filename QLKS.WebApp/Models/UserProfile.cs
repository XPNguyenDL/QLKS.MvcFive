using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLKS.WebApp.Models
{
    public class UserProfile
    {
        [Key, ForeignKey("Account"), StringLength(128)]
        public string AccountID { get; set; } // Mã tài khoản

        [Required, StringLength(200)]
        public string FirstName { get; set; }

        [Required, StringLength(200)]
        public string LastName { get; set;}

        [StringLength(200)]
        public string JobPosition { get; set; } //Chức vụ công việc

        [Required, StringLength(100), RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,40})")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } // đặt password cho tài khoản


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(500)]
        public string Picture { get; set; } //Hình đại diện

        [StringLength(2000)]
        public string Note { get; set; } // Ghi chú thêm

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        // ===========================================================
        // Navigation properties
        // ===========================================================
        public virtual Account Account { get; set; }
    }
}