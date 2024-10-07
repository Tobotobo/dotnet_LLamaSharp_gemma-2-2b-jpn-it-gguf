// https://scisharp.github.io/LLamaSharp/0.16.0/QuickStart/
using LLama.Common;
using LLama;

string modelPath = @"./models/gemma-2-2B-jpn-it-IQ4_XS.gguf"; // change it to your own model path.
// string modelPath = @"./models/gemma-2-2B-jpn-it-Q8_0.gguf";
// string modelPath = @"./models/gemma-2-2B-jpn-it-BF16.gguf"; 


var parameters = new ModelParams(modelPath)
{
    ContextSize = 1024, // The longest length of chat as memory.
    // GpuLayerCount = 5 // How many layers to offload to GPU. Please adjust it according to your GPU memory.
};
using var model = LLamaWeights.LoadFromFile(parameters);
using var context = model.CreateContext(parameters);
var executor = new InteractiveExecutor(context);

await Example1.Run(executor);
// await Example2.Run(executor);