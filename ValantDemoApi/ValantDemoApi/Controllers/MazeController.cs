using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValantDemoApi.Models;
using ValantDemoApi.Services.Interfaces;

namespace ValantDemoApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MazeController : ControllerBase
  {

    private readonly IMazeService _mazeService;

    public MazeController(IMazeService mazeService)
    {
      _mazeService = mazeService;
    }

    [HttpPost("/mazes")]
    public MazeResponse GetPagedMazes(MazeRequest request)
    {
      var (total, mazes) = _mazeService.GetMazeList(request.StartIndex, request.Size);
      return new MazeResponse(total, mazes);
    }

    [HttpGet("/maze/{id}/availableMoves/{position}")]
    public async Task<IEnumerable<string>> GetNextAvailableMoves(string id, int position)
    {
      return await _mazeService.GetPossibleMoves(id, position);
    }

    [HttpGet("/maze/{id}")]
    public async Task<ActionResult<IEnumerable<string>>> RetrieveMazeByIdAsync(string id)
    {
      var maze = await _mazeService.GetMazeById(id);

      if (maze == null)
      {
        return NotFound();
      }

      return Ok(maze);
    }
  }
}
