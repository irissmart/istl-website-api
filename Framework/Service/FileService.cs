using Framework.Interface;
using Microsoft.AspNetCore.Http;

namespace Framework.Service
{
    public class FileService : IFileService
    {
        public async Task<List<string>> UploadAsync(string uploadPath, List<IFormFile> files)
        {
            var fileNames = new List<string>();

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploadPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    fileNames.Add(file.FileName);
                }
            }

            return fileNames;
        }

        public async Task<string> UploadAsync(string uploadPath, IFormFile file)
        {
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = GenerateRandomString() + file.FileName;

            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return fileName;
        }

        public void DeleteFile(string uploadPath, string imageName)
        {
            var filePath = Path.Combine(uploadPath, imageName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GenerateRandomString()
        {
            Random res = new Random();

            String str = "abcdefghijklmnopqrstuvwxyz";
            int size = 20;

            String ran = "";

            for (int i = 0; i < size; i++)
            {
                int x = res.Next(26);
                ran = ran + str[x];
            }

            return ran;
        }

    }
}