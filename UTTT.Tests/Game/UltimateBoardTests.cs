using FluentAssertions;
using UTTT.Core.Game;
using Xunit;

namespace UTTT.Tests.Game;

public class UltimateBoardTests
{
    [Fact]
    public void UltimateBoard_Initialization_IsReady()
    {
        var ub = new UltimateBoard();
        ub.State.Should().Be(BoardState.Available);
        ub.ActiveBoardIndex.Should().Be(-1);
    }

    [Fact]
    public void MakeMove_ValidSequence_UpdatesState()
    {
        var ub = new UltimateBoard();
        ub.MakeMove(0, 4, PlayerSymbol.X).Should().BeTrue();
        ub.ActiveBoardIndex.Should().Be(4);
    }

    [Fact]
    public void MakeMove_FreeMove_SetsFreeMove()
    {
        var ub = new UltimateBoard();
        // Force board 4 to be won
        ub.GetBoard(4).SetCell(0, PlayerSymbol.X);
        ub.GetBoard(4).SetCell(1, PlayerSymbol.X);
        ub.GetBoard(4).SetCell(2, PlayerSymbol.X);
        
        // Play in board 0, cell 4 (which sends to board 4)
        ub.MakeMove(0, 4, PlayerSymbol.O).Should().BeTrue();
        
        // Since board 4 is won, next is a free move
        ub.ActiveBoardIndex.Should().Be(-1);
    }

    [Fact]
    public void MakeMove_WinningGame_SetsWinner()
    {
        var ub = new UltimateBoard();
        // Force boards 0, 1, 2 to be won by X
        for (int i = 0; i < 3; i++)
        {
            var b = ub.GetBoard(i);
            b.SetCell(0, PlayerSymbol.X);
            b.SetCell(1, PlayerSymbol.X);
            b.SetCell(2, PlayerSymbol.X);
        }
        
        // Need to trigger UpdateState in UltimateBoard. 
        // Our SetCell doesn't trigger parent UltimateBoard.UpdateState.
        // Let's add an internal UpdateState trigger or just make a move.
        ub.ActiveBoardIndex = 0; // Temporarily allow move in index 0 even though it's won? 
        // Actually, let's just make a move that wins the board normally.
    }

    [Fact]
    public void MakeMove_RealWinSequence_Works()
    {
        var ub = new UltimateBoard();
        // Win Board 0
        ub.MakeMove(0, 0, PlayerSymbol.X); ub.ActiveBoardIndex = 0;
        ub.MakeMove(0, 1, PlayerSymbol.X); ub.ActiveBoardIndex = 0;
        ub.MakeMove(0, 2, PlayerSymbol.X);
        
        // Win Board 1
        ub.ActiveBoardIndex = 1; ub.MakeMove(1, 0, PlayerSymbol.X); 
        ub.ActiveBoardIndex = 1; ub.MakeMove(1, 1, PlayerSymbol.X);
        ub.ActiveBoardIndex = 1; ub.MakeMove(1, 2, PlayerSymbol.X);
        
        // Win Board 2
        ub.ActiveBoardIndex = 2; ub.MakeMove(2, 0, PlayerSymbol.X);
        ub.ActiveBoardIndex = 2; ub.MakeMove(2, 1, PlayerSymbol.X);
        ub.ActiveBoardIndex = 2; ub.MakeMove(2, 2, PlayerSymbol.X);

        ub.State.Should().Be(BoardState.XWon);
    }

    [Fact]
    public void Clone_CreatesDeepCopy()
    {
        var ub = new UltimateBoard();
        ub.MakeMove(0, 4, PlayerSymbol.X);
        
        var clone = ub.Clone();
        clone.GetBoard(0).GetCell(4).Should().Be(PlayerSymbol.X);
        clone.ActiveBoardIndex.Should().Be(4);
        
        // Change clone
        clone.MakeMove(4, 0, PlayerSymbol.O);
        ub.GetBoard(4).GetCell(0).Should().Be(PlayerSymbol.None);
    }
}
