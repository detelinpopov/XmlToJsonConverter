using Core.Constants;
using Core.Validators;
using Domain.Models;
using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace XMLFileConverter.Controllers;

[Route("api/FileUpload")]
[ApiController]
public class FileUploadController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IFileConvertService _fileConvertService;

    private readonly IFileUploadService _fileUploadService;

    public FileUploadController(IConfiguration configuration, IFileConvertService fileConvertService, IFileUploadService fileUploadService)
    {
        _configuration = configuration;
        _fileConvertService = fileConvertService;
        _fileUploadService = fileUploadService;
    }

    [HttpPost]
    [Route("UploadFile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (!FileValidator.IsValidXmlFile(file))
        {
            return BadRequest("Invalid XML file.");
        }

        var xmlToJsonConversionResult = _fileConvertService.ConvertXmlToJson(file);
        if (!xmlToJsonConversionResult.Success)
        {
            return BadRequest(xmlToJsonConversionResult.GetErrorsAsString());
        }

        var uploadTo = _configuration.GetValue<string>("ConvertFileSettings:UploadToPath");
        var saveFileModel = new UploadFileModel
        {
            Content = xmlToJsonConversionResult.JsonContent,
            FileName = file.FileName.Replace(".xml", ".json"),
            UploadTo = uploadTo ?? "Uploads\\ConvertedJsonFiles"
        };

        var uploadFileResult = await _fileUploadService.UploadFileAsync(saveFileModel);
        if (!uploadFileResult.Success)
        {
           return StatusCode(500, uploadFileResult.GetErrorsAsString());
        }

        return Ok(UserMessages.FileConvertedAndUploaded);
    }
}

