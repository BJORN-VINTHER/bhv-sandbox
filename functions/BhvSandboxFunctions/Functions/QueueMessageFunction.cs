using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;

namespace BhvSandboxFunctions
{
    public static class QueueMessageFunction
    {
        [FunctionName("QueueMessageFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] WebInputDto input,
            [EventGrid(TopicEndpointUri = "EventGridEndpoint", TopicKeySetting = "EventGridKey")] IAsyncCollector<EventGridEvent> eventCollector,
            ILogger log)
        {
            log.LogInformation("QueueMessageFunction processed a request.");

            if (input.Images.Count > 0)
            {
                var message = new QueueMessageDto
                {
                    Name = "Test 1",
                    FileUrl = "test url"
                };
                var e = new EventGridEvent
                {
                    Id = "my id",
                    EventType = "my type",
                    Subject = "my subject",
                    Data = message,
                    DataVersion = "1.0"
                };

                await eventCollector.AddAsync(e);
                return new OkObjectResult($"Added {input.Images.Count} images to queue");
            }
            else
            {
                return new OkObjectResult("Skipped function call since there are no image urls");
            }
        }
    }
}
