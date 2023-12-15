using Domain.Models;

namespace Interfaces.Services;

public interface IFileReaderService
{
    public UploadedFileResult GetUploadedFiles(GetUploadedFilesModel getFilesModel);
}