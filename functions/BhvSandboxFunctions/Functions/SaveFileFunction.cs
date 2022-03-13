// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BhvSandboxFunctions
{
    public static class SaveFileFunction
    {
        [FunctionName("SaveFileFunction")]
        public static void Run(
                [EventGridTrigger] EventGridEvent @event,
                ILogger log)
        {
            var data = GetEventData<QueueMessageDto>(@event);
            log.LogInformation($"Payload - Name:{data.Name} FileUrl:{data.FileUrl}");
            try
            {
                using (WebClient client = new WebClient())
                {
                    var bytes = client.DownloadDataTaskAsync(new Uri(data.FileUrl)).Result;
                    UploadFileToBlob(data.Name + ".png", bytes);
                    log.LogInformation($"Successfully uploaded {data.Name}");
                }
            }
            catch (Exception e)
            {
                log.LogError(e, $"Failed upload {data.FileUrl}");
            }
        }

        private static void UploadFileToBlob(string blobName, byte[] data)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            var connectionString = Environment.GetEnvironmentVariable("StorageConnection");
            var client = new BlobClient(connectionString, "test1", blobName);
            var metaData = new Dictionary<string, string>();
            metaData["PhotoSource"] = "WebUrl";
            client.Upload(BinaryData.FromBytes(data));
            client.SetMetadata(metaData);
        }

        private static T GetEventData<T>(EventGridEvent @event) where T : class
        {
            var jObject = @event.Data as JObject;
            return jObject?.ToObject<T>();
        }
    }
}
