/*
using MatterSearchApi.Interfaces;
using MatterSearchApi.Models;
using OpenAI.Chat;
using OpenAI.Embeddings;

public class RagService : IRagService
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStoreService _vectorStoreService;
    private readonly ChatClient _chatClient;

    public RagService(
        IEmbeddingService embeddingService,
        IVectorStoreService vectorStoreService,
        IConfiguration config)
    {
        _embeddingService = embeddingService;
        _vectorStoreService = vectorStoreService;

        _chatClient = new ChatClient(
            model: "gpt-4.1-mini",
            apiKey: config["OpenAI:ApiKey"]
        );
    }

    // --------------------------------------------------------------------
    // 1. CHAT ANSWER GENERATION (LLM)
    // --------------------------------------------------------------------
    public async Task<string> AskQuestionAsync(string question)
    {
        // 1. Embed the question
        var embedding = await _embeddingService.GenerateEmbeddingAsync(question);

        // 2. Retrieve the top chunks
        var results = await _vectorStoreService.SearchAsync(question);

        var context = string.Join("\n\n", results.Select(r =>
            $"[Source: {r.FileName}, Page {r.Page}]\n{r.Snippet}"
        ));

        // 3. Ask OpenAI using the RAG context
        var answer = await _chatClient.CompleteAsync(
            messages: new[]
            {
                new UserChatMessage($@"
                    Answer the following legal question using ONLY the provided context.

                    Question: {question}

                    Context:
                    {context}

                    If the context does not contain enough information, say:
                    'I do not have enough information in the indexed documents to answer this.'")
            }
        );

        return answer.Value;
    }

    // --------------------------------------------------------------------
    // 2. SOURCE RETRIEVAL (Vector DB only)
    // --------------------------------------------------------------------
    public async Task<List<SearchResultDto>> GetTopSources(string question)
    {
        var results = await _vectorStoreService.SearchAsync(question);
        return results;
    }

    // --------------------------------------------------------------------
    // 3. DOCUMENT CHUNKING
    // --------------------------------------------------------------------
    public Task<List<ChunkDto>> ChunkDocumentAsync(string documentId, string text)
    {
        const int chunkSize = 500;
        const int overlap = 100;

        var chunks = new List<ChunkDto>();
        int start = 0;
        int index = 0;

        while (start < text.Length)
        {
            int length = Math.Min(chunkSize, text.Length - start);
            string chunkText = text.Substring(start, length);

            chunks.Add(new ChunkDto(
                ChunkId: $"{documentId}-chunk-{index}",
                Text: chunkText
            ));

            start += chunkSize - overlap;
            index++;
        }

        return Task.FromResult(chunks);
    }

    // --------------------------------------------------------------------
    // 4. FULL DOCUMENT INDEXING (Chunk + embed + store)
    // --------------------------------------------------------------------
    public async Task IndexDocumentAsync(string documentId, string text)
    {
        var chunks = await ChunkDocumentAsync(documentId, text);

        var embedData = new List<(string ChunkId, string Text, float[] Embedding)>();

        foreach (var c in chunks)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(c.Text);
            embedData.Add((c.ChunkId, c.Text, embedding));
        }

        await _vectorStoreService.IndexDocumentAsync(documentId, embedData);
    }

    // --------------------------------------------------------------------
    // 5. RAW TEXT SEARCH (vector only)
    // --------------------------------------------------------------------
    public async Task<List<SearchResultDto>> SearchAsync(string query)
    {
        return await _vectorStoreService.SearchAsync(query);
    }
}
*/
