// Louis Arriaza Erazo
// Date: 2/25/25

namespace Checkers;

public class Player
{
	// Check If The Player Is Either Human Or Computer
	public bool IsHuman { get; }

    // Color Selection
	public PieceColor Color { get; }

	// Setups Player With A Piece Color
	public Player(bool isHuman, PieceColor color)
	{
		IsHuman = isHuman;
		Color = color;
	}
}
