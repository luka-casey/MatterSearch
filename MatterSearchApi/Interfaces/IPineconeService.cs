namespace MatterSearchApi.Interfaces;

public interface IPineconeService
{
    Task<List<MatterSearchApi.Models.ChunkDto>> CreateChunksFromPdf(string filePath);
    Task StoreEmbeddingsInPinecone(List<MatterSearchApi.Models.ChunkDto> chunks);
}