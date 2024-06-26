using System;
namespace TheFinalProject.Helpers
{
    public class FileHelper
    {
        public static void DeleteFile(string fileName, IWebHostEnvironment env, params string[] folders)
        {
            string filePath = Path.Combine(env.WebRootPath);

            foreach (string folder in folders)
            {
                filePath = Path.Combine(filePath, folder);
            }

            filePath = Path.Combine(filePath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}

