using System.Collections.Generic;
using System.Threading.Tasks;

namespace ValantDemoApi.Repository.Interfaces
{
  public interface IMazeFileHandler
  {
    void EnsureMazeDirectoryExists();
    IEnumerable<string> RetrieveMazeFileNames();
    Task<bool> SaveMazeToFileAsync(string fileName, List<string> mazeLines);
    Task<IEnumerable<string>> LoadMazeFromFileAsync(string fileName);
  }
}
