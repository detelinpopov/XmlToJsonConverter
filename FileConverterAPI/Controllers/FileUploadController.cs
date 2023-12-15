using Core.Constants;
using Core.Validators;
using Domain.Models;
using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileConverterAPI.Controllers;

[Route("api/FileUpload")]
[ApiController]
public class FileUploadController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IFileConvertService _fileConvertService;

    private readonly IFileUploadService _fileUploadService;

    private readonly IFileReaderService _fileReaderService;

    public FileUploadController(IConfiguration configuration, IFileConvertService fileConvertService, IFileUploadService fileUploadService, IFileReaderService fileReaderService)
    {
        _configuration = configuration;
        _fileConvertService = fileConvertService;
        _fileUploadService = fileUploadService;
        _fileReaderService = fileReaderService;
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

        var saveFileModel = new UploadFileModel
        {
            Content = xmlToJsonConversionResult.JsonContent,
            FileName = file.FileName.Replace(".xml", ".json"),
            UploadTo = GetUploadedFilesPath()
        };

        var uploadFileResult = await _fileUploadService.UploadFileAsync(saveFileModel);
        if (!uploadFileResult.Success)
        {
           return StatusCode(500, uploadFileResult.GetErrorsAsString());
        }

        return Ok(UserMessages.FileConvertedAndUploaded);
    }


    [HttpPost]
    [Route("GetUploadedFiles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult GetUploadedFiles()
    {
        var getUploadedFilesModel = new GetUploadedFilesModel { DirectoryUrl = GetUploadedFilesPath() };

        var getUploadedFilesResult = _fileReaderService.GetUploadedFiles(getUploadedFilesModel);
        if (!getUploadedFilesResult.Success)
        {
            return StatusCode(500, getUploadedFilesResult.GetErrorsAsString());
        }

        return Ok(getUploadedFilesResult);
    }

    private string GetUploadedFilesPath()
    {
        var path = _configuration.GetValue<string>("ConvertFileSettings:UploadToPath");
        if (string.IsNullOrWhiteSpace(path))
        {
            path = "Uploads\\ConvertedJsonFiles";
        }

        return path;
    }
}

