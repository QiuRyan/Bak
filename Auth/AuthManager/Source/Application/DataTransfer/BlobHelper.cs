using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moe.Lib;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DataTransfer
{
    internal class BlobHelper
    {
        public static async Task<GrainStateRecord> ReadStateToBlob(string id)
        {
            string dataConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            CloudStorageAccount account = CloudStorageAccount.Parse(dataConnectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("jym-grains");

            await blobContainer.CreateIfNotExistsAsync();
            CloudBlockBlob blob = GetBlob(blobContainer, id);

            try
            {
                string data = await blob.DownloadTextAsync();
                return JsonConvert.DeserializeObject<GrainStateRecord>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static CloudBlockBlob GetBlob(CloudBlobContainer blobContainer, string id)
        {
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference("Tirisfal-Grains/{0}/{1}.json".FormatWith("Yuyi.Jinyinmao.Domain.User", id.ToUpper()));
            blob.Properties.ContentType = "application/json";
            return blob;
        }
    }
}