using MatterSearchApi.Interfaces;

namespace MatterSearchApi.Services;

public class PineconeService(IConfiguration config, IEmbeddingService embeddingService)
    : MatterSearchApi.Interfaces.IPineconeService
{
    private string PineconeApiKey { get; } = new(config["OpenAI:ApiKey"]); // expose client for testing
    public async Task<List<MatterSearchApi.Models.ChunkDto>> CreateChunksFromPdf(string filePath)
    {
        var chunks = new List<MatterSearchApi.Models.ChunkDto>();
        var pdf = UglyToad.PdfPig.PdfDocument.Open(filePath);

        int chunkSize = 800;
        int overlap = 100;

        var allSnippets = new List<(string snippet, int page)>(); // keep page mapping

        // 1. Extract all snippets
        foreach (var page in pdf.GetPages())
        {
            string text = page.Text ?? string.Empty;

            int start = 0;
            while (start < text.Length)
            {
                int length = Math.Min(chunkSize, text.Length - start);
                string snippet = text.Substring(start, length);

                allSnippets.Add((snippet, page.Number));

                start += (chunkSize - overlap);
            }
        }

        // 2. Generate ALL embeddings in one batch
        List<string> snippetTexts = allSnippets.Select(x => x.snippet).ToList();
        float[][] embeddings = await embeddingService.GenerateEmbeddingsAsync(snippetTexts);

        // 3. Build final chunks list
        for (int i = 0; i < allSnippets.Count; i++)
        {
            chunks.Add(new MatterSearchApi.Models.ChunkDto(
                Id: Guid.NewGuid().ToString(),
                FileName: Path.GetFileName(filePath),
                Page: allSnippets[i].page,
                Snippet: allSnippets[i].snippet,
                Embedding: embeddings[i]
            ));
        }

        return chunks;
    }
    
    public async Task StoreEmbeddingsInPinecone(List<MatterSearchApi.Models.ChunkDto> chunks)
    {
        var apiKey = "pcsk_4BurqC_CMqStCi5au5adhN3g7wfDHF4GyyAPNC1gnQE1TJtZesv1csborZpHdHy5fAoG44";
        var indexName = "mattersearch";

        var pineconeClient = new Pinecone.PineconeClient(apiKey);
        var index = pineconeClient.Index(indexName);

        // Build vector list ONCE
        List<Pinecone.Vector> vectors = chunks.Select(chunk => new Pinecone.Vector
        {
            Id = chunk.Id,
            Values = chunk.Embedding,
            Metadata = new Pinecone.Metadata
            {
                ["FileName"] = chunk.FileName,
                ["Page"] = chunk.Page,
                ["Snippet"] = chunk.Snippet
            }
        }).ToList();

        // Batch upload
        const int batchSize = 100;

        for (int i = 0; i < vectors.Count; i += batchSize)
        {
            var batch = vectors.Skip(i).Take(batchSize).ToList();

            await index.UpsertAsync(new Pinecone.UpsertRequest
            {
                Vectors = batch
            });

            Console.WriteLine($"Uploaded {i + batch.Count}/{vectors.Count}");
        }
    }
}


