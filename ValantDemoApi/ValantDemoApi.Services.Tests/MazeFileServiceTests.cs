using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValantDemoApi.Repository.Interfaces;
using ValantDemoApi.Services;

namespace ValantDemoApi.Tests
{
  [TestFixture]
  public class MazeFileServiceTests
  {
    private Mock<IMazeRepository> _mockMazeRepository;
    private MazeFileService _mazeFileService;

    [SetUp]
    public void SetUp()
    {
      _mockMazeRepository = new Mock<IMazeRepository>();
      _mazeFileService = new MazeFileService(_mockMazeRepository.Object);
    }

    [Test]
    public async Task SaveMazeToFileAsync_ShouldReturnTrue_WhenMazeIsSavedSuccessfully()
    {
      // Arrange
      var fileName = "testMaze.txt";
      var mazeLines = new List<string> { "S", "O", "X" };
      _mockMazeRepository.Setup(m => m.SaveMazeAsync(fileName, It.IsAny<List<string>>())).ReturnsAsync(true);

      // Act
      var result = await _mazeFileService.SaveMazeToFileAsync(fileName, mazeLines);

      // Assert
      Assert.IsTrue(result, "The maze should be saved successfully.");
      _mockMazeRepository.Verify(m => m.SaveMazeAsync(fileName, It.Is<List<string>>(lines =>
          lines.SequenceEqual(new List<string> { "S", "O", "X" }))), Times.Once);
    }

    [Test]
    public async Task SaveMazeToFileAsync_ShouldReturnFalse_WhenSaveFails()
    {
      // Arrange
      var fileName = "testMaze.txt";
      var mazeLines = new List<string> { "S", "O", "X" };
      _mockMazeRepository.Setup(m => m.SaveMazeAsync(fileName, It.IsAny<List<string>>())).ReturnsAsync(false);

      // Act
      var result = await _mazeFileService.SaveMazeToFileAsync(fileName, mazeLines);

      // Assert
      Assert.IsFalse(result, "The maze should not be saved successfully.");
    }

    [Test]
    public void RetrieveAllMazes_ShouldReturnMazeList()
    {
      // Arrange
      var mazeList = new List<string> { "maze1.txt", "maze2.txt" };
      _mockMazeRepository.Setup(m => m.GetMazeList()).Returns(mazeList);

      // Act
      var result = _mazeFileService.RetrieveAllMazes();

      // Assert
      Assert.AreEqual(mazeList.Count, result.Count(), "The number of maze files returned should match the expected count.");
      CollectionAssert.AreEquivalent(mazeList, result, "The returned maze file names should match the expected names.");
    }
  }
}
