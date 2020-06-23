# Azure Functions Demos - Hello Orchestration

## Overview

This project demonstrates implementing a fan-out/fan-in application pattern using the Azure Durable Functions Framework. It is a variation of one of the Azure Quickstart walkthrougs available [here](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-create-first-csharp?pivots=code-editor-vscode)

<properties
    pageTitle="Azure Functions Hello Orchestration"
    description="Demo of Azure Durable Functions framework"
    services="azure-functions,durable-functions,c-sharp"
    documentationCenter="Azure"
/>

<tags
    ms.service="azure-functions"
    ms.devlang="C#"/>

![Azure Functions Orchestration Sample](../images/HelloOrch1.png)

#### Azure Functions:
- Version: 2.0
- Runtime: DotNet Core 2.1

#### Extensions:
- Microsoft.Azure.WebJobs.Extensions.Http
- Microsoft.Azure.WebJobs.Extensions.DurableTask
- Microsoft.NET.Sdk.Functions

#### Starter Function
Input Bindings: HTTP <br />
Output Bindings: IDurableOrchestrationClient

#### Orchestrator Function
Input Bindings: IDurableOrchestrationContext

#### Activity Function
Input Bindings: ActivityTrigger

### Overview

The application utilizes a starter function with an HTTP trigger that accepts either a GET or POST action. The starter function then asyncronously instantiates the orchestrator function, logs the startup activity and returns a CheckStatusResponse endpoint. This return manifests in the browser as different endpoints that can be queried afterwards where the Azure Functions Durable Framework will return status and information about this durable function instance:

![Durable Framework Status Endpoint](../images/HelloOrch2.png)

You can use the shown **StatusQueryGetURI** to return the overall status and results from the activity function runs:

![Durable Framework Status Results](../images/HelloOrch3.png)

When the orchestrator function starts, it creates a System.Collections.Generic.List object of type System.String which will be used to contain the outputs from the activity functions. It then creates three activity functions asyncronously via the CallActivityAsync method of the DurableOrchestrationContext object which is passed in to the orchestrator function via the OrchestrationTrigger input binding. The instantiation of the activity functions is performed within the Add method of the List object, so that the output of each activity function will be automatically added to the list when each activity function completes. After the activity functions are called, the orchestrator function executes the last line of code which is to return the output list object. However, since this is the initial invocation of the orchestrator function, no output is available, so at this point an empty list object is returned, and the orchestrator function terminates. 

The ActivityTrigger input binding of the activity function flags its function type to the Durable Functions Framework. The framework keeps track of all activity instances for a particular orchestrator in order to signal back when all activities have either completed or timed out. These activity functions simply log the string passed to them and use that string in the returned message. 

When all of the activity functions have completed, the framework will call the orchestrator function again. The orchestrator function begins a replay, which means it will execute all of its code from the beginning. **This is an important concept** since you will need to code your orchestrator function logic to allow for proper execution during replays. With this in mind, the orchestrator creates a new list object during the replay, then goes about calling the activity functions again. However, during this invocation the durable framework sees that the indicated activity functions have already been called before and have all completed. The framework then retrieves the output from the storage medium employed, in our scenario Azure Blob Storage, and returns the output to the orchestrator function. The last line of the orchestrator function returns the list object, and the orchestrator function terminates. This same process occurs each time the StatusQueryGetURI is called.
