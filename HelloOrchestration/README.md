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

The application utilizes three separate JS based Azure Functions to provide application API's. The chat client ([index.html](./content/index.html)) is a JS SPA located in the [content](./content) folder. It utilizes the [ASP.Net Core SignalR JavaScript client](https://docs.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-3.1) for connectivity to the Azure SignalR service instance, and depends upon the following setting:
- window.apiBaseUrl: Azure SignalR Service endpoint

The GetMessages and CreateMessage functions bind to the hub of the Azure SignalR service instance through their respective output and input bindings. The client will thus invoke the Azure Function CreateMessage when a new chat message is sent to the SignalR hub and automatically receive any chat messages through the Azure Function GetMessages whenever other clients do the same. The CreateMessage function also has an output binding to Azure CosmosDB which will persist the message content as well as user name that the GetMessages function will retrieve through its input binding. These two functions depend upon the following settings for connectivity that you'll need to create in your local.settings.json file:
- CosmosDBConnectionString: connection string to specified CosmosDB backend e.g. *AccountEndpoint=https://<your_cosmosdb>.documents.azure.com;AccessKey=<your_key>;*
- AzureSignalRConnectionString: connection string to specified Azure SignalR instance e.g. *Endpoint=https://<your_signalr>.signalr.net;AccessKey=<your_key>;version=1.0;*
