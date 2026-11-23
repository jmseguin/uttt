namespace UTTT.Core.Game;

public class UltimateBoard
{
    private readonly Board[] _boards = new Board[9];
    public BoardState State { get; private set; } = BoardState.Available;
    public PlayerSymbol Winner { get; private set; } = PlayerSymbol.None;
    
    // -1 means any available board (Free Move)
    public int ActiveBoardIndex { get; internal set; } = -1;

    public UltimateBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            _boards[i] = new Board();
        }
    }

    public Board GetBoard(int index)
    {
        if (index < 0 || index >= 9)
            throw new ArgumentOutOfRangeException(nameof(index));
        return _boards[index];
    }

    public bool MakeMove(int boardIndex, int cellIndex, PlayerSymbol symbol)
    {
        if (State != BoardState.Available)
            return false;

        // Must play in the active board if it's set
        if (ActiveBoardIndex != -1 && boardIndex != ActiveBoardIndex)
            return false;

        // Target board must be available
        if (_boards[boardIndex].State != BoardState.Available)
            return false;

        // Try to make the move on the small board
        if (!_boards[boardIndex].MakeMove(cellIndex, symbol))
            return false;

        // Update active board index: the move's cellIndex determines the next boardIndex
        if (_boards[cellIndex].State == BoardState.Available)
        {
            ActiveBoardIndex = cellIndex;
        }
        else
        {
            // If the next board is already won or full, it's a Free Move
            ActiveBoardIndex = -1;
        }

        UpdateState();
        return true;
    }

    private void UpdateState()
    {
        // Check rows
        for (int i = 0; i < 9; i += 3)
        {
            if (_boards[i].Winner != PlayerSymbol.None && _boards[i].Winner == _boards[i + 1].Winner && _boards[i].Winner == _boards[i + 2].Winner)
            {
                SetWinner(_boards[i].Winner);
                return;
            }
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (_boards[i].Winner != PlayerSymbol.None && _boards[i].Winner == _boards[i + 3].Winner && _boards[i].Winner == _boards[i + 6].Winner)
            {
                SetWinner(_boards[i].Winner);
                return;
            }
        }

        // Check diagonals
        if (_boards[0].Winner != PlayerSymbol.None && _boards[0].Winner == _boards[4].Winner && _boards[0].Winner == _boards[8].Winner)
        {
            SetWinner(_boards[0].Winner);
            return;
        }
        if (_boards[2].Winner != PlayerSymbol.None && _boards[2].Winner == _boards[4].Winner && _boards[2].Winner == _boards[6].Winner)
        {
            SetWinner(_boards[2].Winner);
            return;
        }

        // Check for draw (all boards finished, no winner)
        bool allFinished = true;
        for (int i = 0; i < 9; i++)
        {
            if (_boards[i].State == BoardState.Available)
            {
                allFinished = false;
                break;
            }
        }

        if (allFinished)
        {
            State = BoardState.Draw;
        }
    }

    private void SetWinner(PlayerSymbol symbol)
    {
        Winner = symbol;
        State = symbol == PlayerSymbol.X ? BoardState.XWon : BoardState.OWon;
        ActiveBoardIndex = -2; // Game Over
    }

    public UltimateBoard Clone()
    {
        var clone = new UltimateBoard();
        for (int i = 0; i < 9; i++)
        {
            clone._boards[i] = _boards[i].Clone();
        }
        clone.State = this.State;
        clone.Winner = this.Winner;
        clone.ActiveBoardIndex = this.ActiveBoardIndex;
        return clone;
    }
}
