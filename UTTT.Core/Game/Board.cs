[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("UTTT.Tests")]

namespace UTTT.Core.Game;

public enum PlayerSymbol
{
    None = 0,
    X = 1,
    O = 2
}

public enum BoardState
{
    Available,
    XWon,
    OWon,
    Draw
}

public class Board
{
    private readonly PlayerSymbol[] _cells = new PlayerSymbol[9];
    public BoardState State { get; private set; } = BoardState.Available;
    public PlayerSymbol Winner { get; private set; } = PlayerSymbol.None;

    public PlayerSymbol GetCell(int index)
    {
        if (index < 0 || index >= 9)
            throw new ArgumentOutOfRangeException(nameof(index));
        return _cells[index];
    }

    public bool MakeMove(int index, PlayerSymbol symbol)
    {
        if (State != BoardState.Available)
            return false;

        if (index < 0 || index >= 9 || _cells[index] != PlayerSymbol.None)
            return false;

        _cells[index] = symbol;
        UpdateState();
        return true;
    }

    internal void SetCell(int index, PlayerSymbol symbol)
    {
        if (index < 0 || index >= 9)
            throw new ArgumentOutOfRangeException(nameof(index));
        _cells[index] = symbol;
        UpdateState();
    }

    public bool IsFull()
    {
        for (int i = 0; i < 9; i++)
        {
            if (_cells[i] == PlayerSymbol.None)
                return false;
        }
        return true;
    }

    private void UpdateState()
    {
        // Check rows
        for (int i = 0; i < 9; i += 3)
        {
            if (_cells[i] != PlayerSymbol.None && _cells[i] == _cells[i + 1] && _cells[i] == _cells[i + 2])
            {
                SetWinner(_cells[i]);
                return;
            }
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (_cells[i] != PlayerSymbol.None && _cells[i] == _cells[i + 3] && _cells[i] == _cells[i + 6])
            {
                SetWinner(_cells[i]);
                return;
            }
        }

        // Check diagonals
        if (_cells[0] != PlayerSymbol.None && _cells[0] == _cells[4] && _cells[0] == _cells[8])
        {
            SetWinner(_cells[0]);
            return;
        }
        if (_cells[2] != PlayerSymbol.None && _cells[2] == _cells[4] && _cells[2] == _cells[6])
        {
            SetWinner(_cells[2]);
            return;
        }

        if (IsFull())
        {
            State = BoardState.Draw;
        }
    }

    private void SetWinner(PlayerSymbol symbol)
    {
        Winner = symbol;
        State = symbol == PlayerSymbol.X ? BoardState.XWon : BoardState.OWon;
    }

    public Board Clone()
    {
        var clone = new Board();
        Array.Copy(_cells, clone._cells, 9);
        clone.State = this.State;
        clone.Winner = this.Winner;
        return clone;
    }
}
