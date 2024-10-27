using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValantDemoApi.Repository.Interfaces;
using ValantDemoApi.Services;
using ValantDemoApi.Services.Utils;

namespace ValantDemoApi.Tests
{
  [TestFixture]
  public class MazeServiceTests
  {
    private Mock<IMazeRepository> _mockMazeRepository;
    private MazeService _mazeService;

    [SetUp]
    public void SetUp()
    {
      _mockMazeRepository = new Mock<IMazeRepository>();
      _mazeService = new MazeService(_mockMazeRepository.Object);
    }

    [Test]
    public void GetMazeList_ShouldReturnEmpty_WhenNoMazesAvailable()
    {
      // Arrange
      _mockMazeRepository.Setup(m => m.GetMazeList()).Returns(new List<string>());

      // Act
      var result = _mazeService.GetMazeList(0, 10);

      // Assert
      Assert.AreEqual(0, result.totalCount);
      Assert.IsEmpty(result.mazeFiles);
    }

    [Test]
    public void GetMazeList_ShouldReturnCorrectMazes_WhenMazesAvailable()
    {
      // Arrange
      var mazeList = new List<string> { "maze1.txt", "maze2.txt", "maze3.txt" };
      _mockMazeRepository.Setup(m => m.GetMazeList()).Returns(mazeList);

      // Act
      var result = _mazeService.GetMazeList(0, 2);

      // Assert
      Assert.AreEqual(3, result.totalCount);
      CollectionAssert.AreEquivalent(new List<string> { "maze1.txt", "maze2.txt" }, result.mazeFiles);
    }

    [Test]
    public async Task GetMazeById_ShouldReturnMaze_WhenMazeExists()
    {
      // Arrange
      var mazeId = "maze1.txt";
      var mazeContent = new List<string> { "S", "O", "X" };
      _mockMazeRepository.Setup(m => m.LoadMazeByIdAsync(mazeId)).ReturnsAsync(mazeContent);

      // Act
      var result = await _mazeService.GetMazeById(mazeId);

      // Assert
      CollectionAssert.AreEquivalent(mazeContent, result);
    }

    [Test]
    public async Task GetMazeById_ShouldReturnEmpty_WhenMazeDoesNotExist()
    {
      // Arrange
      var mazeId = "nonexistentMaze.txt";
      _mockMazeRepository.Setup(m => m.LoadMazeByIdAsync(mazeId)).ReturnsAsync(new List<string>());

      // Act
      var result = await _mazeService.GetMazeById(mazeId);

      // Assert
      Assert.IsEmpty(result);
    }

    [Test]
    public async Task GetPossibleMoves_ShouldReturnAvailableMoves_WhenPositionIsValid()
    {
      // Arrange
      var mazeId = "maze1.txt";
      var mazeContent = new List<string>
            {
                "SXX",
                "OOO",
                "XOX"
            };
      var position = 3; // Position for 'O' in the maze
      var expectedMoves = new List<string> { Constants.UP, Constants.RIGHT };

      _mockMazeRepository.Setup(m => m.LoadMazeByIdAsync(mazeId)).ReturnsAsync(mazeContent);

      // Act
      var result = await _mazeService.GetPossibleMoves(mazeId, position);

      // Assert
      CollectionAssert.AreEquivalent(expectedMoves, result);
    }

    [Test]
    public async Task GetPossibleMoves_ShouldReturnEmpty_WhenPositionIsInvalid()
    {
      // Arrange
      var mazeId = "maze1.txt";
      var mazeContent = new List<string>
            {
                "SXX",
                "OOO",
                "XOX"
            };
      var position = -1; // Invalid position

      _mockMazeRepository.Setup(m => m.LoadMazeByIdAsync(mazeId)).ReturnsAsync(mazeContent);

      // Act
      var result = await _mazeService.GetPossibleMoves(mazeId, position);

      // Assert
      Assert.IsEmpty(result);
    }

    [Test]
    public async Task GetPossibleMoves_ShouldReturnEmpty_WhenMazeIsInvalid()
    {
      // Arrange
      var mazeId = "maze1.txt";
      var mazeContent = new List<string>(); // Empty maze
      var position = 0;

      _mockMazeRepository.Setup(m => m.LoadMazeByIdAsync(mazeId)).ReturnsAsync(mazeContent);

      // Act
      var result = await _mazeService.GetPossibleMoves(mazeId, position);

      // Assert
      Assert.IsEmpty(result);
    }
  }
}
