using FluentValidation;

namespace ValantDemoApi.Models
{
  public sealed class MazeRequest
  {
    public int StartIndex { get; init; }
    public int Size { get; init; }

    public MazeRequest(int startIndex, int size)
    {
      StartIndex = startIndex;
      Size = size;
    }
  }

  public class GetMazesRequestValidator : AbstractValidator<MazeRequest>
  {
    public GetMazesRequestValidator()
    {
      RuleFor(r => r.StartIndex).GreaterThanOrEqualTo(0);
      RuleFor(r => r.Size).InclusiveBetween(1, 100);
    }
  }
}
