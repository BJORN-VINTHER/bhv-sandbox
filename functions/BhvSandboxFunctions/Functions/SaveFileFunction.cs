// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BhvSandboxFunctions
{
    public static class SaveFileFunction
    {
        [FunctionName("SaveFileFunction")]
        public static void Run([EventGridTrigger] EventGridEvent @event, ILogger log)
        {
            log.LogInformation($"Received event - ID:{@event.Id} Subject:{@event.Subject} Topic:{@event.Topic} Time:{@event.EventTime}");
            var data = GetEventData<QueueMessageDto>(@event);
            log.LogInformation($"Payload - Name:{data.Name} FileUrl:{data.FileUrl}");
        }

        private static T GetEventData<T>(EventGridEvent @event) where T : class
        {
            var jObject = @event.Data as JObject;
            return jObject?.ToObject<T>();
        }
    }
}
