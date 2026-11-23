using FluentAssertions;
using UTTT.Core.Game;
using Xunit;

namespace UTTT.Tests.Game;

public class GameEngineTests
{
    [Fact]
    public void GameEngine_Initialization_PlayerXStarts()
    {
        var engine = new GameEngine();
        engine.CurrentPlayer.Should().Be(PlayerSymbol.X);
        engine.IsGameOver.Should().BeFalse();
        engine.Board.State.Should().Be(BoardState.Available);
    }

    [Fact]
    public void MakeMove_ValidMove_SwitchesTurns()
    {
        var engine = new GameEngine();
        engine.MakeMove(0, 4).Should().BeTrue();
        engine.CurrentPlayer.Should().Be(PlayerSymbol.O);
    }

    [Fact]
    public void MakeMove_InvalidMove_DoesNotSwitchTurns()
    {
        var engine = new GameEngine();
        engine.MakeMove(0, 4); // X to 0, 4
        engine.MakeMove(0, 0).Should().BeFalse(); // O tries board 0 (forbidden)
        engine.CurrentPlayer.Should().Be(PlayerSymbol.O);
    }

    [Fact]
    public void MakeMove_WinningMove_GameEnds()
    {
        var engine = new GameEngine();
        
        // Win Boards 0 and 1 for X
        for (int i = 0; i < 2; i++)
        {
            var b = engine.Board.GetBoard(i);
            b.SetCell(0, PlayerSymbol.X);
            b.SetCell(1, PlayerSymbol.X);
            b.SetCell(2, PlayerSymbol.X);
        }

        // Setup Board 2 for win
        var b2 = engine.Board.GetBoard(2);
        b2.SetCell(0, PlayerSymbol.X);
        b2.SetCell(1, PlayerSymbol.X);

        // Current player is X. Active board is -1 (after internal SetCell).
        // Making the final move in board 2, cell 2 should win the game.
        engine.Board.ActiveBoardIndex = 2;
        engine.MakeMove(2, 2).Should().BeTrue();

        engine.IsGameOver.Should().BeTrue();
        engine.Board.Winner.Should().Be(PlayerSymbol.X);
    }
}
