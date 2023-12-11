namespace Domain.Models;

public class UploadFileModel
{
    public required string? Content { get; set; }

    public required string UploadTo { get; set; }

    public required string FileName { get; set; }
}