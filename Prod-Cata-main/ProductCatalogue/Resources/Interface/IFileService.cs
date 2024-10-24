using Resources.Models;

namespace Resources.Interface
{
    public interface IFileService
    {
        Response<string> GetFromFile();
        Response<string> SaveToFile(string content);
    }
}