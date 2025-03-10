using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Security;


// Louis Arriaza Erazo
// List Of Deck, Card Discarded, Cards In Hand, Card Dealt By Dealer

// int [] Cards = {1,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5
// 6,6,6,6,6,6,6,6,6,7,7,7,7,7,7,7,7,7,8,8,8,8,8,8,8,8,8,9,9,9,9,9,9,9,9,9,10,10,10,10,10,10,10,10,10
// 11,12,13}
// int [] Cards = {};
// int [] UsedCards = {};

List <Card> deck;
List <Card> usedCards;
List <Card> dealerHand;

int playerOne = 0;
int playerTwo = 0;
int draws = 0;
int i = 0;

// The Initial Deck With All Cards 
void StarterDeck(){
    deck = new List<Card>();
    usedCards = new List<Card>();
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

// Function Where Used Cards Are Added To Discard Deck
void discardCards(){
    deck.AddRange(usedCards);
    usedCards.Clear();
    Shuffle(deck);
}

// Draws A Card and Removes It From Deck
Card DrawCard()
{
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
    var dealerCardOne = DrawCard();
	var dealerCardTwo = DrawCard();
	var dealerCardThree = DrawCard();
    RenderCard(dealerCardOne);
	RenderCard(dealerCardTwo);
	RenderCard(dealerCardThree);
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
    var playerCardOne = DrawCard();
	var playerCardTwo = DrawCard();
	var playerCardThree = DrawCard();
    RenderCard(playerCardOne);
	RenderCard(playerCardTwo);
	RenderCard(playerCardThree);

    Console.WriteLine("\nPress Enter For Results");
    Console.ReadLine();
    
    // Obtaining Values 
    int playerValueOne = playerCardOne.GetValues(); 
	int playerValueTwo = playerCardTwo.GetValues();
	int playerValueThree = playerCardThree.GetValues();
	int playerValue = playerValueOne + playerValueTwo + playerValueThree;

	

    int dealerValueOne = dealerCardOne.GetValues();
	int dealerValueTwo = dealerCardTwo.GetValues();
	int dealerValueThree = dealerCardThree.GetValues();
	int dealerValue = dealerValueOne + dealerValueTwo + dealerValueThree;

	if (playerValueOne == 0 || playerValueTwo == 0 || playerValueThree == 0){
		playerValue = dealerValue + playerValue;
	}

	if (dealerValueOne == 0 || dealerValueTwo == 0 || dealerValueThree == 0){
		playerValue = dealerValue + playerValue;
	}


    Console.Clear();
    Console.WriteLine("\nOpponent's Card");
    RenderCard(dealerCardOne);
	RenderCard(dealerCardTwo);
	RenderCard(dealerCardThree);



    Console.Write("\nPlayer's Card");
    RenderCard(playerCardOne); 
	RenderCard(playerCardTwo);
	RenderCard(playerCardThree);


// Value Comparsion Of Card Pulled 
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

// Used Card Added To used after being used based on player 
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
int rounds = 10;
for(i = 0; i < rounds; i++){
    cardComparsion();
}


// Final Screen
string gameContinue;
string updateRounds;
void gameEnd(){
Console.Clear();
Console.WriteLine("\nFinal Results");
Console.WriteLine($"\nPlayer Wins: {playerOne}");
Console.WriteLine($"\nOppoonent: {playerTwo}");
Console.WriteLine($"\nDraws: {draws}");

// Asking If You Would Like To Continue 
Console.WriteLine("Would you like to keep playing? Type: Yes or No?" );
gameContinue = Console.ReadLine();

// Number Of Rounds 
Console.WriteLine("How many more rounds?" );
updateRounds = Console.ReadLine();
rounds = int.Parse(updateRounds);
}
 gameEnd();

// Checking If The Game Continues Or Another 
if(gameContinue == "Yes"){
	Console.WriteLine("Game Continue");
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
	public Suit Suit;
	public Value Value;
    
    public int GetValues(){  // Get Value Classs Added To Obtain Value
    if(Suit == Suit.Spades){
		  return(int) Value * -1;
	}
	else if(Suit == Suit.Diamonds){
		  return(int) Value * -2;
	} else if (Suit == Suit.Clubs){
		return (int) Value + 10;
	} else if(Suit == Suit.Hearts && Value == Value.Queen){
			return (int) Value - 14;
			
	}else if(Suit == Suit.Hearts && Value == Value.King){
			return (int) Value - 12 + 2;
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
			Value.Jack  =>  "11",

			Value.Queen =>  "Q",  // Updated To Represent Number Value
			Value.King  =>  "K",
			Value.Joker => "J",
			_ => ((int)Value).ToString(CultureInfo.InvariantCulture),
		};
		string card = $"{value}{suit}";
		string a = card.Length < 3 ? $"{card} " : card;
		string b = card.Length < 3 ? $" {card}" : card;
		return
		[
			$"┌───────┐",
			$"│{a}     │",
			$"│       │",
			$"│       │",
			$"│       │",
			$"│    {b} │",
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
