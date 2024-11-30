using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValantDemoApi.Repository;
using ValantDemoApi.Repository.Interfaces;

namespace ValantDemoApi.Tests
{
  [TestFixture]
  public class MazeRepositoryTests
  {
    private Mock<IMazeFileHandler> _mockFileHandler;
    private MazeRepository _mazeRepository;

    [SetUp]
    public void SetUp()
    {
      _mockFileHandler = new Mock<IMazeFileHandler>();
      _mazeRepository = new MazeRepository(_mockFileHandler.Object);
    }

    [Test]
    public async Task SaveMazeAsync_ShouldReturnTrue_WhenSaveIsSuccessful()
    {
      // Arrange
      var mazeFileName = "testMaze.txt";
      var mazeContent = new List<string> { "S", "O", "X", "E" };
      _mockFileHandler.Setup(m => m.SaveMazeToFileAsync(mazeFileName, mazeContent)).ReturnsAsync(true);

      // Act
      var result = await _mazeRepository.SaveMazeAsync(mazeFileName, mazeContent);

      // Assert
      Assert.IsTrue(result, "The maze should be saved successfully.");
    }

    [Test]
    public async Task SaveMazeAsync_ShouldReturnFalse_WhenSaveFails()
    {
      // Arrange
      var mazeFileName = "testMaze.txt";
      var mazeContent = new List<string> { "S", "O", "X", "E" };
      _mockFileHandler.Setup(m => m.SaveMazeToFileAsync(mazeFileName, mazeContent)).ThrowsAsync(new Exception("Save failed."));

      // Act
      var result = await _mazeRepository.SaveMazeAsync(mazeFileName, mazeContent);

      // Assert
      Assert.IsFalse(result, "The maze should not be saved successfully when an exception is thrown.");
    }

    [Test]
    public void GetMazeList_ShouldReturnListOfMazeFileNames()
    {
      // Arrange
      var mazeFileNames = new List<string> { "maze1.txt", "maze2.txt" };
      _mockFileHandler.Setup(m => m.RetrieveMazeFileNames()).Returns(mazeFileNames);

      // Act
      var result = _mazeRepository.GetMazeList();

      // Assert
      Assert.AreEqual(mazeFileNames.Count, result.Count, "The number of maze files returned should match the expected count.");
      CollectionAssert.AreEquivalent(mazeFileNames, result, "The returned maze file names should match the expected names.");
    }

    [Test]
    public async Task LoadMazeByIdAsync_ShouldReturnMazeLines_WhenFileExists()
    {
      // Arrange
      var mazeFileName = "existingMaze.txt";
      var mazeLines = new List<string> { "S", "O", "X", "E" };
      _mockFileHandler.Setup(m => m.LoadMazeFromFileAsync(mazeFileName)).ReturnsAsync(mazeLines);

      // Act
      var result = await _mazeRepository.LoadMazeByIdAsync(mazeFileName);

      // Assert
      Assert.AreEqual(mazeLines.Count, result.Count, "The number of lines returned should match the expected count.");
      CollectionAssert.AreEqual(mazeLines, result, "The loaded maze lines should match the expected lines.");
    }

    [Test]
    public async Task LoadMazeByIdAsync_ShouldReturnEmptyList_WhenFileDoesNotExist()
    {
      // Arrange
      var mazeFileName = "nonexistentMaze.txt";
      _mockFileHandler.Setup(m => m.LoadMazeFromFileAsync(mazeFileName)).ThrowsAsync(new Exception("File not found."));

      // Act
      var result = await _mazeRepository.LoadMazeByIdAsync(mazeFileName);

      // Assert
      Assert.IsEmpty(result, "The result should be an empty list when the file does not exist.");
    }
  }
}
