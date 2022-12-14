using Azure.Storage.Blobs;

namespace ExploreBulgaria.Services.Data
{
    public class BlobServiceClientCustom : IBlobServiceClient
    {
        private readonly BlobServiceClient blobServiceClient;

        public BlobServiceClientCustom(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public BlobContainerClient GetBlobContainerClient(string blobContainerName)
            => blobServiceClient.GetBlobContainerClient(blobContainerName);
    }
}
