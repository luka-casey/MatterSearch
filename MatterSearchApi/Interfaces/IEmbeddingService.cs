namespace MatterSearchApi.Interfaces;

public interface IEmbeddingService
{
    public Task<float[][]> GenerateEmbeddingsAsync(List<string> texts);
}