using LLama.Common;
using LLama;
using LLama.Sampling;
using System.Text;

string modelPath = @"C:/models/gemma-2-2B-jpn-it-IQ4_XS.gguf";
var parameters = new ModelParams(modelPath)
{
    ContextSize = 1024
};
using var model = LLamaWeights.LoadFromFile(parameters);
using var context = model.CreateContext(parameters);
var ex = new InteractiveExecutor(context);
var session = new ChatSession(ex);

// 文字コードを設定（Windows環境向け）
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.InputEncoding = System.Text.Encoding.GetEncoding(932);

while (true)
{
    // プロンプトをコンソールから入力
    Console.Write("質問内容を入力してください: ");
    string userInput = Console.ReadLine()!;
    var message = new ChatHistory.Message(
        AuthorRole.User,
        $"[INST] <<SYS>>あなたは、役立つアシスタントです。<</SYS>{userInput}[/INST]"
    );

    // プロンプトを表示
    Console.WriteLine(message.Content);

    // プロンプトに対する応答を表示　※一文字一文字超ゆっくり表示される
    await foreach (var text in session.ChatAsync(
            message,
            new InferenceParams()
            {
                SamplingPipeline = new DefaultSamplingPipeline()
                {
                    Temperature = 0.5f,
                    RepeatPenalty = 1.0f
                },
                AntiPrompts = new List<string> { "User:" }
            }
        ))
    {
        Console.Write(text);
    }

    Console.WriteLine();
}











// // プロンプト
// var message = new ChatHistory.Message(
//     AuthorRole.User,
//     "[INST] <<SYS>>あなたは、役立つアシスタントです。<</SYS>C#言語の特徴を3つ箇条書きで、一つ20文字程度で回答してください。[/INST]"
// );

// // プロンプトを表示
// Console.WriteLine(message.Content);

// // プロンプトに対する応答を表示　※一文字一文字超ゆっくり表示される
// await foreach (var text in session.ChatAsync(
//         message,
//         new InferenceParams()
//         {
//             SamplingPipeline = new DefaultSamplingPipeline()
//             {
//                 Temperature = 0.5f,
//                 RepeatPenalty = 1.0f
//             },
//             AntiPrompts = new List<string> { "User:" }
//         }
//     ))
// {
//     Console.Write(text);
// }