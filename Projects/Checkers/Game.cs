using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography;

namespace Checkers;

// Testing 12 Changes Rn Github test
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
		Random randomturn = new Random();
			int colorTurn = randomturn.Next(0, 2);

			// Randomize It But Has It That The Opposing Color Follows
			if(colorTurn == 0){
				Turn = Black;
			} else {
			Turn = White;
			} // Updates Turn 
		Winner = null; //  As Winner Set To Null Due To No Winner
	}

    // Move Determined To Capture Or Promote
	public void PerformMove(Move move)
	{
		int WhiteTaken = TakenCount(PieceColor.White);
		int BlackTaken = TakenCount(PieceColor.Black);

		(move.PieceToMove.X, move.PieceToMove.Y) = move.To;
		if ((move.PieceToMove.Color is Black && TakenCount(White) >= 3) ||
			(move.PieceToMove.Color is White && TakenCount(Black) >= 3))
		{
			move.PieceToMove.PowerIncrease = true;
			move.PieceToMove.Promoted = true; 
			move.PieceToMove.upgradeTotal = 3; 
			  // Promote If Conditions Above Are Met
		}
		if ((move.PieceToMove.Color is Black && TakenCount(White) >= 6) ||
			(move.PieceToMove.Color is White && TakenCount(Black) >= 6))
		{
			move.PieceToMove.PowerIncrease = true;
			move.PieceToMove.Promoted = true;
			move.PieceToMove.upgradeTotal = 6;      // Promote If Conditions Above Are Met
		}
		if ((move.PieceToMove.Color is Black && TakenCount(White) >= 9) ||
			(move.PieceToMove.Color is White && TakenCount(Black) >= 9))
		{
			move.PieceToMove.PowerIncrease = true;
			move.PieceToMove.Promoted = true;  
			move.PieceToMove.upgradeTotal = 9;      // Promote If Conditions Above Are Met
		}
		if (move.PieceToCapture is not null)
		{
			Board.Pieces.Remove(move.PieceToCapture);
		}
		Board.Aggressor = null;
		//Add Randomizing Who's Turn It Is
		Turn = Turn is Black ? White : Black;
		// CheckForWinner();
	}

    // Checking There's Any Pieces Left Of Opposing Color
	// public void CheckForWinner()
	// {
	// 	if (!Board.Pieces.Any(piece => piece.Color is Black))
	// 	{
	// 		Winner = White;
	// 	}
	// 	if (!Board.Pieces.Any(piece => piece.Color is White))
	// 	{
	// 		Winner = Black;
	// 	}
	// 	if (Winner is null && Board.GetPossibleMoves(Turn).Count is 0)
	// 	{
	// 		Winner = Turn is Black ? White : Black;
	// 	}
	// }

	public int TakenCount(PieceColor colour) =>
		PiecesPerColor - Board.Pieces.Count(piece => piece.Color == colour);
}