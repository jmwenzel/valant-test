using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValantDemoApi.Repository.Interfaces;
using ValantDemoApi.Services.Interfaces;
using ValantDemoApi.Services.Utils;

namespace ValantDemoApi.Services
{
  public class MazeService : IMazeService
  {
    private const int DEFAULT_START_INDEX = 0;
    private const int DEFAULT_SIZE = 30;
    private readonly IMazeRepository _repository;

    public MazeService(IMazeRepository repository)
    {
      _repository = repository;
    }

    public (int totalCount, IList<string> mazeFiles) GetMazeList(int startIndex, int size)
    {
      var mazes = _repository.GetMazeList();
      if (!mazes.Any())
      {
        return (0, new List<string>());
      }

      AdjustPaginationParameters(ref startIndex, ref size, mazes.Count);
      return (mazes.Count, mazes.Skip(startIndex).Take(size).ToList());
    }

    public async Task<IList<string>> GetMazeById(string id)
    {
      return await _repository.LoadMazeByIdAsync(id);
    }

    public async Task<IList<string>> GetPossibleMoves(string id, int position)
    {
      var maze = await _repository.LoadMazeByIdAsync(id);
      if (!IsMazeValid(maze) || !IsPositionValid(position, maze))
      {
        return new List<string>();
      }

      return DetermineAvailableMoves(maze, position);
    }

    private void AdjustPaginationParameters(ref int startIndex, ref int size, int totalItems)
    {
      if (totalItems < size || startIndex > totalItems)
      {
        startIndex = DEFAULT_START_INDEX;
        size = DEFAULT_SIZE;
      }
    }

    private static bool IsMazeValid(IList<string> maze) => maze != null && maze.Any();

    private static bool IsPositionValid(int position, IList<string> maze)
    {
      var rows = maze.Count;
      var columns = maze.Max(x => x.Length);
      var maxIndex = rows * columns - 1;
      return position >= 0 && position <= maxIndex;
    }

    private IList<string> DetermineAvailableMoves(IList<string> maze, int position)
    {
      var moves = new List<string>();
      var columns = maze.Max(x => x.Length);
      var rowIndex = position / columns;
      var columnIndex = position % columns;

      EvaluateMove(maze, rowIndex - 1, columnIndex, ValantConstants.UP, moves);    // UP
      EvaluateMove(maze, rowIndex + 1, columnIndex, ValantConstants.DOWN, moves);  // DOWN
      EvaluateMove(maze, rowIndex, columnIndex - 1, ValantConstants.LEFT, moves);  // LEFT
      EvaluateMove(maze, rowIndex, columnIndex + 1, ValantConstants.RIGHT, moves); // RIGHT

      return moves;
    }

    private void EvaluateMove(IList<string> maze, int rowIndex, int columnIndex, string direction, IList<string> availableMoves)
    {
      if (IsIndexWithinBounds(rowIndex, columnIndex, maze) && maze[rowIndex][columnIndex] != ValantConstants.WALL_CHAR)
      {
        availableMoves.Add(direction);
      }
    }

    private static bool IsIndexWithinBounds(int rowIndex, int columnIndex, IList<string> maze) =>
      rowIndex >= 0 && rowIndex < maze.Count && columnIndex >= 0 && columnIndex < maze[rowIndex].Length;
  }
}
