using System;
using System.Collections.Generic;

namespace ConsoleCasino
{
	// Cards and dice are supposed to handle any generic methods and structs involving cards and dice
	public class CardsAndDice
	{
		public struct Card
		{
			public int Suit;
			public int Value;
			public Card(int suit, int value)
			{
				Suit = suit;
				Value = value;
			}
		}


		// The next two methods aren't complicated, just big.
		// Turning them into methods is not just for cleaner code when reading,
		// but making sure that using their functionality becomes as easy as possible
		// when writing. 

		public static string GetCardInString(Card card)
		{

			string cardString = "";

			if (card.Value == 1 && card.Suit == 3)
			{
				cardString = "♠ACE OF SPADES♠";
				return cardString;
			}

			switch (card.Value)
			{
				case 1:
					cardString += "Ace ";
					break;
				case 11:
					cardString += "Jack ";
					break;
				case 12:
					cardString += "Queen ";
					break;
				case 13:
					cardString += "King ";
					break;
				default:
					cardString += card.Value.ToString() + " ";
					break;
			}
			switch (card.Suit)
			{
				case 0:
					cardString += "of Clubs";
					break;
				case 1:
					cardString += "of Diamonds";
					break;
				case 2:
					cardString += "of Hearts";
					break;
				case 3:
					cardString += "of Spades";
					break;
			}
			return cardString;
		}

		public static string GetHandInString(List<Card> hand)
		{
			string handInString = "the ";
			for (int i = 0; i < hand.Count; i++)
			{
				handInString += GetCardInString(hand[i]);
				if (i < hand.Count - 2)
				{
					handInString += ", the ";
				}
				else if (i != hand.Count - 1)
				{
					handInString += " and the ";
				}
			}
			return handInString;
		}

		// Simple deck making method with shuffling and customizable size
		// 
		public static List<Card> CreateDeckOfCards(int noOfDecks = 1, bool shuffled = true)
		{
			if (noOfDecks < 1)
			{
				return null;
			}
			List<Card> deck = new List<Card>();
			int currentSuit;
			int currentValue;
			Card card;
			for (int i = 0; i < 4 * noOfDecks; i++)
			{
				currentSuit = i % 4;
				for (int j = 1; j < 14; j++)
				{
					currentValue = j;
					card = new Card(currentSuit, currentValue);
					deck.Add(card);
				}
			}
			// The shuffle is a bit funky.
			// We have 2 new lists, the shuffled deck of cards,
			// and the list of indexes of the unshuffled deck.
			//
			// We will pick a random number from the list of indexes,
			// then use that index to pick a card from the unshuffled deck
			// and copy it to the shuffled deck.
			// We then remove that index from the index list.
			//
			// This makes sure every card will be selected from the unshuffled
			// deck, but only once.
			if (shuffled)
			{
				List<Card> shuffledDeck = new List<Card>();
				List<int> indexes = new List<int>();
				for (int i = 0; i < deck.Count; i++)
				{
					indexes.Add(i);
				}
				Random random = new Random();


				for (int i = deck.Count - 1; i > 0; i--)
				{
					int randomIndexForIndexList = random.Next() % i;
					int randomIndexForDeck = indexes[randomIndexForIndexList];
					indexes.RemoveAt(randomIndexForIndexList);
					shuffledDeck.Add(deck[randomIndexForDeck]);
				}
				// Adding the last card manually since "%" doesn't support the value 0
				shuffledDeck.Add(deck[indexes[0]]);

				return shuffledDeck;

			}

			return deck;
		}


		// Moves a set number of cards from the deck to a hand.
		// Uses refs to keep it as simple as possible to implement.
		public static bool AddCardsToHand(ref List<Card> deck, ref List<Card> hand, int numberOfCards)
		{
			if(numberOfCards > deck.Count)
			{
				Console.WriteLine("Attempted to add more cards than exists in deck");

				return false;
			}

			while (numberOfCards > 0)
			{
				hand.Add(deck[0]);
				deck.RemoveAt(0);
				numberOfCards--;
			}
			return true;
		}


		public static List<int> DiceThrow(int numberOfDice = 1)
		{
			List<int> thrownDice = new List<int>();

			Random random = new Random();

			for (int i = 0; i < numberOfDice; i++)
			{
				thrownDice.Add((random.Next() % 6) + 1);
			}

			return thrownDice;
		}

	}
}
