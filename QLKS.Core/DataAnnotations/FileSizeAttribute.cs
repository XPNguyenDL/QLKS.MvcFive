using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace QLKS.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int maxSize;

        // maxSize = Dung lượng tối đa, tính theo Mb
        // Sử dụng {MAXSIZE) để đánh dấu sẽ thay bằng giá trị maxSize
        public FileSizeAttribute(int maxSize) : base("Dung lượn quá lớn {MAXSIZE} Mb")
        {
            this.maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            // Lấy đối tượng 
            var upload = value as HttpPostedFileBase;

            if (upload == null) return true;

            return upload.ContentLength < maxSize * 1024 * 1024;
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessage = base.ErrorMessageString;

            if (errorMessage != null && errorMessage.Contains("{MAXSIZE}"))
            {
                errorMessage = errorMessage.Replace("{MAXSIZE}", maxSize.ToString());
            }

            return errorMessage;
        }
    }
}
