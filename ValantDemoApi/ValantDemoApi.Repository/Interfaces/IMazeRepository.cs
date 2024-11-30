using System.Collections.Generic;
using System.Threading.Tasks;

namespace ValantDemoApi.Repository.Interfaces
{
  public interface IMazeRepository
  {
    Task<bool> SaveMazeAsync(string mazeFileName, List<string> mazeContent);
    IList<string> GetMazeList();
    Task<IList<string>> LoadMazeByIdAsync(string mazeFileName);
  }
}
