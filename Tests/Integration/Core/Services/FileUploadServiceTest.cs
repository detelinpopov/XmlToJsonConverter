using Core.Services;
using Domain.Models;
using Xunit;

namespace Tests.Integration.Core.Services;

public class FileUploadServiceTest
{
    [Fact]
    public async Task UploadFileAsync_UploadsFileSuccessfully()
    {
        // Arrange
        var service = new FileUploadService();
        var testFile = FileTestHelper.CreateTestFile();

        var uploadFileModel = new UploadFileModel
        {
            Content = testFile.OpenReadStream().ToString(),
            FileName = "File_Uploaded_By_Integration_Test.json",
            UploadTo = "IntegrationTests//UploadedFiles"
        };

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadFileModel.UploadTo, $"{uploadFileModel.FileName}");
        FileTestHelper.DeleteFilesFrom(Path.Combine(Directory.GetCurrentDirectory(), uploadFileModel.UploadTo));

        // Make sure that the file does not exist before the upload.
        Assert.False(File.Exists(filePath));

        // Act
        await service.UploadFileAsync(uploadFileModel);

        // Assert
        Assert.True(File.Exists(filePath));
    }
}