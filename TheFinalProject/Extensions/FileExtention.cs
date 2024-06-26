using System;
namespace TheFinalProject.Extensions
{
    public static class FileExtention
    {
        public static bool CheckFileContentType(this IFormFile file, string contentType)
        {
            return file.ContentType == contentType;
        }

        public static bool CheckFileLength(this IFormFile file, short length)
        {
            return (file.Length / 1024) <= length;
        }

        public async static Task<string> CreateFileAsync(this IFormFile file, IWebHostEnvironment env, params string[] folders)
        {
            string fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}-{file.FileName}";

            string filePath = Path.Combine(env.WebRootPath);

            foreach (string folder in folders)
            {
                filePath = Path.Combine(filePath, folder);
            }

            filePath = Path.Combine(filePath, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}

