using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using ValantDemoApi.Services.Utils;

namespace ValantDemoApi.Models.UploadMaze
{
  public class FileRequest
  {
    public string FileName { get; init; }
    public List<string> MazeFile { get; init; }

    public FileRequest(string fileName, List<string> mazeFile)
    {
      FileName = fileName;
      MazeFile = mazeFile;
    }
  }

  public class UploadMazeRequestValidator : AbstractValidator<FileRequest>
  {
    public UploadMazeRequestValidator()
    {
      RuleFor(r => r.FileName)
          .NotEmpty()
          .WithMessage("The file name cannot be empty. Please provide a valid file name.");

      RuleFor(r => r.MazeFile)
          .NotEmpty()
          .WithMessage("The maze file cannot be empty. Please provide at least one maze line.");

      RuleForEach(r => r.MazeFile)
          .Must(mazeLine => mazeLine.All(c => Constants.ALLOWED_CHARS.Contains(c, StringComparison.InvariantCultureIgnoreCase)))
          .WithMessage("Each line of the maze can only contain the following characters: 'S', 'O', 'X', and 'E'. Please review the input.");
    }
  }
}
