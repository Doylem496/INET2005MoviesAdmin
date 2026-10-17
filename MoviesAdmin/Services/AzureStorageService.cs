using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MoviesAdmin.Services
{
    public class AzureStorageService : IAzureStorageService
    {

        private readonly IConfiguration _configuration;

        private readonly BlobServiceClient blobServiceClient;
        private string containerName;

        public AzureStorageService(IConfiguration configuration)
        {
            //  to get appsettings user secrets
            _configuration = configuration;

            containerName = _configuration["AzureStorage:ContainerName"] ?? throw new InvalidOperationException("Missing Configuration: Container Name");

            string connectionString = _configuration["AzureStorage:ConnectionString"] ?? throw new InvalidOperationException("Missing Configuration: Connection String");

            blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string filename)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(filename);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            };


            // file I/O
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, options);
            }

            // return file url 
            return blobClient.Uri.ToString();
        }
        
    }
}
 