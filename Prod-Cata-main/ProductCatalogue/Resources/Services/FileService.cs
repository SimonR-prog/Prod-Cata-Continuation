using Resources.Interface;
using Resources.Models;

namespace Resources.Services;

public class FileService : IFileService
{
    //Making of a private field to use.
    private readonly string _filePath;

    //Constructor to assign the filepath from the program file into the private field above.
    public FileService(string filePath)
    {
        _filePath = filePath;
    }

    public Response<string> GetFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("File not found");

            using var sr = new StreamReader(_filePath);
            var content = sr.ReadToEnd();

            return new Response<string>
            {
                Succeeded = true,
                Content = content
            };
        }
        catch (Exception ex)
        {
            return new Response<string>
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }

    public Response<string> SaveToFile(string content)
    {
        try
        {
            using var sw = new StreamWriter(_filePath, false);
            sw.WriteLine(content);

            return new Response<string>
            {
                Succeeded = true
            };
        }
        catch (Exception ex)
        {
            return new Response<string>
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }
}
