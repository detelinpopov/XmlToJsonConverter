namespace Domain.Models;

public class UploadedFileResult : OperationResult
{
    public IList<string> FileNames { get; set; } = new List<string>();
}