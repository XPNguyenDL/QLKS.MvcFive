using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLKS.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ImageSizeAttribute : ValidationAttribute
    {
        private ImageValidationResult ivResult = ImageValidationResult.Valid;

        /// <summary>
        /// Mãng lưu các định dạng nội dung tập tin hình ảnh
        /// </summary>

        private static string[] mimeTypes =
        {
            "image/jpg", "image/jpeg", "image/pjpeg", "image/gif",
            "image/x-png", "image/png", "image/bmp", "image/x-icon",
            "image/x-tiff", "image/tiff"
        };

        /// <summary>
        /// Mãng lưu loại hình được phép upload
        /// </summary>

        private static string[] imageExts =
        {
            ".jpg", ".jpeg", ".pjpeg", ".gif",
            ".x-png", ".png", ".bmp", ".x-icon",
            ".x-tiff", ".tiff"
        };

        
        public int Width { get; set; } // pixel
        public int Height { get; set; } // pixel


        // Sử dụng {Width} và {Height} để đánh dấu vị trí
        // sẽ thay đối bởi độ rộng và chiều cao tối đa
        public ImageSizeAttribute() : base("Kích thước hình vượt quá kích cỡ {Width}x{Height}")
        {
            
        }

        /// <summary>
        /// Kiểm tra kiểu nội dung của tập tin được upload
        /// có phải là kiểu nội dung hình ảnh
        /// </summary>
        /// <param name="Upload">Tập tin được upload</param>
        /// <return>
        /// trả về flase nếu không đúng định dạng nội dung
        /// của các loại tập tin hình ảnh. True nếu ngược lại
        /// </return>
        private bool CheckMimeTypes(HttpPostedFileBase upload)
        {
            var contentType = upload.ContentType.ToLower();

            // Nếu nội dung không thuộc kiểu hình ảnh
            if (!mimeTypes.Contains(contentType))
            {
                // Đánh dấu Mime type không hợp lệ
                ivResult = ImageValidationResult.InvalidMimeType;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra tên phần mở rộng của tập tin có nằm trong
        /// danh sách những loại hình ảnh được phép upload?
        /// </summary>
        /// <param name="Upload">Tập tin được upload</param>
        /// <return>
        /// trả về flase nếu tên mở rộng không nằm trong danh
        /// sách các định dạng được phép upload. True nếu ngược lại
        /// </return>
        private bool CheckFileExtension(HttpPostedFileBase upload)
        {
            // Lấy phần mở rộng của tập tin 
            var fileExt = Path.GetExtension(upload.FileName);

            // Nếu có phần tên mở rộng
            if (!string.IsNullOrWhiteSpace(fileExt))
            {
                // trả về true nếu phần mở rộng nằm trong danh sách cho phép
                if (imageExts.Contains(fileExt, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            // đánh dấu định dạng file không hợp lệ
            ivResult = ImageValidationResult.NotAllowedType;

            return false;
        }

        private bool CheckImageSize(HttpPostedFileBase upload)
        {
            if (!upload.InputStream.CanRead)
            {
                ivResult = ImageValidationResult.InvaliHeader;
                return false;
            }

            try
            {
                // Tạo hình ảnh từ luồng upload
                using (var image = Image.FromStream(upload.InputStream))
                {
                    // kiểm tra kích cỡ ảnh có vượt quá cỡ cho phép
                    // Nếu có, trả về false
                    if (image.Width > Width || image.Height > Height)
                    {
                        ivResult = ImageValidationResult.OverSize;
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                ivResult = ImageValidationResult.InvaliHeader;
                return false;
            }
        }

        public override bool IsValid(object value)
        {
            // lấy đối tượng lưu tập tin được upload
            var upload = value as HttpPostedFileBase;

            // nếu không có tập tin được lên thi xem như hợp lệ
            if (upload == null)
            {
                return true;
            }

            // Lần lượt thực hiện các thao tác kiểm tra
            bool valid = true;

            // Kiểm tra mime types 
            valid = CheckMimeTypes(upload);

            // Kiểm tra phần đuôi mở rộng
            if (valid) valid = CheckFileExtension(upload);

            // Kiểm tra header có đúng định dạng ảnh
            // if (valid) valid = CheckFileHeader(upload);

            //Kiểm tra kích cở ảnh
            if (valid) valid = CheckImageSize(upload);

            return valid;
        }


        public override string FormatErrorMessage(string name)
        {
            var errorMessage = base.ErrorMessageString;

            switch (ivResult)
            {
                case ImageValidationResult.InvaliHeader:
                    return "Không thể đọc được nội dung của file ảnh.";

                case ImageValidationResult.InvalidMimeType:
                case ImageValidationResult.NotAllowedType:
                    return "Hệ thống không hỗ trợ định dạnh ảnh này.";
                case ImageValidationResult.OverSize:
                    if (errorMessage != null)
                    {
                        if (errorMessage.Contains("{Width}"))
                        {
                            errorMessage = errorMessage.Replace(
                                            "{Width}", Width.ToString());
                        }
                        if (errorMessage.Contains("{Height}"))
                        {
                            errorMessage = errorMessage.Replace(
                                "{Height}", Height.ToString());
                        }
                    }

                    return errorMessage;
                default:
                    return errorMessage;
            }
        }
    }
}