using System.Net;

namespace XMLFilesConverter.Models;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);