using System.Text.Json;
using MatterSearchApi.Interfaces;
using OpenAI;
using OpenAI.Embeddings;

namespace MatterSearchApi.Services;

public class EmbeddingService(IConfiguration config) : IEmbeddingService
{
    private OpenAIClient Client { get; } = new(config["OpenAI:ApiKey"]); // expose client for testing

    public async Task<float[][]> GenerateEmbeddingsAsync(List<string> texts)
    {
        var embeddingClient = Client.GetEmbeddingClient("text-embedding-3-large");

        // Call batched embedding API
        var response = await embeddingClient.GenerateEmbeddingsAsync(texts);

        
        float[][] floats = response.Value.Select(x => x.ToFloats().ToArray()).ToArray();
        
        
        /*// Raw JSON response
        var raw = response.GetRawResponse();

        using var doc = JsonDocument.Parse(raw.Content);

        // The "data" array contains one embedding per input text
        var dataArray = doc.RootElement.GetProperty("data");

        float[][] result = new float[dataArray.GetArrayLength()][];

        for (int i = 0; i < dataArray.GetArrayLength(); i++)
        {
            string base64Embedding = dataArray[i]
                .GetProperty("embedding")
                .GetString();

            // Decode Base64 → bytes
            var bytes = Convert.FromBase64String(base64Embedding);

            // Convert bytes → float[]
            var floatArray = new float[bytes.Length / 4];
            for (int j = 0; j < floatArray.Length; j++)
            {
                floatArray[j] = BitConverter.ToSingle(bytes, j * 4);
            }

            result[i] = floatArray;
        }*/

        return floats;
    }

}