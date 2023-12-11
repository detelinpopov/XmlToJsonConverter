using Core.Validators;
using Microsoft.AspNetCore.Mvc;
using XMLFileConverterApp.Helpers;
using XMLFileConverterApp.Models;

namespace XMLFileConverterApp.Controllers;

public class FileUploadController : Controller
{
    private readonly HttpClient _httpClient = new();

    public IActionResult FileUpload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile fileUpload)
    {
        if (!FileValidator.IsValidXmlFile(fileUpload))
        {
            var invalidXmlResultModel = new FileUploadResultModel { ResponseMessage = "Invalid XML file." };
            return PartialView("~/Views/FileUpload/FileUploadFailed.cshtml", invalidXmlResultModel);
        }

        const string uploadFileApiUrl = "https://localhost:7127/api/FileUpload/UploadFile";
        var fileUploadResult = await HttpRequestHelper.CallApiToUploadFileAsync(fileUpload, _httpClient, uploadFileApiUrl);

        if (fileUploadResult.Success)
        {
            return PartialView("~/Views/FileUpload/FileUploadedSuccessfully.cshtml");
        }

        var resultModel = new FileUploadResultModel { ResponseMessage = fileUploadResult.ResponseMessage };
        return PartialView("~/Views/FileUpload/FileUploadFailed.cshtml", resultModel);
    }
}


