// https://scisharp.github.io/LLamaSharp/0.16.0/QuickStart/
using LLama;
using LLama.Common;
using LLama.Sampling;

public static class Example1
{
    public static async Task Run(InteractiveExecutor executor) {
        var chatHistory = new ChatHistory();
        chatHistory.AddMessage(AuthorRole.System, "あなたは、役立つアシスタントです。");

        ChatSession session = new(executor, chatHistory);

        InferenceParams inferenceParams = new InferenceParams()
        {
            SamplingPipeline = new DefaultSamplingPipeline()
            {
                Temperature = 0.5f,
                RepeatPenalty = 1.0f
            },
            // MaxTokens = 256, // No more than 256 tokens should appear in answer. Remove it if antiprompt is enough for control.
            MaxTokens = 1024,
            AntiPrompts = new List<string> { "User:" } // Stop generation once antiprompts appear.
        };

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("The chat session has started.\nUser: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine() ?? "";

        while (userInput != "exit")
        {
            await foreach ( // Generate the response streamingly.
                var text
                in session.ChatAsync(
                    new ChatHistory.Message(AuthorRole.User, userInput),
                    inferenceParams))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(text);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            userInput = Console.ReadLine() ?? "";
        }
    }
}