using Raylib_cs;
using UTTT.Core.Game;
using UTTT.Core.Players;
using System.Numerics;

namespace UTTT.Gui;

class Program
{
    private static readonly Color PrimaryColor = new Color(41, 121, 255, 255); // Vibrant Blue
    private static readonly Color SecondaryColor = new Color(255, 82, 82, 255); // Vibrant Red
    private static readonly Color BackgroundColor = new Color(18, 18, 18, 255); // Dark Theme
    private static readonly Color BoardLinesColor = new Color(60, 60, 60, 255); // Slightly lighter for visibility
    private static readonly Color HighlightColor = new Color(255, 255, 255, 20);
    private static readonly Color DimColor = new Color(0, 0, 0, 150); // For dimming won boards

    static void Main(string[] args)
    {
        const int screenWidth = 900;
        const int screenHeight = 1000; // Extra space for status if needed, or keeping it square

        Raylib.InitWindow(screenWidth, screenHeight - 100, "Ultimate Tic Tac Toe - Premium Edition");
        Raylib.SetTargetFPS(144);
        Raylib.SetExitKey(KeyboardKey.Null); // Prevent ESC from closing window

        GameEngine engine = new GameEngine();
        AiPlayer? ai = null;

        bool gameStarted = false;
        bool shouldExit = false;
        
        while (!Raylib.WindowShouldClose() && !shouldExit)
        {
            if (!gameStarted)
            {
                gameStarted = DrawMainMenu(ref ai, ref shouldExit);
                if (gameStarted) engine = new GameEngine();
            }
            else
            {
                Update(engine, ai, ref gameStarted);
                Draw(engine);
            }
        }

        Raylib.CloseWindow();
    }

    static bool DrawMainMenu(ref AiPlayer? ai, ref bool shouldExit)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(BackgroundColor);

        float time = (float)Raylib.GetTime();
        float pulse = (float)Math.Sin(time * 2) * 10;

        Raylib.DrawTextEx(Raylib.GetFontDefault(), "ULTIMATE", new Vector2(250, 120 + pulse), 80, 5, PrimaryColor);
        Raylib.DrawTextEx(Raylib.GetFontDefault(), "TIC TAC TOE", new Vector2(220, 210 + pulse), 80, 5, SecondaryColor);

        bool start = false;
        if (DrawButton("TWO PLAYER", 350, Color.DarkGray)) { start = true; ai = null; }
        if (DrawButton("EASY AI", 430, Color.Green)) { start = true; ai = new AiPlayer(PlayerSymbol.O, Difficulty.Easy); }
        if (DrawButton("MEDIUM AI", 510, Color.Orange)) { start = true; ai = new AiPlayer(PlayerSymbol.O, Difficulty.Medium); }
        if (DrawButton("HARD AI", 590, Color.Red)) { start = true; ai = new AiPlayer(PlayerSymbol.O, Difficulty.Hard); }
        if (DrawButton("EXIT", 700, Color.Gray)) { shouldExit = true; }

        Raylib.DrawText("A Modern Game Experience", 320, 820, 20, Color.Gray);

