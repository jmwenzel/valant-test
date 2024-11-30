using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ValantDemoApi.Models.UploadMaze;
using ValantDemoApi.Services.Interfaces;

namespace ValantDemoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MazeFileController : ControllerBase
  {
    private readonly IMazeFileService _mazeFileService;

    public MazeFileController(IMazeFileService mazeFileService)
    {
      _mazeFileService = mazeFileService;
    }

    [HttpPost]
    public async Task<bool> SaveMazeToFileAsync(FileRequest request)
    {
      return await _mazeFileService.SaveMazeToFileAsync(request.FileName, request.MazeFile);
    }
  }
}
