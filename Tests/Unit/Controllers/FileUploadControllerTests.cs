﻿using Domain.Models;
using FileConverterAPI.Controllers;
using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests.Unit.Controllers;

public class FileUploadControllerTests
{
    private readonly Mock<IFileConvertService> _fileConvertServiceMock;

    private readonly Mock<IFileUploadService> _fileUploadServiceMock;

    private readonly Mock<IFileReaderService> _fileReaderServiceMock;

    private readonly FileUploadController _controller;

    public FileUploadControllerTests()
    {
        var configurationMock = new Mock<IConfiguration>();
        _fileConvertServiceMock = new Mock<IFileConvertService>();
        _fileUploadServiceMock = new Mock<IFileUploadService>();
        _fileReaderServiceMock = new Mock<IFileReaderService>();

        configurationMock.Setup(c => c.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

        _controller = new FileUploadController(configurationMock.Object, _fileConvertServiceMock.Object, _fileUploadServiceMock.Object, _fileReaderServiceMock.Object);
    }

    [Fact]
    public async Task UploadFile_ReturnsOkResult_WhenUploadIsSuccessful()
    {
        // Arrange
        var fileToUpload = FileTestHelper.CreateTestFile();

        var convertFileResult = new XmlToJsonConversionResult { Success = true, JsonContent = "test" };
        _fileConvertServiceMock.Setup(s => s.ConvertXmlToJson(fileToUpload)).Returns(convertFileResult);

        var uploadFileResult = new UploadConvertedXmlFileResult { Success = true };
        _fileUploadServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<UploadFileModel>())).ReturnsAsync(uploadFileResult);

        // Act
        var actionResult = await _controller.UploadFile(fileToUpload);

        // Assert
        Assert.IsAssignableFrom<OkObjectResult>(actionResult);
    }

    [Fact]
    public async Task UploadFile_CallsFileConvertService()
    {
        // Arrange
        var fileToUpload = FileTestHelper.CreateTestFile();

        var convertFileResult = new XmlToJsonConversionResult { Success = true, JsonContent = "test" };
        _fileConvertServiceMock.Setup(s => s.ConvertXmlToJson(fileToUpload)).Returns(convertFileResult);

        var uploadFileResult = new UploadConvertedXmlFileResult { Success = true };
        _fileUploadServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<UploadFileModel>())).ReturnsAsync(uploadFileResult);

        // Act
        await _controller.UploadFile(fileToUpload);

        // Assert
        _fileConvertServiceMock.Verify(s => s.ConvertXmlToJson(fileToUpload), Times.Once);
    }

    [Fact]
    public async Task UploadFile_CallsFileUploadService_WhenFileConvertedSuccessfully()
    {
        // Arrange
        var fileToUpload = FileTestHelper.CreateTestFile();

        var convertFileResult = new XmlToJsonConversionResult { Success = true, JsonContent = "test" };
        _fileConvertServiceMock.Setup(s => s.ConvertXmlToJson(fileToUpload)).Returns(convertFileResult);

        var uploadFileResult = new UploadConvertedXmlFileResult { Success = true };
        _fileUploadServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<UploadFileModel>())).ReturnsAsync(uploadFileResult);

        // Act
        await _controller.UploadFile(fileToUpload);

        // Assert
        _fileUploadServiceMock.Verify(s => s.UploadFileAsync(It.IsAny<UploadFileModel>()), Times.Once);
    }

    [Fact]
    public async Task UploadFile_DoesNotCallFileUploadService_WhenFileNotConverted()
    {
        // Arrange
        var fileToUpload = FileTestHelper.CreateTestFile();

        var convertFileResult = new XmlToJsonConversionResult { Success = false, JsonContent = "test" };
        _fileConvertServiceMock.Setup(s => s.ConvertXmlToJson(fileToUpload)).Returns(convertFileResult);

        var uploadFileResult = new UploadConvertedXmlFileResult { Success = true };
        _fileUploadServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<UploadFileModel>())).ReturnsAsync(uploadFileResult);

        // Act
        await _controller.UploadFile(fileToUpload);

        // Assert
        _fileUploadServiceMock.Verify(s => s.UploadFileAsync(It.IsAny<UploadFileModel>()), Times.Never);
    }

    [Fact]
    public async Task UploadFile_ReturnsBadRequestResult_IfUploadedFileIsNotValidXmlFile()
    {
        // Arrange
        var fileToUpload = FileTestHelper.CreateTestFile("invalidFormat");

        // Act
        var actionResult = await _controller.UploadFile(fileToUpload);

        // Assert
        Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    public async Task UploadFile_ReturnsBadRequestResult_IfFileNotUploadedSuccessfully()
    {
        // Arrange
        var fileToUpload = FileTestHelper.CreateTestFile();

        var convertFileResult = new XmlToJsonConversionResult { Success = false, JsonContent = "test" };
        _fileConvertServiceMock.Setup(s => s.ConvertXmlToJson(fileToUpload)).Returns(convertFileResult);

        // Act
        var actionResult = await _controller.UploadFile(fileToUpload);

        // Assert
        Assert.IsAssignableFrom<BadRequestObjectResult>(actionResult);
    }

    [Fact]
    public void GetUploadedFiles_CallsFileReaderService()
    {
        // Arrange
        _fileReaderServiceMock.Setup(s => s.GetUploadedFiles(It.IsAny<GetUploadedFilesModel>()))
            .Returns(new UploadedFileResult());

        // Act
        _controller.GetUploadedFiles();

        // Assert
        _fileReaderServiceMock.Verify(s => s.GetUploadedFiles(It.IsAny<GetUploadedFilesModel>()), Times.Once);
    }

    [Fact]
    public void GetUploadedFiles_ReturnsValidResponse()
    {
        // Arrange
        _fileReaderServiceMock.Setup(s => s.GetUploadedFiles(It.IsAny<GetUploadedFilesModel>()))
            .Returns(new UploadedFileResult {Success = true});

        // Act
        var response = _controller.GetUploadedFiles();

        // Assert
        Assert.IsAssignableFrom<OkObjectResult>(response);
    }

    [Fact]
    public void GetUploadedFiles_ReturnsObjectResult_WhenFilesNotRetrieved()
    {
        // Arrange
        _fileReaderServiceMock.Setup(s => s.GetUploadedFiles(It.IsAny<GetUploadedFilesModel>()))
            .Returns(new UploadedFileResult { Success = false });

        // Act
        var response = _controller.GetUploadedFiles();

        // Assert
        Assert.IsAssignableFrom<ObjectResult>(response);
    }

    [Fact]
    public void GetUploadedFiles_ReturnsValidResponse_WhenFileReaderServiceThrowsException()
    {
        // Arrange
        _fileReaderServiceMock.Setup(s => s.GetUploadedFiles(It.IsAny<GetUploadedFilesModel>()))
            .Throws<Exception>();

        // Act && Assert
        Assert.Throws<Exception>(() => _controller.GetUploadedFiles());
    }
}