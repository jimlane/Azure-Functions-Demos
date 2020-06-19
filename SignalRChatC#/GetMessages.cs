using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Contoso.Chat
{
    public class Chats {
        public string id {get; set; }
        public string sender {get; set; }
        public string text {get; set;}
    }
        public static class GetMessages
    {
        [FunctionName("GetMessages")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "messages")] HttpRequest req,
            [CosmosDB(
                databaseName: "chats",
                collectionName: "messages",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "SELECT * FROM c ORDER BY c._ts desc")] IEnumerable<Chats> chats,
                ILogger log)
        {
            //log this invocation
            log.LogInformation("SignalRChat.getMessagesCS processed a request.");

            //return collection from CosmosDB
            return new OkObjectResult(chats);
        }
    }
}
