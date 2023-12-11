using Microsoft.AspNetCore.Http;

namespace Tests
{
    public static class FileTestHelper
    {
        public static IFormFile CreateTestFile(string contentType = "text/xml", bool setValidXmlContent = true)
        {
            string content = "invalid xml";
            if (setValidXmlContent)
            {
                content =
                    "<note>\r\n<to>Tove</to>\r\n<from>Jani</from>\r\n<heading>Reminder</heading>\r\n<body>Don't forget me this weekend!</body>\r\n</note>";
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            IFormFile file = new FormFile(stream, 0, stream.Length, "test_file", "test.xml")
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return file;
        }

        public static void DeleteFilesFrom(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
