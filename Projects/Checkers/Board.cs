using System.Reflection;

namespace Checkers;

public class Board
{
	// List Of Possible Pieces 
	public List<Piece> Pieces { get; }

	// List Of Possible Captures 
	public Piece? Aggressor { get; set; }

    // Updating Initial Position Of Piece To New Position
	public Piece? this[int x, int y] =>
		Pieces.FirstOrDefault(piece => piece.X == x && piece.Y == y);

	public Board()
	{
		// Creating Position For Initial Selection
		Aggressor = null;
		Pieces = new List<Piece>
			{
				new() { NotationPosition ="A3", Color = Black},
				new() { NotationPosition ="A1", Color = Black},
				new() { NotationPosition ="B2", Color = Black},
				new() { NotationPosition ="C3", Color = Black},
				new() { NotationPosition ="C1", Color = Black},
				new() { NotationPosition ="D2", Color = Black},
				new() { NotationPosition ="E3", Color = Black},
				new() { NotationPosition ="E1", Color = Black},
				new() { NotationPosition ="F2", Color = Black},
				new() { NotationPosition ="G3", Color = Black},
				new() { NotationPosition ="G1", Color = Black},
				new() { NotationPosition ="H2", Color = Black},

				new() { NotationPosition ="A7", Color = White},
				new() { NotationPosition ="B8", Color = White},
				new() { NotationPosition ="B6", Color = White},
				new() { NotationPosition ="C7", Color = White},
				new() { NotationPosition ="D8", Color = White},
				new() { NotationPosition ="D6", Color = White},
				new() { NotationPosition ="E7", Color = White},
				new() { NotationPosition ="F8", Color = White},
				new() { NotationPosition ="F6", Color = White},
				new() { NotationPosition ="G7", Color = White},
				new() { NotationPosition ="H8", Color = White},
				new() { NotationPosition ="H6", Color = White}
			};
	}

    // Setting Up Label To Be Equal X & Y Value
	public static string ToPositionNotationString(int x, int y)
	{
		if (!IsValidPosition(x, y)) throw new ArgumentException("Not a valid position!");
		return $"{(char)('A' + x)}{y + 1}";
	}

	// Converting Notation To Equal A Label Position
	public static (int X, int Y) ParsePositionNotation(string notation)
	{
		if (notation is null) throw new ArgumentNullException(nameof(notation));
		notation = notation.Trim().ToUpper();
		if (notation.Length is not 2 ||
			notation[0] < 'A' || 'H' < notation[0] ||
			notation[1] < '1' || '8' < notation[1])
			throw new FormatException($@"{nameof(notation)} ""{notation}"" is not valid");
		return (notation[0] - 'A', notation[1] - '1');
	}

    // Determing That Piece Cannot Be Move There 
	public static bool IsValidPosition(int x, int y) =>
		0 <= x && x < 8 &&
		0 <= y && y < 8;

    // Determines Closest Possible Capturable Piece
	public (Piece A, Piece B) GetClosestRivalPieces(PieceColor priorityColor)
	{
		double minDistanceSquared = double.MaxValue;
		(Piece A, Piece B) closestRivals = (null!, null!);

		// Does So By Opposing Color
		foreach (Piece a in Pieces.Where(piece => piece.Color == priorityColor))
		{
			foreach (Piece b in Pieces.Where(piece => piece.Color != priorityColor))
			{
				(int X, int Y) vector = (a.X - b.X, a.Y - b.Y);
				double distanceSquared = vector.X * vector.X + vector.Y * vector.Y;
				if (distanceSquared < minDistanceSquared)
				{
					minDistanceSquared = distanceSquared;
					closestRivals = (a, b);
				}
			}
		}
		return closestRivals;
	}

    // Possible Movement Locations Updates For Color
	public List<Move> GetPossibleMoves(PieceColor color)
	{
		List<Move> moves = new();
			foreach (Piece piece in Pieces.Where(piece => piece.Color == color))
			{
				moves.AddRange(GetPossibleMoves(piece));
			}
		return moves;
	}

