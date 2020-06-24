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
            // Call our approval activity function and simulate both approval states
            var approval = context.CallActivityAsync<string>("ApprovalFunction", "Approved");
            var rejection = context.CallActivityAsync<string>("ApprovalFunction", "Rejected");

            // Have orchestrator wait for both approval activities to complete
            await Task.WhenAll(approval, rejection);

            // Gather the outputs from both activities 
            var outputs = new List<string>();
            outputs.Add(approval.Result);
            outputs.Add(rejection.Result);

            // returns the outputs gathered from the activity function calls and persist them 
            // in the orchestrator history
            return outputs;
        }

    }
}