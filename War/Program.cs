using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Security;


// Louis Arriaza Erazo
// DGM 6308 
// 03/12/25

// List Setup For Deck, Used Cards, Opponent's Hand (dealerHand)
List <Card> deck;
List <Card> usedCards;
List <Card> dealerHand;

// Initial Scores For Player and Opponent and Draws
int playerOne = 0;
int playerTwo = 0;
int draws = 0;
int i = 0;


// The Initial Deck With All Cards 
void StarterDeck(){
    deck = new List<Card>();
    usedCards = new List<Card>();

	// Iterates Through Each Suit and Value To Set It For A Card Draw
	foreach(Suit suit in Enum.GetValues(typeof(Suit))){
    foreach(Value value in Enum.GetValues(typeof(Value))){
        deck.Add(new Card{Suit = suit, Value = value});
  }
  }
}

// Function Used To Shuffle Card Within The List 
void Shuffle(List<Card> cards)
{
	for (int i = 0; i < cards.Count; i++)
	{
		int swap = Random.Shared.Next(cards.Count);
		(cards[i], cards[swap]) = (cards[swap], cards[i]);
	}
}

// Function Used To Add Drawn/Used Cards To Discarded Deck
void discardCards(){
    deck.AddRange(usedCards);
    usedCards.Clear();

	// Reshuffles Cards Out Of The Remaining Cards 
    Shuffle(deck);
}

// Draws A Card and Removes It From Deck
Card DrawCard()
{
	// Continues To Draw Till No Cards Are Left
	if (deck.Count <= 0)
	{
		discardCards();
	}
	Card card = deck[^1];
	deck.RemoveAt(deck.Count - 1);
	return card;
}

// Function Used To Compare Cards Between You and The Opponent 
void cardComparsion(){
    Console.Clear();
    
    // Opponent's Card Drawing (Initial View)
    Console.WriteLine("\nOpponent's Card");

	// Variable Setup For Each Of The Opponent's Three Card Draws 
    var dealerCardOne = DrawCard();
	var dealerCardTwo = DrawCard();
	var dealerCardThree = DrawCard();

	// Rendering The Three Cards That Are Drawn 
    RenderCard(dealerCardOne);
	RenderCard(dealerCardTwo);
	RenderCard(dealerCardThree);

    // Setups Card To Opponent's Hand 
    dealerHand.Add(dealerCardOne);
	dealerHand.Add(dealerCardOne);
	dealerHand.Add(dealerCardOne);

    // Confirming Player's Turn
    Console.WriteLine("\nPress Enter To Draw Your Card");
    Console.ReadLine();
    

    // Opponent's Card Drawing (Secondary View)
    Console.Clear();
    Console.WriteLine("\nOpponent's Card");
    RenderCard(dealerCardOne);
	RenderCard(dealerCardTwo);
	RenderCard(dealerCardThree);

    // Player's Card Drawing 
    Console.Write("\nPlayer's Card");

	// Variable Setup For Each Of The Player's Three Card Draws 
    var playerCardOne = DrawCard();
	var playerCardTwo = DrawCard();
	var playerCardThree = DrawCard();

	// Rendering The Three Cards That Are Drawn 
    RenderCard(playerCardOne);
	RenderCard(playerCardTwo);
	RenderCard(playerCardThree);

   // Confirms To Show Result Screen 
    Console.WriteLine("\nPress Enter For Results");
    Console.ReadLine();
    
    // Obtains Values From Each Of The Player's Cards
    int playerValueOne = playerCardOne.GetValues(); 
	int playerValueTwo = playerCardTwo.GetValues();
	int playerValueThree = playerCardThree.GetValues();
	int playerValue = playerValueOne + playerValueTwo + playerValueThree;

	// Obtains Values From Each Of The Opponent's Cards
    int dealerValueOne = dealerCardOne.GetValues();
	int dealerValueTwo = dealerCardTwo.GetValues();
	int dealerValueThree = dealerCardThree.GetValues();
	int dealerValue = dealerValueOne + dealerValueTwo + dealerValueThree;


// Joker Cards Mechanic Change, If Any Of The Player's or Opponent's Card Are
// A Joker, The Value Of The User That Drew It Will Equal It's Competition's Value 

	if (playerValueOne == 0 || playerValueTwo == 0 || playerValueThree == 0){
		playerValue = dealerValue + playerValue;
	}

	if (dealerValueOne == 0 || dealerValueTwo == 0 || dealerValueThree == 0){
		dealerValue = playerValue + dealerValue;
	}

// Jack Check Similar To Joker Except It Only Work For The Jack Of Hearts 
// Updated To 14 So It Can Be Update Card Effect. Card Equals Value Of Other Two Cards 
	if (playerValueOne == 14 || playerValueTwo == 14 || playerValueThree == 14){
		playerValue = 2 * (playerValue - 14); 
	}

	if (dealerValueOne == 14 || dealerValueTwo == 14 || dealerValueThree == 14){
		dealerValue = 2 * (dealerValue  - 14);
	}

// Displays Cards During Comparsion (Console.WriteLine Comments Used For Testing Purposes)
    Console.Clear();
    Console.WriteLine("\nOpponent's Card");
    RenderCard(dealerCardOne);
	Console.WriteLine($"{dealerValueOne}");
	RenderCard(dealerCardTwo);
	Console.WriteLine($"{dealerValueTwo}");
	RenderCard(dealerCardThree);
	Console.WriteLine($"{dealerValueThree}");
	Console.WriteLine($"{dealerValue}");



    Console.Write("\nPlayer's Card");
    RenderCard(playerCardOne); 
	Console.WriteLine($"{playerValueOne}");
	RenderCard(playerCardTwo);
	Console.WriteLine($"{playerValueTwo}");
	RenderCard(playerCardThree);
	Console.WriteLine($"{playerValueThree}");
	Console.WriteLine($"{playerValue}");


// Value Comparsion Of Card Pulled In Comparsion To User and Opponent
// Increment Score Based On Who Has The Lowest Value 

 if (playerValue < dealerValue){
    Console.WriteLine("\nPlayer Win");
    playerOne++;
 } else if (playerValue > dealerValue){
    Console.WriteLine("\nPlayer Lose");
    playerTwo++;
 } else if (playerValue == dealerValue){
    Console.WriteLine("\nDraw");
    draws++;
 }

 // Score Display At The End Of The Round
 Console.WriteLine($"\nPlayer Wins: {playerOne}");
 Console.WriteLine($"\nOppoonent: {playerTwo}");
 Console.WriteLine($"\nDraws: {draws}");

// Used Card Added To Used Card Deck
 usedCards.Add(dealerCardOne);
 usedCards.Add(dealerCardTwo);
 usedCards.Add(dealerCardThree);
 
 usedCards.Add(playerCardOne);
 usedCards.Add(playerCardTwo);
 usedCards.Add(playerCardThree);


 Console.WriteLine("\nPress Enter For Next Compare");
 Console.ReadLine();
}