	// Possible Movement Locations Updates For Piece Coordinates
	public List<Move> GetPossibleMoves(Piece piece)
	{
		List<Move> moves = new();
		// Rise & Run From Coordinates (Diagonally)
		ValidateMovement(-1, -1); // Down & Left
		ValidateMovement(-1, 1);  // Down & Right
		ValidateMovement(1, -1);  // Up & Left
		ValidateMovement(1,  1);  // Up & Right

       // Rise & Run From Coordinates (Vertically & Horitzonally)
		ValidateMovement(0, -1);  // Left
		ValidateMovement(0,1);   // Right
		ValidateMovement(-1,0);  // Down
		ValidateMovement(1,0);   // Up


	// If A Piece Reaches Power Increase, Movement Is Updated To 2 Spaces 
		if(piece.PowerIncrease && piece.Promoted && piece.upgradeTotal >= 3){
		ValidateMovement(0, -2);  // Left
		ValidateMovement(0,2);   // Right
		ValidateMovement(-2,0);  // Down
		ValidateMovement(2,0);  // Up
		}

	// If A Piece Reaches Power Increase, Movement Is Updated To 2 Spaces 
		if(piece.PowerIncrease && piece.Promoted && piece.upgradeTotal >= 6){
		ValidateMovement(0, -3);  // Left
		ValidateMovement(0,3);   // Right
		ValidateMovement(-3,0);  // Down
		ValidateMovement(3,0);  // Up
		}

	// If A Piece Reaches Power Increase, Movement Is Updated To 2 Spaces 
		if(piece.PowerIncrease && piece.Promoted && piece.upgradeTotal >= 9){
		ValidateMovement(0, -4);  // Left
		ValidateMovement(0,4);   // Right
		ValidateMovement(-4,0);  // Down
		ValidateMovement(4,0);  // Up
		}

		return moves;
		
        // Check For Conflicting Pieces 
		void ValidateMovement(int dx, int dy)
		{
			// Checks If Piece Is Moving Diagonally 
            if( dx != 0 && dy != 0){
			if (!piece.Promoted && piece.Color is Black && dy is -1) return;
			if (!piece.Promoted && piece.Color is White && dy is 1) return;
			}
			(int X, int Y) target = (piece.X + dx, piece.Y + dy);
			if (!IsValidPosition(target.X, target.Y)) return;
			PieceColor? targetColor = this[target.X, target.Y]?.Color;

			// Check If Opposing Piece Is Not In Range
			if (targetColor is null)
			{
				if (!IsValidPosition(target.X, target.Y)) return;
				Move newMove = new(piece, target);
				moves.Add(newMove);
			}
			// Check That Color Isn't The Same
			else if (targetColor != piece.Color)
			{
				(int X, int Y) jump = (piece.X + 2 * dx, piece.Y + 2 * dy);
				if (!IsValidPosition(jump.X, jump.Y)) return;
				PieceColor? jumpColor = this[jump.X, jump.Y]?.Color;
				if (jumpColor is not null) return;
				Move attack = new(piece, jump, this[target.X, target.Y]);
				moves.Add(attack);
			}
		}
	}

	/// <summary>Returns a <see cref="Move"/> if <paramref name="from"/>-&gt;<paramref name="to"/> is valid or null if not.</summary>
	/// // Validates If The Piece Can Move There 
	public Move? ValidateMove(PieceColor color, (int X, int Y) from, (int X, int Y) to)
	{
		Piece? piece = this[from.X, from.Y];
		if (piece is null)
		{
			return null;
		}
		foreach (Move move in GetPossibleMoves(color))
		{
			if ((move.PieceToMove.X, move.PieceToMove.Y) == from && move.To == to)
			{
				return move;
			}
		}
		return null;
	}

    // Updates Piece Onto That Position On The Board
	public static bool IsTowards(Move move, Piece piece)
	{
		(int Dx, int Dy) a = (move.PieceToMove.X - piece.X, move.PieceToMove.Y - piece.Y);
		int a_distanceSquared = a.Dx * a.Dx + a.Dy * a.Dy;
		(int Dx, int Dy) b = (move.To.X - piece.X, move.To.Y - piece.Y);
		int b_distanceSquared = b.Dx * b.Dx + b.Dy * b.Dy;
		return b_distanceSquared < a_distanceSquared;
	}
}
