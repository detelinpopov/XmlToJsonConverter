using Domain.Models;

namespace Interfaces.Services;

public interface IFileUploadService
{
    public Task<UploadConvertedXmlFileResult> UploadFileAsync(UploadFileModel fileModel);
}