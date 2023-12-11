using System.Net.Http;
using XMLFileConverterApp.Models;

namespace XMLFileConverterApp.Helpers;

public static class HttpRequestHelper
{
    public static async Task<FileUploadResultModel> CallApiToUploadFileAsync(IFormFile file, HttpClient httpClient, string apiUrl)
    {
        var result = new FileUploadResultModel();

        if (file is not { Length: > 0 })
        {
            result.Success = false;
            return result;
        }

        try
        {
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
            {
                data = br.ReadBytes((int)file.OpenReadStream().Length);
            }

            var bytes = new ByteArrayContent(data);
            var multiContent = new MultipartFormDataContent();
            bytes.Headers.Add("Content-Type", file.ContentType);
            multiContent.Add(bytes, "file", file.FileName);

            var response = await httpClient.PostAsync(apiUrl, multiContent);

            result.Success = response.IsSuccessStatusCode;
            result.StatusCode = (int)response.StatusCode;

            var responseMessage = response.Content.ReadAsStringAsync().Result;
            result.ResponseMessage = responseMessage;

            return result;
        }
        catch (Exception)
        {
            result.StatusCode = 500;
            result.Success = false;
            result.ResponseMessage = "An error occurred while uploading your file. The upload file API might be down.";
            return result;
        }
    }
}