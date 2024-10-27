using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ValantDemoApi.Repository.Interfaces;

namespace ValantDemoApi.Repository
{
  public class MazeRepository : IMazeRepository
  {
    private readonly IMazeFileHandler _mazeFileHandler;

    public MazeRepository(IMazeFileHandler fileHandler)
    {
      _mazeFileHandler = fileHandler;
      _mazeFileHandler.EnsureMazeDirectoryExists();
    }

    public async Task<bool> SaveMazeAsync(string mazeFileName, List<string> mazeContent)
    {
      try
      {
        return await _mazeFileHandler.SaveMazeToFileAsync(mazeFileName, mazeContent);
      }
      catch (Exception)
      {
        return false;
      }
    }

    public IList<string> GetMazeList()
    {
      return _mazeFileHandler.RetrieveMazeFileNames().ToList();
    }

    public async Task<IList<string>> LoadMazeByIdAsync(string mazeFileName)
    {
      try
      {
        return (await _mazeFileHandler.LoadMazeFromFileAsync(mazeFileName)).ToList();
      }
      catch (Exception)
      {
        return new List<string>();
      }
    }
  }
}
