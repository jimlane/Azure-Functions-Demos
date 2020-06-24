using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace ApprovalOrchestration
{
    public static class Approval
    {
        [FunctionName("ApprovalFunction")]
        public static string ApprovalFunction([ActivityTrigger] string action, ILogger log)
        {
            log.LogInformation($"Project proposal has been - {action}");
            return $"Your project design proposal has been - {action}";
        }
    }
}