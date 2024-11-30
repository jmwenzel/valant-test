using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValantDemoApi.Repository.Interfaces;
using ValantDemoApi.Services.Interfaces;
using ValantDemoApi.Services.Utils;

namespace ValantDemoApi.Services
{
  public class MazeFileService : IMazeFileService
  {
    private readonly IMazeRepository _mazeRepository;

    public MazeFileService(IMazeRepository mazeRepository)
    {
      _mazeRepository = mazeRepository;  
    }

    public async Task<bool> SaveMazeToFileAsync(string fileName, List<string> mazeLines)
    {
      var maxLineLength = mazeLines.Max(line => line.Length);

      mazeLines = mazeLines
          .Select(line => line.PadRight(maxLineLength, Constants.WALL_CHAR).ToUpper())
      .ToList();

      return await _mazeRepository.SaveMazeAsync(fileName, mazeLines);
    }

    public IEnumerable<string> RetrieveAllMazes() => _mazeRepository.GetMazeList();
    
  }
}
