using System.Collections.Generic;
using System.Threading.Tasks;

namespace ValantDemoApi.Services.Interfaces
{
  public interface IMazeFileService
  {
    Task<bool> SaveMazeToFileAsync(string fileName, List<string> mazeLines);
    IEnumerable<string> RetrieveAllMazes();
  }
}
