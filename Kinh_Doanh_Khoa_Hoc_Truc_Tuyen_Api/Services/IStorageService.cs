using System.IO;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string folder);

        Task DeleteFileAsync(string fileName);

        string GetFileRoot(string fileName, string folder);
    }
}