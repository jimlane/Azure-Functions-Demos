using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Contoso.Chat
{
    public static class CreateMessage
    {
        [FunctionName("createMessage")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "createmessage")] HttpRequest req,
            [CosmosDB(
                databaseName: "chats",
                collectionName: "messages",
                ConnectionStringSetting = "CosmosDBConnection")] out Chats chats,
            [SignalR(HubName = "chat")]IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            
            //log invocation
            log.LogInformation("SignalRChat.createMessageCS API processed a request.");

            //setup and send CosmosDB entry
            string newGuid = Guid.NewGuid().ToString();
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            chats = new Chats { sender = data.sender, text = data.text, id = newGuid};

            //broadcast message to SignalR hub
            signalRMessages.AddAsync(
                new SignalRMessage {
                    Target = "newMessage",
                    Arguments = new [] { chats }
                }
            );

            //return results
            string responseMessage = "New chat record recorded: " + newGuid;
            return new OkObjectResult(responseMessage);
        }

        [FunctionName("signalRInfo")]
        public static SignalRConnectionInfo signalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "signalrinfo")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "chat")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            //log invocation
            log.LogInformation("SignalRChat.getSignalRInfoCS API processed a request.");

            //return signalRInfo from context
            return connectionInfo;
        }
    }
}
