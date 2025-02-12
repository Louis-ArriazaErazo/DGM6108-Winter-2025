namespace Checkers;

public class Move
{
	// Determine Where & From The Piece Is Going And From
	public Piece PieceToMove { get; set; }

    // Coordinate For To 
	public (int X, int Y) To { get; set; }

	// Determines If The Piece Has Been Captured Or Not 
	public Piece? PieceToCapture { get; set; }

	// Setups Where The Piece Is Moving To, Based On The X & Y And If The Piece Is Captured
	public Move(Piece pieceToMove, (int X, int Y) to, Piece? pieceToCapture = null)
	{
		PieceToMove = pieceToMove;
		To = to;
		PieceToCapture = pieceToCapture;
	}
}
