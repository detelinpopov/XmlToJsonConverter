using Domain.Models;
using Microsoft.AspNetCore.Http;
namespace Interfaces.Services
{
    public interface IFileConvertService
    {
        public XmlToJsonConversionResult ConvertXmlToJson(IFormFile xmlFile);
    }
}
