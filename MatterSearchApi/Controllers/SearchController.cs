using MatterSearchApi.Interfaces;
using MatterSearchApi.Models;
using MatterSearchApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatterSearchApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController(
    IEmbeddingService embeddingService,
    //IVectorStoreService vectorStoreService,
    IPineconeService pineconeService)
    : ControllerBase
{
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<SearchResultDto>>> Search(string query)
    // {
    //     float[] embedding = await embeddingService.GenerateEmbeddingAsync(query);
    //     
    //     // 2️⃣ Use the embedding to search the vector store
    //     List<VectorSearchResultDto> results = await vectorStoreService.SearchByEmbeddingAsync(embedding);
    //
    //     // 3️⃣ Map results to your API DTO
    //     var response = results.Select(r =>
    //         new SearchResultDto(r.FileName, r.Page, r.Snippet, r.Score)
    //     );
    //
    //     return Ok(response);
    // }
    
    [HttpPost("UploadPdfChunksToPinecone")]
    public async Task<ActionResult> UploadPdfChunksToPinecone(string filePath)
    {
        List<MatterSearchApi.Models.ChunkDto> chunks = await pineconeService.CreateChunksFromPdf(filePath);
        
        await pineconeService.StoreEmbeddingsInPinecone(chunks);
        
        return Ok();
    }
}