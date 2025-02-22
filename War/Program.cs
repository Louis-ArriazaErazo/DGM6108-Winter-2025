using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

// Group Member:
// Louis Arriaza Erazo
// Tianli Zhou

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

// The Initial Deck With All Cards 
void StarterDeck(){
    deck = new List<Card>();
    usedCards = new List<Card>();
    foreach(Value value in Enum.GetValues(typeof(Value))){
        deck.Add(new Card{Value = value});
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
    var dealerCard = DrawCard();
    RenderCard(dealerCard);
    dealerHand.Add(dealerCard);

    // Confirming Player's Turn
    Console.WriteLine("\nPress Enter To Draw Your Card");
    Console.ReadLine();
    
    // Opponent's Card Drawing (Secondary View)
    Console.Clear();
    Console.WriteLine("\nOpponent's Card");
    RenderCard(dealerCard);

    // Player's Card Drawing 
    Console.Write("\nPlayer's Card");
    var playerCard = DrawCard();
    RenderCard(playerCard);

    Console.WriteLine("\nPress Enter For Results");
    Console.ReadLine();
    
    // Obtaining Values 
    int playerValue = playerCard.GetValues();
    int dealerValue = dealerCard.GetValues();

    Console.Clear();
    Console.WriteLine("\nOpponent's Card");
    RenderCard(dealerCard);

    Console.Write("\nPlayer's Card");
    RenderCard(playerCard);


// Value Comparsion Of Card Pulled 
 if (playerValue > dealerValue){
    Console.WriteLine("\nPlayer Win");
    playerOne++;
 } else if (playerValue < dealerValue){
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
 usedCards.Add(dealerCard);
 usedCards.Add(playerCard);

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
for(int i = 0; i < 26; i++){
    cardComparsion();
}

// Final Screen
Console.Clear();
Console.WriteLine("\nFinal Results");
Console.WriteLine($"\nPlayer Wins: {playerOne}");
Console.WriteLine($"\nOppoonent: {playerTwo}");
Console.WriteLine($"\nDraws: {draws}");

// Class For Cards
class Card
{
	public Value Value;
    
    public int GetValues(){  // Get Value Classs Added To Obtain Value
        return(int) Value;
    }

	public const int RenderHeight = 7;

	public string[] Render()
	{
	
		string value = Value switch
		{
			Value.Ace   =>  "A",
			Value.Ten   => "10",
			Value.Jack  =>  "11",
			Value.Queen =>  "12",  // Updated To Represent Number Value
			Value.King  =>  "13",
			_ => ((int)Value).ToString(CultureInfo.InvariantCulture),
		};
		string card = $"{value}";
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
}
