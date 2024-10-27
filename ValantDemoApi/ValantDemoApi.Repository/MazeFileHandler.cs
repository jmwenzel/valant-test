using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ValantDemoApi.Repository.Interfaces;

namespace ValantDemoApi.Repository
{
  public class MazeFileHandler : IMazeFileHandler
  {
    private readonly string mazeDirectory = Path.Combine(Environment.CurrentDirectory, "mazes");

    public void EnsureMazeDirectoryExists()
    {
      if (!Directory.Exists(mazeDirectory))
      {
        Directory.CreateDirectory(mazeDirectory);
      }
    }

    public IEnumerable<string> RetrieveMazeFileNames()
    {
      return Directory.GetFiles(mazeDirectory)
                     .Select(file => Path.GetFileName(file));
    }

    public async Task<bool> SaveMazeToFileAsync(string fileName, List<string> mazeLines)
    {
      var filePath = Path.Combine(mazeDirectory, fileName);
      using var streamWriter = new StreamWriter(filePath);

      foreach (var line in mazeLines)
      {
        await streamWriter.WriteLineAsync(line.ToUpper());
      }

      Console.WriteLine("Maze lines have been successfully saved to the file.");
      return true;
    }

    public async Task<IEnumerable<string>> LoadMazeFromFileAsync(string fileName)
    {
      var filePath = Path.Combine(mazeDirectory, fileName);
      if (!File.Exists(filePath))
      {
        throw new FileNotFoundException($"The file '{fileName}' was not found.");
      }

      using var streamReader = new StreamReader(filePath);
      var mazeLines = new List<string>();
      string line;

      // Read each line from the file asynchronously and add it to the list
      while ((line = await streamReader.ReadLineAsync()) != null)
      {
        mazeLines.Add(line);
      }

      return mazeLines;
    }
  }
}
