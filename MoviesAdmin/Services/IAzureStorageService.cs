namespace MoviesAdmin.Services
{
    public interface IAzureStorageService
    {
       // string containerName as a a parameteror not? It is stored in appsettings.json for now 
            public Task<string> UploadFileAsync(IFormFile file, string fileName);

    }
}
