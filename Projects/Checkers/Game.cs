namespace Checkers;

// Testing 12 Changes Rn
public class Game
{
	// Number Of Colors Possible Per Type Of Piece (Black & White)
	private const int PiecesPerColor = 12;

   // Turn Determine On Color 
	public PieceColor Turn { get; private set; }

	// Board Setup
	public Board Board { get; }

	// Winner Displayed Based On Color 
	public PieceColor? Winner { get; private set; }

	// Updates List Based On Player Number
	public List<Player> Players { get; }

	public Game(int humanPlayerCount)
	{
		// Human Player Options
		if (humanPlayerCount < 0 || 2 < humanPlayerCount) throw new ArgumentOutOfRangeException(nameof(humanPlayerCount));
		Board = new Board();
		Players = new()
		{
			new Player(humanPlayerCount >= 1, Black),
			new Player(humanPlayerCount >= 2, White),
		};
		Turn = Black;  // Updates Turn 
		Winner = null; //  As Winner Set To Null Due To No Winner
	}

    // Move Determined To Capture Or Promote
	public void PerformMove(Move move)
	{
		(move.PieceToMove.X, move.PieceToMove.Y) = move.To;
		if ((move.PieceToMove.Color is Black && move.To.Y is 7) ||
			(move.PieceToMove.Color is White && move.To.Y is 0))
		{
			move.PieceToMove.Promoted = true; // Promote If Conditions Above Are Met
		}
		if (move.PieceToCapture is not null)
		{
			Board.Pieces.Remove(move.PieceToCapture);  // Capture Piece
		}
		if (move.PieceToCapture is not null &&
			Board.GetPossibleMoves(move.PieceToMove).Any(m => m.PieceToCapture is not null))
		{
			Board.Aggressor = move.PieceToMove;    // Checks IF It's Possible Move
		}
		else
		{
			Board.Aggressor = null;
			Turn = Turn is Black ? White : Black;
		}
		CheckForWinner();
	}

    // Checking There's Any Pieces Left Of Opposing Color
	public void CheckForWinner()
	{
		if (!Board.Pieces.Any(piece => piece.Color is Black))
		{
			Winner = White;
		}
		if (!Board.Pieces.Any(piece => piece.Color is White))
		{
			Winner = Black;
		}
		if (Winner is null && Board.GetPossibleMoves(Turn).Count is 0)
		{
			Winner = Turn is Black ? White : Black;
		}
	}

	public int TakenCount(PieceColor colour) =>
		PiecesPerColor - Board.Pieces.Count(piece => piece.Color == colour);
}
