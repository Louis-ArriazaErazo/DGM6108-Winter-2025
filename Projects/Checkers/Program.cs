// Louis Arriaza Erazo
// Date: 2/19/25

//  Requirement A:
// Randomize Player Turn Order
// Pieces Are Able To Move In All Directions 
// Special Piece (Uses Promoted Logo)

// Requirement B: 
// Abilities 

// Updates were made for these changes within the cs files of Game.cs, Board.cs and Piece.cs
// The changes that took place in Board.cs were changing ValidateDiagonal to ValidateMovement. Due to the
// movement of each piece being determined by the space like a graph. I made it that the coordinates for dx
// and dy were updated to (0,-1) for left, (0,1) for right. Then (1,0) for up and (-1,0) for down. I later on
// updated this segment to have it be that the new special piece that is obtained by increasing the TakenCount to
// above 3. Allows the player to move vertically and horitonzally by values of 2. This new power was added to the
// Piece.cs to have a new class with unique attributes be called throughout the program. In Game.cs the modifications
// were having the player that is played after the last is randomize. Meaning it could be the black piece player again 
// or the white piece again. Then I added that the random is voided when a player takes a piece from the other player. 
// It defaults to being the other player's turn. 

Exception? exception = null;

Encoding encoding = Console.OutputEncoding;

// Any Words Such As Game, Player Are In Reference To Their Respective CS Files
try
{
	Console.OutputEncoding = Encoding.UTF8;

    // Function Call To Display Options & Game Mode 
	Game game = ShowIntroScreenAndGetOption();
	Console.Clear();

    // Function Call To Run Game With A Parameter Of Game 
	RunGameLoop(game);

   // Function Call To Change The State Based On KeyPressed
	RenderGameState(game, promptPressKey: true);
	Console.ReadKey(true);
}

// Catching Used Along With The Try, & Finally To Catch Any Potiental Unexcepted Erora
catch (Exception e)
{
	exception = e;
	throw;
}
finally
{
	Console.OutputEncoding = encoding;
	Console.CursorVisible = true;
	Console.Clear();

    // Prompt That Is Displayed When Checkers Is Clocked 
	Console.WriteLine(exception?.ToString() ?? "Checkers was closed.");
}

// Starting Screen & Menu 
Game ShowIntroScreenAndGetOption()
{
	Console.Clear();
	Console.WriteLine();
	Console.WriteLine("  Checkers");
	Console.WriteLine();
	Console.WriteLine("  Checkers is played on an 8x8 board between two sides commonly known as black");
	Console.WriteLine("  and white. The objective is simple - capture all your opponent's pieces. An");
	Console.WriteLine("  alternative way to win is to trap your opponent so that they have no valid");
	Console.WriteLine("  moves left.");
	Console.WriteLine();
	Console.WriteLine("  Black starts first and players take it in turns to move their pieces forward");
	Console.WriteLine("  across the board diagonally. Should a piece reach the other side of the board");
	Console.WriteLine("  the piece becomes a king and can then move diagonally backwards as well as");
	Console.WriteLine("  forwards.");
	Console.WriteLine();
	Console.WriteLine("  Pieces are captured by jumping over them diagonally. More than one enemy piece");
	Console.WriteLine("  can be captured in the same turn by the same piece. If you can capture a piece");
	Console.WriteLine("  you must capture a piece.");
	Console.WriteLine();
	Console.WriteLine("  Moves are selected with the arrow keys. Use the [enter] button to select the");
	Console.WriteLine("  from and to squares. Invalid moves are ignored.");
	Console.WriteLine();
	Console.WriteLine("  Press a number key to choose number of human players:");
	Console.WriteLine("    [0] Black (computer) vs White (computer)");
	Console.WriteLine("    [1] Black (human) vs White (computer)");
	Console.Write("    [2] Black (human) vs White (human)");

// Comparsion Stating That humanplayer Can Hold An Int or A Null Value 
	int? humanPlayerCount = null;
	while (humanPlayerCount is null)
	{
		Console.CursorVisible = false;
		switch (Console.ReadKey(true).Key)
		{
            // Updates humanPlayercount Based On The Key Pressed 
			case ConsoleKey.D0 or ConsoleKey.NumPad0: humanPlayerCount = 0; break;
			case ConsoleKey.D1 or ConsoleKey.NumPad1: humanPlayerCount = 1; break;
			case ConsoleKey.D2 or ConsoleKey.NumPad2: humanPlayerCount = 2; break;
		}
	}
    // Returns The Value Of Players Within The Game 
	return new Game(humanPlayerCount.Value);
}