// Display Each Of The Cards Drawn
void RenderCard(Card card){
    string[] renderedCard = card.Render();
    foreach (var line in renderedCard){
        Console.WriteLine(line);
    }
}

// Starter Deck Called 
StarterDeck();

// Deck Is Shuffled 
Shuffle(deck);

dealerHand = new List<Card>();

// Number Of Rounds 
int rounds = 26;
for(i = 0; i < rounds; i++){
    cardComparsion();
}


// Final Screen
string gameContinue;
string updateRounds;

// Function Used To Continue Game After Completion (Error Where After Restarting The First. Quits Game Must Be Fixed)
void gameEnd(){
Console.Clear();
Console.WriteLine("\nFinal Results");
Console.WriteLine($"\nPlayer Wins: {playerOne}");
Console.WriteLine($"\nOppoonent: {playerTwo}");
Console.WriteLine($"\nDraws: {draws}");

// Asks The User If They Would Like To Continue 
Console.WriteLine("Would you like to keep playing? Type: Yes or No?" );
gameContinue = Console.ReadLine();

// Asks The User How Many More Rounds They Wish To Play
Console.WriteLine("How many more rounds?" );

// Takes Number Typed By User, Converts It To Int 
updateRounds = Console.ReadLine();
rounds = int.Parse(updateRounds);

}
 gameEnd(); // Function Call For Game End


// Checking If The Game Continues Or Another 
if(gameContinue == "Yes"){
	Console.WriteLine("Game Continue");

// Updated Rounds Based On User Input
	for(i = 0; i < rounds; i++){
    cardComparsion();
}
 gameEnd();

} else {
	Console.WriteLine("Game End");
}


// Class For Cards
class Card
{
	public Suit Suit;  // Suit References 
	public Value Value; // Value References 
    
    public int GetValues(){  // Get Value Classs Added To Obtain Value

	// Different Suit Effect Setup 

	// If Card Is A Suit Of Spade, Then Their Values Will Be Mutliplied -1
    if(Suit == Suit.Spades){
		  return(int) Value * -1;
	}

	// If Card Is A Suit Of Diamond, Then Their Values Will Be Mutliplied 2
	else if(Suit == Suit.Diamonds){
		  return(int) Value * 2;

	// If Card Is A Suit Of Clubs, Then Their Values Will Added By 10 
	} else if (Suit == Suit.Clubs){
		return (int) Value + 10;

	// If The Card Is A Queen Of Hearts, Then It's Value Will Be A Minus 2 Card 
	} else if(Suit == Suit.Hearts && Value == Value.Queen){
			return (int) Value - 14;

    // If The Card Is A King Of Hearts, Then It's Value Will Be A Plus 2 Card 
	}else if(Suit == Suit.Hearts && Value == Value.King){
			return (int) Value - 12 + 2;

	// If The Card Is A Jack Of Hearts, Then It Will Be Updated To 14 
	}else if(Suit == Suit.Hearts && Value == Value.Jack){
			return (int) Value + 3;
	} else
		return(int) Value;
    }

	public const int RenderHeight = 7;
	public string[] Render()
	{
		char suit = Suit.ToString()[0];
		string value = Value switch
		{
			Value.Ace   =>  "A",
			Value.Ten   => "10",
			Value.Jack  =>  "11", // Changed Jack To 11 

			Value.Queen =>  "Q",  
			Value.King  =>  "K",
			Value.Joker => "J",  // Added Value J As Joker 
			_ => ((int)Value).ToString(CultureInfo.InvariantCulture),
		};
		string card = $"{value}{suit}";
		string a = card.Length < 3 ? $"{card} " : card;
		string b = card.Length < 3 ? $" {card}" : card;
		return
		[
			$"┌───────┐",
			$"│{a}    │",
			$"│       │",
			$"│       │",
			$"│       │",
			$"│    {b}│",
			$"└───────┘",
		];
	}
}

// Different Suit Values
enum Suit
{
	Hearts,
	Clubs,
	Spades,
	Diamonds,
}

// Different Card Values
enum Value
{
	Ace   = 01,
	Two   = 02,
	Three = 03,
	Four  = 04,
	Five  = 05,
	Six   = 06,
	Seven = 07,
	Eight = 08,
	Nine  = 09,
	Ten   = 10,
	Jack  = 11,
	Queen = 12,
	King  = 13,

	Joker = 00,
}
