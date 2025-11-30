namespace MatterSearchApi.Models;

public record UploadResponseDto(
    string FileName,
    bool Success,
    string Message
);
