namespace Checkers;

public class Piece
{
	// X Postion Can Be Found & Set 
	public int X { get; set; }

	// Y Postion Can Be Found & Set 
	public int Y { get; set; }

//   Setting Up Position For Piece Based On Finding It 
	public string NotationPosition
	{
		get => Board.ToPositionNotationString(X, Y);
		set => (X, Y) = Board.ParsePositionNotation(value);
	}

// Displaying Piece When Placed On Position
	public PieceColor Color { get; init; }

 // Changing Piece Type Based On Position 
	public bool Promoted { get; set; }

// Setting Upgrade total
	public int upgradeTotal{ get; set; }

// Setting Up Power Increase Update 
	public bool PowerIncrease { get; set; }
}