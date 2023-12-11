using Core.Constants;
using Core.Validators;
using Domain.Enums;
using Domain.Models;
using Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Services;

public class FileConvertService : IFileConvertService
{
    public XmlToJsonConversionResult ConvertXmlToJson(IFormFile xmlFile)
    {
        var conversionResult = new XmlToJsonConversionResult();

        if (!FileValidator.IsValidXmlFile(xmlFile))
        {
            SetInvalidXmlFileResponse(conversionResult);
            return conversionResult;
        }

        var document = new ConvertedXmlDocument();
        if (!document.TryParseXml(xmlFile.OpenReadStream()))
        {
            SetInvalidXmlFileResponse(conversionResult);
            return conversionResult;
        }

        var jsonContent = JsonConvert.SerializeXmlNode(document, Formatting.Indented);
        conversionResult.JsonContent = jsonContent;
        return conversionResult;
    }

    private static void SetInvalidXmlFileResponse(OperationResult result)
    {
        result.Success = false;
        result.Errors = new List<ErrorModel>
        {
            new() { ErrorMessage = UserMessages.InvalidXmlFileMessage, ErrorType = ErrorType.ConvertFile }
        };
    }
}