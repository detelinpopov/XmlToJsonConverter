namespace Domain.Models;

public abstract class OperationResult
{
    /// <summary>
    ///     Indicates if the operation completed successfully.
    /// </summary>
    public bool Success { get; set; } = true;

    public IEnumerable<ErrorModel> Errors { get; set; } = Enumerable.Empty<ErrorModel>();

    public string GetErrorsAsString()
    {
        return string.Join(",", Errors.Select(e => e.ErrorMessage).ToArray());
    }
}