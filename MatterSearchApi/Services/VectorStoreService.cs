using MatterSearchApi.Interfaces;
using MatterSearchApi.Models;

namespace MatterSearchApi.Services;

public class VectorStoreService : IVectorStoreService
{
    private readonly HttpClient _httpClient;

    public VectorStoreService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    /*public async Task<List<VectorSearchResultDto>> SearchByEmbeddingAsync(float[] queryEmbedding)
    {
        // Imagine we have precomputed document embeddings stored in memory
        var documents = _documentStore; // List<(string FileName, string Snippet, float[] Embedding)>
    
        var results = new List<MatterSearchApi.Models.VectorSearchResultDto>();

        foreach (var doc in documents)
        {
            float similarity = CosineSimilarity(queryEmbedding, doc.Embedding);
            results.Add(new MatterSearchApi.Models.VectorSearchResultDto(
                FileName: doc.FileName,
                Page: doc.Page,
                Snippet: doc.Snippet,
                Score: similarity
            ));
        }

        // Return top 5 most similar results
        return results.OrderByDescending(r => r.Score).Take(5).ToList();
    }

    // Cosine similarity helper
    private float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0f;
        float magA = 0f;
        float magB = 0f;
    
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / ((float)Math.Sqrt(magA) * (float)Math.Sqrt(magB));
    }*/
}





























