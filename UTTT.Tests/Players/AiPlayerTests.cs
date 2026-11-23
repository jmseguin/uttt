using FluentAssertions;
using UTTT.Core.Game;
using UTTT.Core.Players;
using Xunit;

namespace UTTT.Tests.Players;

public class AiPlayerTests
{
    [Fact]
    public void GetBestMove_EasyLevel_ReturnsValidMove()
    {
        var ub = new UltimateBoard();
        var ai = new AiPlayer(PlayerSymbol.X, Difficulty.Easy);
        
        var move = ai.GetBestMove(ub);
        
        move.Should().NotBeNull();
        ub.MakeMove(move.Value.BoardIndex, move.Value.CellIndex, PlayerSymbol.X).Should().BeTrue();
    }

    [Fact]
    public void GetBestMove_MediumLevel_WinsSmallBoardIfPossible()
    {
        var ub = new UltimateBoard();
        // X has (0,0) and (0,1). Active board is 0.
        ub.GetBoard(0).SetCell(0, PlayerSymbol.X);
        ub.GetBoard(0).SetCell(1, PlayerSymbol.X);
        ub.ActiveBoardIndex = 0;
        
        var ai = new AiPlayer(PlayerSymbol.X, Difficulty.Medium);
        var move = ai.GetBestMove(ub);
        
        move.Should().Be(new Move(0, 2));
    }

    [Fact]
    public void GetBestMove_MediumLevel_BlocksOpponent()
    {
        var ub = new UltimateBoard();
        // O has (0,0) and (0,1). Active board is 0.
        ub.GetBoard(0).SetCell(0, PlayerSymbol.O);
        ub.GetBoard(0).SetCell(1, PlayerSymbol.O);
        ub.ActiveBoardIndex = 0;
        
        var ai = new AiPlayer(PlayerSymbol.X, Difficulty.Medium);
        var move = ai.GetBestMove(ub);
        
        move.Should().Be(new Move(0, 2));
    }

    [Fact]
    public void GetBestMove_HardLevel_FindsWinningMove()
    {
        var ub = new UltimateBoard();
        // Win board 0 and 1. X has (2,0) and (2,1). Can win game in board 2.
        ub.GetBoard(0).SetCell(0, PlayerSymbol.X);
        ub.GetBoard(0).SetCell(1, PlayerSymbol.X);
        ub.GetBoard(0).SetCell(2, PlayerSymbol.X);
        
        ub.GetBoard(1).SetCell(0, PlayerSymbol.X);
        ub.GetBoard(1).SetCell(1, PlayerSymbol.X);
        ub.GetBoard(1).SetCell(2, PlayerSymbol.X);
        
        ub.GetBoard(2).SetCell(0, PlayerSymbol.X);
        ub.GetBoard(2).SetCell(1, PlayerSymbol.X);
        ub.ActiveBoardIndex = 2;

        var ai = new AiPlayer(PlayerSymbol.X, Difficulty.Hard);
        var move = ai.GetBestMove(ub);
        
        move.Should().Be(new Move(2, 2));
    }
}
