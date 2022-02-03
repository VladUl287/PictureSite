using System.IO;
using Microsoft.AspNetCore.Http;

namespace react_Api.Infrastructure.Extensions
{
    public static class FormFileExtensions
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsImage(this IFormFile postedFile)
        {
            var type = postedFile.ContentType.ToLower();
            if (type != "image/jpg" && type != "image/jpeg" && type != "image/pjpeg" 
                && type != "image/x-png" && type != "image/png")
            {
                return false;
            }

            var extension = Path.GetExtension(postedFile.FileName).ToLower();
            if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
            {
                return false;
            }

            if (postedFile.Length < ImageMinimumBytes)
            {
                return false;
            }

            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}