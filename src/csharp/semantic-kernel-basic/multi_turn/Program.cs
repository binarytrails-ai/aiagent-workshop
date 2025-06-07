// How to run multi-turn conversations with Semantic Kernel:
// This program demonstrates a multi-turn chat loop using Microsoft Semantic Kernel's chat completion service.
//
// To run: Build and execute this file. Enter your questions at the prompt; press Enter on an empty line to exit.
// Ensure your .env file is configured with the correct model and API keys.

using System.ClientModel;
using OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;
using DotNetEnv;
using System.IO;

// Step 1: Read environment variables
// Load variables from a .env file (if present) so they can be accessed by the application.
var envFilePath = Path.Combine(Environment.CurrentDirectory, "../..", ".env");
if (File.Exists(envFilePath))
{
    Env.Load(envFilePath);
    Console.WriteLine($"Loaded environment variables from {envFilePath}");
}
else
{
    Console.WriteLine($"No .env file found at {envFilePath}");
}

// Step 2: Instantiate the Kernel
// The Semantic Kernel orchestrates AI services and plugins. Here, we create and configure a Kernel instance.
OpenAIClient client = null;
if (Environment.GetEnvironmentVariable("USE_AZURE_OPENAI") == "true")
{
    // Configure Azure OpenAI client
    var azureEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
    var apiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
    client = new OpenAIClient(new ApiKeyCredential(apiKey), new OpenAIClientOptions { Endpoint = new Uri(azureEndpoint) });
}
else if (Environment.GetEnvironmentVariable("USE_OPENAI") == "true")
{
    // Configure OpenAI client
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    client = new OpenAIClient(new ApiKeyCredential(apiKey));
}
else if (Environment.GetEnvironmentVariable("USE_GITHUB") == "true")
{
    // Configure GitHub model client
    var uri = Environment.GetEnvironmentVariable("GITHUB_MODEL_ENDPOINT");
    var apiKey = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
    client = new OpenAIClient(new ApiKeyCredential(apiKey), new OpenAIClientOptions { Endpoint = new Uri(uri) });
}

var modelId = Environment.GetEnvironmentVariable("MODEL");
var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(modelId, client);

// Build the kernel and get the chat completion service
Kernel kernel = builder.Build();
var chat = kernel.GetRequiredService<IChatCompletionService>();

// Step 3: Multi-turn chat loop
// The chat history maintains the conversation context for multi-turn interactions.

// The OpenAIPromptExecutionSettings object allows you to control how the AI model generates its response.
// - MaxTokens: Limits the maximum number of tokens (words or word pieces) in the response. Here, it's set to 2000, allowing for long answers.
// - Temperature: Controls the randomness of the output. Lower values (like 0.2) make the output more focused and deterministic, while higher values make it more creative and random.
// - TopP: Implements nucleus sampling. The model considers only the most likely tokens whose cumulative probability is at least TopP (here, 0.5). Lower values make the output more focused.
var executionSettings = new OpenAIPromptExecutionSettings 
{
    MaxTokens = 2000,
    Temperature = 0.2,
    TopP = 0.5
};
var history = new ChatHistory();
history.AddSystemMessage("You are a useful chatbot. If you don't know an answer, say 'I don't know!'. Always reply in a funny way. Use emojis if possible.");

while (true)
{
    Console.Write("Q: ");
    var userQ = Console.ReadLine();
    if (string.IsNullOrEmpty(userQ))
    {
        break;
    }
    history.AddUserMessage(userQ);

    // Step 4: Call the Kernel and stream the response
    var sb = new StringBuilder();
    var result = chat.GetStreamingChatMessageContentsAsync(history,executionSettings);
    Console.Write("AI: ");
    await foreach (var item in result)
    {
        sb.Append(item);
        Console.Write(item.Content);
    }
    Console.WriteLine();

    history.AddAssistantMessage(sb.ToString());
}