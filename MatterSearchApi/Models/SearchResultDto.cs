namespace MatterSearchApi.Models;

public record SearchResultDto(
    string FileName,
    int Page,
    string Snippet,
    double Score
);
