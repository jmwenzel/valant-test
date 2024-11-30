using System.Collections.Generic;
using System.Threading.Tasks;

namespace ValantDemoApi.Services.Interfaces
{
  public interface IMazeService
  {
    (int totalCount, IList<string> mazeFiles) GetMazeList(int startIndex, int size);
    Task<IList<string>> GetMazeById(string id);
    Task<IList<string>> GetPossibleMoves(string id, int position);
  }
}
