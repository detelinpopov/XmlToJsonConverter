using Core.Constants;
using Domain.Enums;
using Domain.Models;
using Interfaces.Services;

namespace Core.Services;

public class FileUploadService : IFileUploadService
{
    public async Task<UploadConvertedXmlFileResult> UploadFileAsync(UploadFileModel fileModel)
    {
        try
        {
            var result = new UploadConvertedXmlFileResult();

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), fileModel.UploadTo);
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            var exactPath = Path.Combine(Directory.GetCurrentDirectory(), fileModel.UploadTo, $"{fileModel.FileName}");
            await File.WriteAllTextAsync(exactPath, fileModel.Content);

            return result;
        }
        catch (Exception)
        {
            var result = new UploadConvertedXmlFileResult
            {
                Success = false,
                Errors = new List<ErrorModel>
                {
                    new(){ ErrorMessage = UserMessages.SaveJsonFileError, ErrorType = ErrorType.UploadFile}
                }
            };

            return result;
        }
    }
}