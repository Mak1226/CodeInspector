using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ServerlessFunc
{
    public class BlobUtility
    {
        public static async Task UploadSubmissionToBlob( string blobname , byte[] dll , string connectionString , string DllContainerName )
        {
            try
            {
                BlobServiceClient blobServiceClient = new( connectionString );
                BlobContainerClient DllcontainerClient = blobServiceClient.GetBlobContainerClient( DllContainerName );

                // Create the container if it doesn't exist
                await DllcontainerClient.CreateIfNotExistsAsync();

                // Upload the file to Azure Blob Storage
                BlobClient DllblobClient = DllcontainerClient.GetBlobClient( blobname );
                await DllblobClient.UploadAsync( new MemoryStream( dll ) , true );
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Console.WriteLine( $"An error occurred: {ex.Message}" );
            }
        }

        public static async Task<byte[]> GetBlobContentAsync( string containerName , string blobName , string connectionString )
        {
            try
            {
                BlobServiceClient blobServiceClient = new( connectionString );
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient( containerName );
                BlobClient blobClient = containerClient.GetBlobClient( blobName );

                // Check if the blob exists
                if (!await blobClient.ExistsAsync())
                {
                    return null; // Or throw an exception, depending on your requirements
                }

                // Download the blob content as a byte array
                Response<BlobDownloadInfo> response = await blobClient.DownloadAsync();
                BlobDownloadInfo blobInfo = response.Value;

                using MemoryStream memoryStream = new();
                await blobInfo.Content.CopyToAsync( memoryStream );
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                // TODO : Add logs
                Console.WriteLine( $"An error occurred: {ex.Message}" );
                return null; // Or throw an exception
            }
        }

        public static async Task DeleteContainer( string containerName , string connectionString )
        {
            try
            {
                BlobServiceClient blobServiceClient = new( connectionString );
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient( containerName );

                await containerClient.DeleteAsync();
            }
            catch (Exception ex)
            {
                // TODO : Add logs
                Console.WriteLine( $"An error occurred: {ex.Message}" );
            }
        }
    }
}
