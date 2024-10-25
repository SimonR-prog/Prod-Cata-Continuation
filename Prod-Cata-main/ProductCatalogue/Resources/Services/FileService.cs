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
        //Assigns the filepath string to the private field.
        _filePath = filePath;
    }

    public Response<string> GetFromFile()
    {
        //Returns a string type.
        try
        {
            //If the file does NOT exist at the filepath, throw exception.
            if (!File.Exists(_filePath))
                throw new FileNotFoundException("File not found");

            //Using will delete the string from the memory after it is used.
            //Using the streamreader from newtonsoft to bring in the string of data from the file at the filepath into the content variable.
            using var sr = new StreamReader(_filePath);
            var content = sr.ReadToEnd();

            //Returns the string in the content property from the response class.
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
            //Using will delete the string content from the memory after being used.
            //Streamwriter from newtonsoft will write the string in content into the file at the filepath.
            using var sw = new StreamWriter(_filePath, false);
            sw.WriteLine(content);

            //Just returns a true bool. (And a string tehnically..)
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
