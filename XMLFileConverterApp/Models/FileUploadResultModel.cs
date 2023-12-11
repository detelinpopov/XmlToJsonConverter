using Microsoft.AspNetCore.Mvc;

namespace XMLFileConverterApp.Models;

public class FileUploadResultModel
{
    public bool Success { get; set; }

    public string ResponseMessage { get; set; }

    public int StatusCode { get; set; }
}