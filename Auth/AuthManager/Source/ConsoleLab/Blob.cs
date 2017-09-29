using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moe.Lib;
using System;
using System.Threading.Tasks;

namespace ConsoleLab
{
    internal class Blob
    {
        public async Task Test()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse("BlobEndpoint=https://jymstoredev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://jymstoredev.queue.core.chinacloudapi.cn/;TableEndpoint=https://jymstoredev.table.core.chinacloudapi.cn/;AccountName=jymstoredev;AccountKey=1dCLRLeIeUlLAIBsS9rYdCyFg3UNU239MkwzNOj3BYbREOlnBmM4kfTPrgvKDhSmh6sRp2MdkEYJTv4Ht3fCcg==");
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference("jym-grains");

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference("Tirisfal-Grains/{0}/{1}".FormatWith("Yuyi.Jinyinmao.Domain.User", "00F3432B0BBA411CA64D0151B49D5A0B"));
            Console.WriteLine(await blob.DownloadTextAsync());
        }
    }
}