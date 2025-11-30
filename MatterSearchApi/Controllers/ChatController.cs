using MatterSearchApi.Interfaces;
using MatterSearchApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MatterSearchApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IRagService _ragService;

    public ChatController(IRagService ragService)
    {
        _ragService = ragService;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<ChatResponseDto>> AskQuestion(ChatRequestDto request)
    {
        var answer = await _ragService.AskQuestionAsync(request.Question);
        var topSources = await _ragService.GetTopSources(request.Question);

        var sources = topSources
            .Select(r => new SearchResultDto(r.FileName, r.Page, r.Snippet, r.Score));

        return Ok(new ChatResponseDto(answer, sources));
    }
}
