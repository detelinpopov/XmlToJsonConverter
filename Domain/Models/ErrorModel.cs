using Domain.Enums;

namespace Domain.Models;

public class ErrorModel
{
    public required string ErrorMessage { get; set; }

    public ErrorType ErrorType { get; set; }
}