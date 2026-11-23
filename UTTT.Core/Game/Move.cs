namespace UTTT.Core.Game;

public record struct Move(int BoardIndex, int CellIndex);

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
