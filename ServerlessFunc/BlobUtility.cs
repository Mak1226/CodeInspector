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
        /// <summary>
        /// Uploads a DLL file to Azure Blob Storage.
        /// </summary>
        /// <param name="blobname">The name of the blob to upload.</param>
        /// <param name="dll">The byte array containing the DLL file.</param>
        /// <param name="connectionString">The connection string to Azure Blob Storage.</param>
        /// <param name="DllContainerName">The name of the Azure Blob Storage container to upload the DLL to.</param>
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

        /// <summary>
        /// Downloads the specified blob from Azure Blob Storage as a byte array.
        /// </summary>
        /// <param name="containerName">The name of the Azure Blob Storage container to download the blob from.</param>
        /// <param name="blobName">The name of the blob to download.</param>
        /// <param name="connectionString">The connection string to Azure Blob Storage.</param>
        /// <returns>The content of the downloaded blob as a byte array.</returns>
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
                Console.WriteLine( $"An error occurred: {ex.Message}" );
                return null; // Or throw an exception
            }
        }

        /// <summary>
        /// Deletes the specified Azure Blob Storage container.
        /// </summary>
        /// <param name="containerName">The name of the Azure Blob Storage container to delete.</param>
        /// <param name="connectionString">The connection string to Azure Blob Storage.</param>
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
                Console.WriteLine( $"An error occurred: {ex.Message}" );
            }
        }
    }
}
