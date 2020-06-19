using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Contoso.Functions
{
    public static class ConvertFilesToByte
    {
        [FunctionName("ConvertFilesToByte")]
        public static void Run(
            [BlobTrigger("files-to-convert/{name}", Connection = "AzureWebJobsStorage")]
             Stream inBlob, 
             string name, 
            [Blob("output-blob/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outBlob,
             ILogger log)
        {
            log.LogInformation($"ConvertFilesToByte function Processed blob\n Name:{name} \n Size: {inBlob.Length} Bytes");

            using (MemoryStream ms = new System.IO.MemoryStream())
            {
                inBlob.CopyTo(ms);
                var byteArray = ms.ToArray();
                outBlob.Write(byteArray, 0, byteArray.Length);
            }
            
        }
    }
}
