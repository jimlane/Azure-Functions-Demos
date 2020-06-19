using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace ApprovalOrchestration
{
    public static class Approval
    {
        [FunctionName("Approval")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Project proposal has been - {name}");
            return $"Your project design proposal has been - {name}";
        }
    }
}