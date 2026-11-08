using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RaceDay2.Services
{
    public interface IBlobStorageService
    {
        string? Validate(IFormFile file);
        Task<string> UploadBannerAsync(IFormFile file, CancellationToken ct = default);
        Task DeleteAsync(string blobName, CancellationToken ct = default);
    }
}