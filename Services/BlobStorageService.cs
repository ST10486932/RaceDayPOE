using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RaceDay2.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _container;
        private const long MaxBytes = 2 * 1024 * 1024; // 2 MB

        private static readonly Dictionary<string, string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png",
            [".webp"] = "image/webp"
        };

        public BlobStorageService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            var containerName = config["AzureStorage:ContainerName"] ?? "event-banners";

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("AzureStorage:ConnectionString is not configured.");

            _container = new BlobContainerClient(connectionString, containerName);
        }

        public string? Validate(IFormFile file)
        {
            if (file.Length == 0) return "The selected file is empty.";
            if (file.Length > MaxBytes) return "The banner must be 2 MB or smaller.";

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedTypes.TryGetValue(ext, out var expectedType) ||
                !string.Equals(file.ContentType, expectedType, StringComparison.OrdinalIgnoreCase))
            {
                return "Only JPG, PNG or WebP images are allowed.";
            }
            return null;
        }

        public async Task<string> UploadBannerAsync(IFormFile file, CancellationToken ct = default)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var blobName = $"banners/{Guid.NewGuid():N}{ext}";
            var blob = _container.GetBlobClient(blobName);

            await using var stream = file.OpenReadStream();

            await blob.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = AllowedTypes[ext],
                    CacheControl = "public, max-age=31536000"
                }
            }, ct);

            return blob.Uri.ToString();
        }

        public async Task DeleteAsync(string blobName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(blobName)) return;
            await _container.DeleteBlobIfExistsAsync(blobName, cancellationToken: ct);
        }
    }
}