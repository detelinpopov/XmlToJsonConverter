using Microsoft.AspNetCore.Http;

namespace Core.Validators;

public static class FileValidator
{
    public static bool IsValidXmlFile(IFormFile file)
    {
        var isValid = file is { Length: > 0, ContentType: "text/xml" };
        return isValid;
    }
}