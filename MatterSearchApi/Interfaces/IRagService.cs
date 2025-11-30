using MatterSearchApi.Models;

namespace MatterSearchApi.Interfaces;

public interface IRagService
{
    Task<string> AskQuestionAsync(string question);
    Task<List<SearchResultDto>> GetTopSources(string question);

    Task<List<ChunkDto>> ChunkDocumentAsync(string documentId, string text);
    Task IndexDocumentAsync(string documentId, string text);

    Task<List<SearchResultDto>> SearchAsync(string query);
}


