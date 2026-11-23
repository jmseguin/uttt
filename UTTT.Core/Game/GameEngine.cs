namespace UTTT.Core.Game;

public class GameEngine
{
    public UltimateBoard Board { get; }
    public PlayerSymbol CurrentPlayer { get; private set; }
    public bool IsGameOver => Board.State != BoardState.Available;

    public GameEngine()
    {
        Board = new UltimateBoard();
        CurrentPlayer = PlayerSymbol.X;
    }

    public bool MakeMove(int boardIndex, int cellIndex)
    {
        if (IsGameOver)
            return false;

        if (Board.MakeMove(boardIndex, cellIndex, CurrentPlayer))
        {
            if (!IsGameOver)
            {
                CurrentPlayer = CurrentPlayer == PlayerSymbol.X ? PlayerSymbol.O : PlayerSymbol.X;
            }
            return true;
        }

        return false;
    }
}
