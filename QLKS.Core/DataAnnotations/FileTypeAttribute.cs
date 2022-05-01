using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace QLKS.Core.DataAnnotations
{
    public enum ImageValidationResult
    {
        // Kiểu nội dung tập tin không phải hình ảnh
        InvalidMimeType,
        
        // Định dạnh ảnh không được phép upload
        NotAllowedType,

        // Tập tin không phải là tập tin ảnh
        InvaliHeader,

        // Ảnh có kích cỡ vượt quá quy định
        OverSize,

        // Hợp lệ
        Valid
    }

    /// <summary>
    /// Thuộc tính quy định loại tập tin được upload
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FileTypeAttribute : ValidationAttribute
    {
        private readonly List<string> allowFileTypes;

        // allowFileTypes chuỗi chứa danh các phần mở rộng
        // của tập tin được phép upload, phân tách nhau bởi dấu phẩy
        // sử dụng (FILE_TYPES) trong ErrorMessage để đánh dấu
        // chỗ sẽ thay đổi bởi danh sách các loại file được upload.

        public FileTypeAttribute(string allowTypes)
            : base("Chỉ được phép upload và các tập tin {FILE_TYPES}")
        {
            // PHân tách
            allowFileTypes = allowTypes
                .Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();
        }

        public override bool IsValid(object value)
        {
            // Lấy đối tượng lưu tập tin được upload
            var upload = value as HttpPostedFileBase;

            // Nếu không có tập tin được post lên thì xem như hợp lệ
            if (upload == null) return true;

            // NẾu có, Kiểm tra loại tập tin có hợp lệ
            // Lấy phần mở rộng của tập tin
            var fileExt = Path.GetExtension(upload.FileName);

            if (!string.IsNullOrEmpty(fileExt))
            {
                // Bỏ dấu chấm ('.')
                fileExt = fileExt.Substring(1);
                
                // Trả về true nếu phần mở rộng nằm trong danh sách cho phép

                return allowFileTypes.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
            }
            
            //
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            var fileTypes = string.Join(", ", allowFileTypes);

            var errorMessage = base.ErrorMessageString;

            if (errorMessage != null && errorMessage.Contains("{FILE_TYPES}"))
            {
                errorMessage = errorMessage.Replace("{FILE_TYPES}", fileTypes);
            }
            return errorMessage;
        }
    }

}
