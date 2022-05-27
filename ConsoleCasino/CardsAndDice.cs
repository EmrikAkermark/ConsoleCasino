using System;
using System.Collections.Generic;

namespace ConsoleCasino
{
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

		public static string GetCardValueInString(Card card)
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

		public static List<Card> CreateDeckOfCards(int noOfDecks = 1, bool shuffled = true)
		{
			if (noOfDecks < 1)
			{
				return null;
			}
			List<Card> deck = new List<Card>();
			int currentSuit;
			int currentValue;
			Card card = new Card();
			for (int i = 0; i < 4; i++)
			{
				currentSuit = i;
				for (int j = 1; j < 14; j++)
				{
					currentValue = j;
					card = new Card(currentSuit, currentValue);
					deck.Add(card);
				}
			}

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
					int random2 = random.Next() % i;
					int randomIndex = indexes[random2];
					indexes.RemoveAt(random2);
					shuffledDeck.Add(deck[randomIndex]);
				}
				shuffledDeck.Add(deck[indexes[0]]);

				return shuffledDeck;

			}

			return deck;
		}

		public static void AddCardsToHand(ref List<Card> deck, ref List<Card> hand, int numberOfCards)
		{
			while (numberOfCards > 0)
			{
				hand.Add(deck[0]);
				deck.RemoveAt(0);
				numberOfCards--;
			}
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