        Raylib.EndDrawing();
        return start;
    }

    static bool DrawButton(string text, int y, Color hoverColor)
    {
        Rectangle rect = new Rectangle(300, y, 300, 60);
        bool hover = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect);
        
        Raylib.DrawRectangleRec(rect, hover ? hoverColor : new Color(30, 30, 30, 255));
        Raylib.DrawRectangleLinesEx(rect, 2, hover ? Color.White : Color.Gray);
        
        int fontSize = 30;
        int textWidth = Raylib.MeasureText(text, fontSize);
        Raylib.DrawText(text, (int)(rect.X + (rect.Width - textWidth) / 2), (int)(rect.Y + (rect.Height - fontSize) / 2), fontSize, Color.White);

        return hover && Raylib.IsMouseButtonPressed(MouseButton.Left);
    }

    static void Update(GameEngine engine, AiPlayer? ai, ref bool gameStarted)
    {
        if (engine.IsGameOver)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                gameStarted = false;
            }
            return;
        }

        if (ai != null && engine.CurrentPlayer == PlayerSymbol.O)
        {
            var move = ai.GetBestMove(engine.Board);
            if (move != null) engine.MakeMove(move.Value.BoardIndex, move.Value.CellIndex);
            return;
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            var mousePos = Raylib.GetMousePosition();
            var (boardIdx, cellIdx) = GetMoveFromMouse(mousePos);
            if (boardIdx != -1) engine.MakeMove(boardIdx, cellIdx);
        }

        // Allow ESC to return to menu anytime (optional, but requested for end game)
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
             gameStarted = false;
        }
    }

    static (int boardIdx, int cellIdx) GetMoveFromMouse(Vector2 mousePos)
    {
        int boardX = (int)(mousePos.X / 300);
        int boardY = (int)(mousePos.Y / 300);
        int boardIdx = boardY * 3 + boardX;

        int cellX = (int)((mousePos.X % 300) / 100);
        int cellY = (int)((mousePos.Y % 300) / 100);
        int cellIdx = cellY * 3 + cellX;

        if (boardIdx >= 0 && boardIdx < 9 && cellIdx >= 0 && cellIdx < 9)
            return (boardIdx, cellIdx);

        return (-1, -1);
    }

    static void Draw(GameEngine engine)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(BackgroundColor);

        // Draw Boards
        for (int b = 0; b < 9; b++)
        {
            int bx = (b % 3) * 300;
            int by = (b / 3) * 300;

            var board = engine.Board.GetBoard(b);

            // Highlight Active
            if (!engine.IsGameOver && (engine.Board.ActiveBoardIndex == b || engine.Board.ActiveBoardIndex == -1))
            {
                if (board.State == BoardState.Available)
                {
                    Raylib.DrawRectangle(bx + 5, by + 5, 290, 290, HighlightColor);
                }
            }

            // Small Grid (Lighter to see where to click)
            for (int i = 1; i < 3; i++)
            {
                Raylib.DrawLine(bx + i * 100, by + 10, bx + i * 100, by + 290, BoardLinesColor);
                Raylib.DrawLine(bx + 10, by + i * 100, bx + 290, by + i * 100, BoardLinesColor);
            }

            // Marks
            for (int c = 0; c < 9; c++)
            {
                int cx = bx + (c % 3) * 100 + 50;
                int cy = by + (c / 3) * 100 + 50;
                var symbol = board.GetCell(c);
                if (symbol == PlayerSymbol.X) DrawMarkX(cx, cy, 30, PrimaryColor);
                else if (symbol == PlayerSymbol.O) DrawMarkO(cx, cy, 30, SecondaryColor);
            }

            // Won State (Dimming board)
            if (board.State != BoardState.Available)
            {
                // Dim the board
                Raylib.DrawRectangle(bx + 5, by + 5, 290, 290, DimColor);

                if (board.State == BoardState.XWon) DrawLargeMarkX(bx + 150, by + 150, PrimaryColor);
                else if (board.State == BoardState.OWon) DrawLargeMarkO(bx + 150, by + 150, SecondaryColor);
                else if (board.State == BoardState.Draw) Raylib.DrawText("DRAW", bx + 100, by + 130, 40, Color.Gray);
            }
        }

        // Ultimate Grid (More prominent)
        for (int i = 1; i < 3; i++)
        {
            Raylib.DrawLineEx(new Vector2(i * 300, 0), new Vector2(i * 300, 900), 6, Color.DarkGray);
            Raylib.DrawLineEx(new Vector2(0, i * 300), new Vector2(900, i * 300), 6, Color.DarkGray);
        }

        if (engine.IsGameOver)
        {
            string text = engine.Board.Winner == PlayerSymbol.None ? "DRAW" : (engine.Board.Winner == PlayerSymbol.X ? "X WINS" : "O WINS");
            Color color = engine.Board.Winner == PlayerSymbol.None ? Color.Gray : (engine.Board.Winner == PlayerSymbol.X ? PrimaryColor : SecondaryColor);
            Raylib.DrawRectangle(0, 0, 900, 900, new Color(18, 18, 18, 220));
            Raylib.DrawText(text, 450 - Raylib.MeasureText(text, 60) / 2, 420, 60, color);
            Raylib.DrawText("Press ESC to Main Menu", 320, 500, 20, Color.White);
        }

        Raylib.EndDrawing();
    }

    static void DrawMarkX(int x, int y, int size, Color color)
    {
        Raylib.DrawLineEx(new Vector2(x - size, y - size), new Vector2(x + size, y + size), 8, color);
        Raylib.DrawLineEx(new Vector2(x + size, y - size), new Vector2(x - size, y + size), 8, color);
    }

    static void DrawMarkO(int x, int y, int size, Color color)
    {
        Raylib.DrawCircleLines(x, y, size, color);
        Raylib.DrawCircleLines(x, y, size - 1, color);
    }

    static void DrawLargeMarkX(int x, int y, Color color)
    {
        DrawMarkX(x, y, 100, color);
    }

    static void DrawLargeMarkO(int x, int y, Color color)
    {
        DrawMarkO(x, y, 100, color);
    }
}
