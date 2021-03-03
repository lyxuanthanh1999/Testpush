using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services
{
    public class StorageService : IStorageService
    {
        private readonly string _userContentFolder;

        private const string UserContentFolderName = "attachments";

        public StorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, UserContentFolderName);
        }

        public string GetFileUrl(string fileName)
        {
            return string.IsNullOrEmpty(fileName) ? "" : $"/{UserContentFolderName}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string folder)
        {
            if (!Directory.Exists(_userContentFolder))
                Directory.CreateDirectory(_userContentFolder);

            var filePath = Path.Combine(_userContentFolder, folder + "\\" + fileName);
            await using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public string GetFileRoot(string fileName, string folder)
        {
            return Path.Combine(_userContentFolder, folder + "\\" + fileName);
        }
    }
}