using Core.Validators;
using Xunit;

namespace Tests.Unit.Core.Validators
{
    public class FileValidatorTests
    {
        [Fact]
        public void IsValidXmlFile_ReturnsTrue_WhenFileIsValid()
        {
            // Arrange         
            var file = FileTestHelper.CreateTestFile();

            // Act
            var isFileValid = FileValidator.IsValidXmlFile(file);

            // Assert
            Assert.True(isFileValid);
        }

        [Theory]
        [InlineData("text/html")]
        [InlineData("json")]
        public void IsValidXmlFile_ReturnsFalse_WhenInvalidFileType(string contentType)
        {
            // Arrange         
            var file = FileTestHelper.CreateTestFile(contentType);

            // Act
            var isFileValid = FileValidator.IsValidXmlFile(file);

            // Assert
            Assert.False(isFileValid);
        }

        [Fact]
        public void IsValidXmlFile_ReturnsFalse_WhenUploadedFileModelIsNull()
        {
            // Act
            var isFileValid = FileValidator.IsValidXmlFile(null);

            // Assert
            Assert.False(isFileValid);
        }
    }
}
