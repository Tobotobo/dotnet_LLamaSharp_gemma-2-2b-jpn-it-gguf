using LLama;
using LLama.Common;
using LLama.Sampling;

public static class Example2
{
    public static async Task Run(InteractiveExecutor executor) {
        var chatHistory = new ChatHistory();
        chatHistory.AddMessage(AuthorRole.System, @"
        あなたは、役立つ変換ツールです。これからユーザーが送ってくる文章からメールアドレスと名前を抽出し、以下の JSON 形式で回答してください。
        {
            mail: ""XXXXX"",
            name: ""XXXXX""
        }
        ");

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

        var userInput = @"
        ────────────────────────
        Example 株式会社
        システム部
        山田 太郎
        〒000-0000 東京都〇〇〇〇
        TEL：03-0000-0000 FAX： 03-0000-0000
        Email：taro.yamada＠example.com
        URL: https://example.com/
        ────────────────────────
        ";

        await foreach ( // Generate the response streamingly.
            var text
            in session.ChatAsync(
                new ChatHistory.Message(AuthorRole.User, userInput),
                inferenceParams))
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
        }
    }
}