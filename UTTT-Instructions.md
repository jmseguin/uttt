# Ultimate Tic Tac Toe (UTTT) Instructions

## The Board
Ultimate Tic Tac Toe is played on a 3x3 grid of 3x3 Tic Tac Toe boards. We call the 3x3 grid the **Ultimate Board** and each individual 3x3 grid a **Small Board**.

## Objective
The objective is to win the **Ultimate Board** by winning three **Small Boards** in a row (horizontally, vertically, or diagonally).

## Gameplay Rules
1. **The First Move**: The first player (X) can place their mark in any of the 81 empty cells.
2. **Sending the Opponent**: The cell you choose in a Small Board determines which Small Board your opponent must play in next. 
   - *Example*: If you play in the top-right cell of a Small Board, your opponent MUST play their next move in the top-right Small Board of the Ultimate Board.
3. **Winning a Small Board**: When a player gets three in a row in a Small Board, they win that board. The Small Board is then marked with that player's symbol (X or O).
4. **Drawing a Small Board**: If a Small Board results in a draw, it is considered "sent" but neither player wins it. (Standard variant: it counts for neither).
5. **Full/Won Small Boards**: If a player is sent to a Small Board that is already won or full, they are allowed to play in ANY empty cell on ANY available Small Board (this is called a "Free Move").
6. **Winning the Game**: The first player to win three Small Boards in a row on the Ultimate Board wins the entire game.

## Strategy Tips
- Try to think ahead! Sometimes winning a Small Board is less important than where you send your opponent.
- Avoid sending your opponent to a board where they can easily win.
- Use "Free Moves" wisely to regain control of the game or block the opponent's progress.
