using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moe.Lib;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SqlTranferToBlob
{
    internal class BlobHelper
    {
        public static async Task WriteStateToBlob(GrainStateRecord record)
        {
            string dataConnectionString = CloudConfigurationManager.GetSetting("DataConnectionString");
            CloudStorageAccount account = CloudStorageAccount.Parse(dataConnectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("jym-grains");
            CloudBlobContainer snapshotBlobContainer = blobClient.GetContainerReference("jym-grains-snapshot");

            await blobContainer.CreateIfNotExistsAsync();
            await snapshotBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob blob = GetBlob(blobContainer, record);

            CloudBlockBlob blobSnapshot = GetSnapshotBlob(snapshotBlobContainer, record);
            try
            {
                if (record.Type.Contains("Domain.User"))
                {
                    UserState state = record.Data.FromJson<UserState>();
                    state.CredentialNo = state.CredentialNo?.Trim();
                    record.Data = state.ToJson();
                }
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(record.ToJson())))
                {
                    stream.Position = 0;
                    await blob.UploadFromStreamAsync(stream);
                    await blobSnapshot.StartCopyAsync(blob);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static CloudBlockBlob GetBlob(CloudBlobContainer blobContainer, GrainStateRecord record)
        {
            string id = record.Id;
            CloudBlockBlob blob;
            if (id.StartsWith("00000000", StringComparison.Ordinal) && !id.EndsWith("000000", StringComparison.Ordinal))
            {
                blob = blobContainer.GetBlockBlobReference("Tirisfal-Grains/{0}/{1}.json".FormatWith(record.Type, record.LongId));
            }
            else
            {
                blob = blobContainer.GetBlockBlobReference("Tirisfal-Grains/{0}/{1}.json".FormatWith(record.Type, id));
            }
            blob.Properties.ContentType = "application/json";
            return blob;
        }

        private static CloudBlockBlob GetSnapshotBlob(CloudBlobContainer snapshotBlobContainer, GrainStateRecord record)
        {
            string id = record.Id;
            CloudBlockBlob blob;
            if (id.StartsWith("00000000", StringComparison.Ordinal) && !id.EndsWith("000000", StringComparison.Ordinal))
            {
                blob = snapshotBlobContainer.GetBlockBlobReference("Tirisfal-Grains/{0}/{1}/{2}.json".FormatWith(record.Type, record.LongId, DateTime.UtcNow.UnixTimestamp()));
            }
            else
            {
                blob = snapshotBlobContainer.GetBlockBlobReference("Tirisfal-Grains/{0}/{1}/{2}.json".FormatWith(record.Type, id, DateTime.UtcNow.UnixTimestamp()));
            }
            blob.Properties.ContentType = "application/json";
            return blob;
        }
    }
}