using System.Collections.Generic;

namespace ValantDemoApi.Models
{
  public class MazeResponse
  {
    public int Total { get; init; }
    public IEnumerable<string> Items { get; init; }

    public MazeResponse(int total, IEnumerable<string> items)
    {
      Total = total;
      Items = items;
    }
  }
}
