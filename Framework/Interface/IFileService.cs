using Microsoft.AspNetCore.Http;

namespace Framework.Interface
{
    public interface IFileService
    {
        Task<List<string>> UploadAsync(string uploadPath, List<IFormFile> files);

        Task<string> UploadAsync(string uploadPath, IFormFile file);

        void DeleteFile(string uploadPath, string imageName);
    }
}