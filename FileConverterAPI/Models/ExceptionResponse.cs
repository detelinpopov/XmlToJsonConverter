using System.Net;

namespace FileConverterAPI.Models;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);