using Azure.Storage.Blobs;

namespace ExploreBulgaria.Services.Data
{
    public interface IBlobServiceClient
    {
        public BlobContainerClient GetBlobContainerClient(string blobContainerName);
    }
}
