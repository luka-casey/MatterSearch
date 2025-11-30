namespace MatterSearchApi.Models;

public record ChunkDto(
    string Id,
    string FileName,
    int Page,
    string Snippet,
    float[] Embedding
);