// Function To Run The Game (Takes Parameters To Set Up When A New Game)
void RunGameLoop(Game game)
{
    // While Loop Checking There Hasn't Been Anyone That Has Won Yet
	while (game.Winner is null)
	{
        // Based On Who The Current Player Is, The Player Will Go To Player Piece Color For Their Turn
		Player currentPlayer = game.Players.First(player => player.Color == game.Turn);

        // Checks If The Current Player Is Human 
		if (currentPlayer.IsHuman)
		{
            // While Loop To Have Player Go Through These Moves (Based On Color) 
			while (game.Turn == currentPlayer.Color)
			{
                // Checker Piece Starting Position 
				(int X, int Y)? selectionStart = null;

                // Checks Position Where It Is Currently, Then Where User Has Selected To Move
				(int X, int Y)? from = game.Board.Aggressor is not null ? (game.Board.Aggressor.X, game.Board.Aggressor.Y) : null;

                // Checks If The Selected Move Is A Possible Move For That Piece
				List<Move> moves = game.Board.GetPossibleMoves(game.Turn);

                // If The Move Is Possible, Then Have Piece Move To That Position
				if (moves.Select(move => move.PieceToMove).Distinct().Count() is 1)
				{
					Move must = moves.First();
					from = (must.PieceToMove.X, must.PieceToMove.Y);
					selectionStart = must.To; // Selected Position Isn't Null Anymore & Set To Destination 
				}
                // 
				while (from is null)
				{
					from = HumanMoveSelection(game);
					selectionStart = from; // Selected Position Isn't Null Anymore & Set To Coming From Position
				}

                // Piece Is Actually Then Move From Point From to Point To
				(int X, int Y)? to = HumanMoveSelection(game, selectionStart: selectionStart, from: from);

                // Piece Variable Can Be Set To A Value(Position On Game Board) Or Null
				Piece? piece = null;
				piece = game.Board[from.Value.X, from.Value.Y];

                // If It Isn't A Piece's Turn Set To Null
				if (piece is null || piece.Color != game.Turn)
				{
					from = null;
					to = null;
				}

                // If It Is A Piece's Turn Set Enable The Ability To Move 
				if (from is not null && to is not null)
				{
					Move? move = game.Board.ValidateMove(game.Turn, from.Value, to.Value);
					if (move is not null &&
						(game.Board.Aggressor is null || move.PieceToMove == game.Board.Aggressor))
					{
						game.PerformMove(move); // Moves The Piece 
					}
				}
			}
		}
		else
		{
            // List Of Possible Moves 
			List<Move> moves = game.Board.GetPossibleMoves(game.Turn);

            // List Of Possible Ways Of Capturing A Pieces
			List<Move> captures = moves.Where(move => move.PieceToCapture is not null).ToList();

            // Checks To See If Not All Pieces Have Been Captured 
			if (captures.Count > 0)
			{
				game.PerformMove(captures[Random.Shared.Next(captures.Count)]);  // Iterates Through All The Possible Ways Of Capturing (Based On Count Of Pieces)
			}
			else if(!game.Board.Pieces.Any(piece => piece.Color == game.Turn && !piece.Promoted))
			{
				var (a, b) = game.Board.GetClosestRivalPieces(game.Turn);  // Turn Of Capturing A Piece 
				Move? priorityMove = moves.FirstOrDefault(move => move.PieceToMove == a && Board.IsTowards(move, b));

                // Perform Move From What Is Determined(Most Logical Move?) Or Own Selection
				game.PerformMove(priorityMove ?? moves[Random.Shared.Next(moves.Count)]);
			}
			else
			{
                // Random Choice Of Selectioned Capture Piece
				game.PerformMove(moves[Random.Shared.Next(moves.Count)]);
			}
		}

        // Display Of Different Game States (Men, Player/CPU Moves) Based On Key Press
		RenderGameState(game, playerMoved: currentPlayer, promptPressKey: true);
		Console.ReadKey(true);
	}
}

