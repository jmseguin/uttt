using FluentAssertions;
using UTTT.Core.Game;
using Xunit;

namespace UTTT.Tests.Game;

public class BoardTests
{
    [Fact]
    public void Board_Initialization_IsAvailable()
    {
        var board = new Board();
        board.State.Should().Be(BoardState.Available);
        board.Winner.Should().Be(PlayerSymbol.None);
        for (int i = 0; i < 9; i++)
        {
            board.GetCell(i).Should().Be(PlayerSymbol.None);
        }
    }

    [Theory]
    [InlineData(0, PlayerSymbol.X)]
    [InlineData(4, PlayerSymbol.O)]
    [InlineData(8, PlayerSymbol.X)]
    public void MakeMove_ValidMove_ReturnsTrueAndUpdatesCell(int index, PlayerSymbol symbol)
    {
        var board = new Board();
        bool result = board.MakeMove(index, symbol);
        result.Should().BeTrue();
        board.GetCell(index).Should().Be(symbol);
    }

    [Fact]
    public void MakeMove_InvalidIndex_ReturnsFalse()
    {
        var board = new Board();
        board.MakeMove(-1, PlayerSymbol.X).Should().BeFalse();
        board.MakeMove(9, PlayerSymbol.X).Should().BeFalse();
    }

    [Fact]
    public void MakeMove_CellAlreadyOccupied_ReturnsFalse()
    {
        var board = new Board();
        board.MakeMove(0, PlayerSymbol.X);
        bool result = board.MakeMove(0, PlayerSymbol.O);
        result.Should().BeFalse();
        board.GetCell(0).Should().Be(PlayerSymbol.X);
    }

    [Fact]
    public void GetCell_InvalidIndex_ThrowsException()
    {
        var board = new Board();
        Action act = () => board.GetCell(-1);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0, 1, 2)] // Row 0
    [InlineData(3, 4, 5)] // Row 1
    [InlineData(6, 7, 8)] // Row 2
    [InlineData(0, 3, 6)] // Col 0
    [InlineData(1, 4, 7)] // Col 1
    [InlineData(2, 5, 8)] // Col 2
    [InlineData(0, 4, 8)] // Diag 1
    [InlineData(2, 4, 6)] // Diag 2
    public void MakeMove_WinningLine_SetsWinnerAndWonState(int i1, int i2, int i3)
    {
        var board = new Board();
        board.MakeMove(i1, PlayerSymbol.X);
        board.MakeMove(i2, PlayerSymbol.X);
        board.MakeMove(i3, PlayerSymbol.X);

        board.State.Should().Be(BoardState.XWon);
        board.Winner.Should().Be(PlayerSymbol.X);
    }

    [Fact]
    public void MakeMove_OWins_SetsOWonState()
    {
        var board = new Board();
        board.MakeMove(0, PlayerSymbol.O);
        board.MakeMove(1, PlayerSymbol.O);
        board.MakeMove(2, PlayerSymbol.O);

        board.State.Should().Be(BoardState.OWon);
        board.Winner.Should().Be(PlayerSymbol.O);
    }

    [Fact]
    public void MakeMove_FullBoardNoWinner_SetsDrawState()
    {
        var board = new Board();
        // X O X
        // X X O
        // O X O
        board.MakeMove(0, PlayerSymbol.X);
        board.MakeMove(1, PlayerSymbol.O);
        board.MakeMove(2, PlayerSymbol.X);
        board.MakeMove(3, PlayerSymbol.X);
        board.MakeMove(4, PlayerSymbol.X);
        board.MakeMove(5, PlayerSymbol.O);
        board.MakeMove(6, PlayerSymbol.O);
        board.MakeMove(7, PlayerSymbol.X);
        board.MakeMove(8, PlayerSymbol.O);

        board.State.Should().Be(BoardState.Draw);
        board.Winner.Should().Be(PlayerSymbol.None);
    }

    [Fact]
    public void MakeMove_BoardNotAvailable_ReturnsFalse()
    {
        var board = new Board();
        board.MakeMove(0, PlayerSymbol.X);
        board.MakeMove(1, PlayerSymbol.X);
        board.MakeMove(2, PlayerSymbol.X); // X Wins

        board.State.Should().NotBe(BoardState.Available);
        bool result = board.MakeMove(3, PlayerSymbol.O);
        result.Should().BeFalse();
    }
}
