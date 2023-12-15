using Core.Constants;
using Core.Services;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Tests.Unit.Core.Services;

public class FileConvertServiceTests
{
    [Fact]
    public void ConvertXmlToJson_ReturnsCorrectResult_WhenTheXmlFileIsValid()
    {
        // Arrange
        var xmlFile = FileTestHelper.CreateTestFile();
        var service = new FileConvertService();

        // Act
        var convertToJsonResult = service.ConvertXmlToJson(xmlFile);

        // Assert
        Assert.NotNull(convertToJsonResult);
        Assert.True(convertToJsonResult.Success);
        Assert.Empty(convertToJsonResult.Errors);
        Assert.NotNull(convertToJsonResult.JsonContent);
    }

    [Fact]
    public void ConvertXmlToJson_ReturnsValidJsonResult()
    {
        // Arrange
        var xmlFile = FileTestHelper.CreateTestFile();
        var service = new FileConvertService();

        // Act
        var convertToJsonResult = service.ConvertXmlToJson(xmlFile);

        // Assert
        var convertedJson = convertToJsonResult.JsonContent;
        var parsedJson = JToken.Parse(convertedJson);
        Assert.NotNull(parsedJson);
    }

    [Fact]
    public void ConvertXmlToJson_ReturnsCorrectResult_WhenTheXmlFileContentIsNotValid()
    {
        // Arrange
        var xmlFile = FileTestHelper.CreateTestFile("text/xml", false);
        var service = new FileConvertService();

        // Act
        var convertToJsonResult = service.ConvertXmlToJson(xmlFile);

        // Assert
        Assert.NotNull(convertToJsonResult);
        Assert.False(convertToJsonResult.Success);
        Assert.NotEmpty(convertToJsonResult.Errors);
        Assert.Null(convertToJsonResult.JsonContent);
    }

    [Fact]
    public void ConvertXmlToJson_ReturnsCorrectResult_WhenTheXmlFileContentTypeIsNotValid()
    {
        // Arrange
        var xmlFile = FileTestHelper.CreateTestFile("text/invalid");
        var service = new FileConvertService();

        // Act
        var convertToJsonResult = service.ConvertXmlToJson(xmlFile);

        // Assert
        Assert.NotNull(convertToJsonResult);
        Assert.False(convertToJsonResult.Success);
        Assert.NotEmpty(convertToJsonResult.Errors);
        Assert.Null(convertToJsonResult.JsonContent);
    }

    [Fact]
    public void ConvertXmlToJson_ReturnsCorrectErrorMessage_WhenTheXmlFileIsNotValid()
    {
        // Arrange
        var xmlFile = FileTestHelper.CreateTestFile("text/invalid");
        var service = new FileConvertService();

        var expectedErrorMessage = UserMessages.InvalidXmlFileMessage;

        // Act
        var convertToJsonResult = service.ConvertXmlToJson(xmlFile);

        // Assert
        Assert.False(convertToJsonResult.Success);
        Assert.NotEmpty(convertToJsonResult.Errors);
        Assert.Contains(convertToJsonResult.Errors, e => e.ErrorMessage == expectedErrorMessage);
    }
}