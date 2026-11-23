using UTTT.Core.Game;

namespace UTTT.Core.Players;

public class AiPlayer
{
    private readonly Difficulty _difficulty;
    private readonly PlayerSymbol _symbol;
    private readonly Random _random = new Random();

    public AiPlayer(PlayerSymbol symbol, Difficulty difficulty)
    {
        _symbol = symbol;
        _difficulty = difficulty;
    }

    public Move? GetBestMove(UltimateBoard board)
    {
        var validMoves = GetValidMoves(board);
        if (validMoves.Count == 0) return null;

        return _difficulty switch
        {
            Difficulty.Easy => GetRandomMove(validMoves),
            Difficulty.Medium => GetMediumMove(board, validMoves),
            Difficulty.Hard => GetHardMove(board),
            _ => GetRandomMove(validMoves)
        };
    }

    private Move GetRandomMove(List<Move> validMoves)
    {
        return validMoves[_random.Next(validMoves.Count)];
    }

    private Move GetMediumMove(UltimateBoard board, List<Move> validMoves)
    {
        // 1. Try to win a small board
        foreach (var move in validMoves)
        {
            var tempBoard = CloneBoard(board);
            tempBoard.MakeMove(move.BoardIndex, move.CellIndex, _symbol);
            if (tempBoard.GetBoard(move.BoardIndex).Winner == _symbol)
                return move;
        }

        // 2. Try to block opponent from winning a small board
        var opponent = _symbol == PlayerSymbol.X ? PlayerSymbol.O : PlayerSymbol.X;
        foreach (var move in validMoves)
        {
            var tempBoard = CloneBoard(board);
            tempBoard.MakeMove(move.BoardIndex, move.CellIndex, opponent);
            if (tempBoard.GetBoard(move.BoardIndex).Winner == opponent)
                return move;
        }

        return GetRandomMove(validMoves);
    }

    private Move GetHardMove(UltimateBoard board)
    {
        // Simple Minimax for now
        var bestMove = new Move(-1, -1);
        int bestScore = int.MinValue;
        var validMoves = GetValidMoves(board);

        foreach (var move in validMoves)
        {
            var tempBoard = CloneBoard(board);
            tempBoard.MakeMove(move.BoardIndex, move.CellIndex, _symbol);
            int score = Minimax(tempBoard, 4, false, int.MinValue, int.MaxValue);
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(UltimateBoard board, int depth, bool isMaximizing, int alpha, int beta)
    {
        if (board.State == (_symbol == PlayerSymbol.X ? BoardState.XWon : BoardState.OWon)) return 1000 + depth;
        if (board.State == (_symbol == PlayerSymbol.X ? BoardState.OWon : BoardState.XWon)) return -1000 - depth;
        if (board.State == BoardState.Draw) return 0;
        if (depth == 0) return EvaluateBoard(board);

        var validMoves = GetValidMoves(board);
        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (var move in validMoves)
            {
                var tempBoard = CloneBoard(board);
                tempBoard.MakeMove(move.BoardIndex, move.CellIndex, _symbol);
                int eval = Minimax(tempBoard, depth - 1, false, alpha, beta);
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha) break;
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            var opponent = _symbol == PlayerSymbol.X ? PlayerSymbol.O : PlayerSymbol.X;
            foreach (var move in validMoves)
            {
                var tempBoard = CloneBoard(board);
                tempBoard.MakeMove(move.BoardIndex, move.CellIndex, opponent);
                int eval = Minimax(tempBoard, depth - 1, true, alpha, beta);
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha) break;
            }
            return minEval;
        }
    }

    private int EvaluateBoard(UltimateBoard board)
    {
        int score = 0;
        var opponent = _symbol == PlayerSymbol.X ? PlayerSymbol.O : PlayerSymbol.X;

        for (int i = 0; i < 9; i++)
        {
            var b = board.GetBoard(i);
            if (b.Winner == _symbol) score += 100;
            else if (b.Winner == opponent) score -= 100;
            else if (b.State == BoardState.Available)
            {
                // Simple heuristic: count symbols in board
                for (int j = 0; j < 9; j++)
                {
                    if (b.GetCell(j) == _symbol) score += 1;
                    else if (b.GetCell(j) == opponent) score -= 1;
                }
            }
        }
        return score;
    }

    private List<Move> GetValidMoves(UltimateBoard board)
    {
        var moves = new List<Move>();
        if (board.ActiveBoardIndex != -1)
        {
            AddMovesForBoard(board, board.ActiveBoardIndex, moves);
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                if (board.GetBoard(i).State == BoardState.Available)
                {
                    AddMovesForBoard(board, i, moves);
                }
            }
        }
        return moves;
    }

    private void AddMovesForBoard(UltimateBoard board, int boardIndex, List<Move> moves)
    {
        var b = board.GetBoard(boardIndex);
        for (int i = 0; i < 9; i++)
        {
            if (b.GetCell(i) == PlayerSymbol.None)
            {
                moves.Add(new Move(boardIndex, i));
            }
        }
    }

    private UltimateBoard CloneBoard(UltimateBoard original)
    {
        return original.Clone();
    }
}
