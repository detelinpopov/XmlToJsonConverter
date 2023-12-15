using Core.Services;
using Domain.Models;
using Xunit;

namespace Tests.Integration.Core.Services;

public class FIleReaderServiceTest
{
    [Fact]
    public void GetUploadedFiles_ReturnsUploadedFiles()
    {
        // Arrange
        var service = new FileReaderService();
        var model = new GetUploadedFilesModel
        {
            DirectoryUrl = @"C:\\Users\\Detelin_Popov\\Desktop\\XmlToJsonConverter\\Tests\\Integration\\TestUploadedFiles"
        };

        // Act 
        var result = service.GetUploadedFiles(model);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotEmpty(result.FileNames);
    }

    [Fact]
    public void GetUploadedFiles_ReturnsErrorModel_WhenDirectoryNotFound()
    {
        // Arrange
        var service = new FileReaderService();
        var model = new GetUploadedFilesModel
        {
            DirectoryUrl = "Invalid Directory"
        };

        // Act 
        var result = service.GetUploadedFiles(model);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.NotEmpty(result.Errors);
    }
}