// Fumction For Game States 
void RenderGameState(Game game, Player? playerMoved = null, (int X, int Y)? selection = null, (int X, int Y)? from = null, bool promptPressKey = false)
{
	const char BlackPiece = '○';  // Different Pieces As Labeled 
	const char BlackKing  = '☺';
	const char WhitePiece = '◙';
	const char WhiteKing  = '☻';  
	const char Vacant     = '·'; // No Piece 

// Board Setup Each With A Responding Coordinate And Spot 
	Console.CursorVisible = false;
	Console.SetCursorPosition(0, 0);
	StringBuilder sb = new();
	sb.AppendLine();
	sb.AppendLine("  Checkers");
	sb.AppendLine();
	sb.AppendLine($"    ╔═══════════════════╗");
	sb.AppendLine($"  8 ║  {B(0, 7)} {B(1, 7)} {B(2, 7)} {B(3, 7)} {B(4, 7)} {B(5, 7)} {B(6, 7)} {B(7, 7)}  ║ {BlackPiece} = Black");
	sb.AppendLine($"  7 ║  {B(0, 6)} {B(1, 6)} {B(2, 6)} {B(3, 6)} {B(4, 6)} {B(5, 6)} {B(6, 6)} {B(7, 6)}  ║ {BlackKing} = Black King");
	sb.AppendLine($"  6 ║  {B(0, 5)} {B(1, 5)} {B(2, 5)} {B(3, 5)} {B(4, 5)} {B(5, 5)} {B(6, 5)} {B(7, 5)}  ║ {WhitePiece} = White");
	sb.AppendLine($"  5 ║  {B(0, 4)} {B(1, 4)} {B(2, 4)} {B(3, 4)} {B(4, 4)} {B(5, 4)} {B(6, 4)} {B(7, 4)}  ║ {WhiteKing} = White King");
	sb.AppendLine($"  4 ║  {B(0, 3)} {B(1, 3)} {B(2, 3)} {B(3, 3)} {B(4, 3)} {B(5, 3)} {B(6, 3)} {B(7, 3)}  ║");
	sb.AppendLine($"  3 ║  {B(0, 2)} {B(1, 2)} {B(2, 2)} {B(3, 2)} {B(4, 2)} {B(5, 2)} {B(6, 2)} {B(7, 2)}  ║ Taken:");
	sb.AppendLine($"  2 ║  {B(0, 1)} {B(1, 1)} {B(2, 1)} {B(3, 1)} {B(4, 1)} {B(5, 1)} {B(6, 1)} {B(7, 1)}  ║ {game.TakenCount(White),2} x {WhitePiece}");
	sb.AppendLine($"  1 ║  {B(0, 0)} {B(1, 0)} {B(2, 0)} {B(3, 0)} {B(4, 0)} {B(5, 0)} {B(6, 0)} {B(7, 0)}  ║ {game.TakenCount(Black),2} x {BlackPiece}");
	sb.AppendLine($"    ╚═══════════════════╝");
	sb.AppendLine($"       A B C D E F G H");
	sb.AppendLine();

    // Checks If Select Isn't Taken Already
	if (selection is not null)
	{
		sb.Replace(" $ ", $"[{ToChar(game.Board[selection.Value.X, selection.Value.Y])}]"); // Updates Char Displayed Based On The Selected Spot 
	}

    // Checks If The Position It's Coming From Isn't Empty 
	if (from is not null)
	{
		char fromChar = ToChar(game.Board[from.Value.X, from.Value.Y]);
		sb.Replace(" @ ", $"<{fromChar}>"); // Removes Char
		sb.Replace("@ ",  $"{fromChar}>");
		sb.Replace(" @",  $"<{fromChar}");
	}

    // States Which Color Is A Winner 
	PieceColor? wc = game.Winner;

    // States Which Color's Turn It Is
	PieceColor? mc = playerMoved?.Color;

    // States Which Color's Turn It Is
	PieceColor? tc = game.Turn;

	// Note: these strings need to match in length
	// so they overwrite each other.
	string w = $"  *** {wc} wins ***"; // 
	string m = $"  {mc} moved       ";
	string t = $"  {tc}'s turn      ";

     // Displays If Not Empty
	sb.AppendLine(
		game.Winner is not null ? w :
		playerMoved is not null ? m :
		t);
	string p = "  Press any key to continue...";
	string s = "                              ";

    // Displays Prompt 
	sb.AppendLine(promptPressKey ? p : s);
	Console.Write(sb);

    // Move Selection 
	char B(int x, int y) =>
		(x, y) == selection ? '$' :
		(x, y) == from ? '@' :
		ToChar(game.Board[x, y]);

    // Updating Piece To King 
	static char ToChar(Piece? piece) =>
		piece is null ? Vacant :
		(piece.Color, piece.Promoted) switch
		{
			(Black, false) => BlackPiece,
			(Black, true)  => BlackKing,
			(White, false) => WhitePiece,
			(White, true)  => WhiteKing,
			_ => throw new NotImplementedException(),
		};
}

// Movement Controls 
(int X, int Y)? HumanMoveSelection(Game game, (int X, int y)? selectionStart = null, (int X, int Y)? from = null)
{
	(int X, int Y) selection = selectionStart ?? (3, 3); // Null Then Set To Coordinates
	while (true)
	{
		RenderGameState(game, selection: selection, from: from);
		switch (Console.ReadKey(true).Key)
		{
			case ConsoleKey.DownArrow:  selection.Y = Math.Max(0, selection.Y - 1); break;
			case ConsoleKey.UpArrow:    selection.Y = Math.Min(7, selection.Y + 1); break;
			case ConsoleKey.LeftArrow:  selection.X = Math.Max(0, selection.X - 1); break;
			case ConsoleKey.RightArrow: selection.X = Math.Min(7, selection.X + 1); break;
			case ConsoleKey.Enter:      return selection;
			case ConsoleKey.Escape:     return null;
		}
	}
}
