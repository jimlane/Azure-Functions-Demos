using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace ApprovalOrchestration
{
    public static class OrchFunction
    {
        [FunctionName("OrchFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Call our durable activity function and simulate both states
            outputs.Add(await context.CallActivityAsync<string>("Approval", "Approved"));
            outputs.Add(await context.CallActivityAsync<string>("Approval", "Rejected"));

            // returns the outputs gathered from the activity function calls
            return outputs;
        }

    }
}