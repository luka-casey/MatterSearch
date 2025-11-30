namespace MatterSearchApi.Models;

public record ChatResponseDto(
    string Answer,
    IEnumerable<SearchResultDto> Sources
);
