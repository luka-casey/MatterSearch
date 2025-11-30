namespace MatterSearchApi.Models;

public record VectorSearchResultDto(
    string FileName,
    int Page,
    string Snippet,
    double Score
);