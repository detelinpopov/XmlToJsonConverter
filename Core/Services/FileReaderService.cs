using Core.Constants;
using Domain.Enums;
using Domain.Models;
using Interfaces.Services;

namespace Core.Services;

public class FileReaderService : IFileReaderService
{
    public UploadedFileResult GetUploadedFiles(GetUploadedFilesModel getFilesModel)
    {
        var result = new UploadedFileResult();
        
        if (!Directory.Exists(getFilesModel.DirectoryUrl))
        {
            result.Success = false;
            result.Errors = new List<ErrorModel>
            {
                new() { ErrorMessage = UserMessages.GetFilesDirectoryNotFound, ErrorType = ErrorType.RetrieveFiles }
            };
            return result;
        }

        string[] files = Directory.GetFiles(getFilesModel.DirectoryUrl);
        foreach (string file in files)
        {
            result.FileNames.Add(Path.GetFileName(file));
        }

        return result;
    }
}