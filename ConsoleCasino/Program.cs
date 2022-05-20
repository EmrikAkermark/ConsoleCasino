using System;
using System.Collections.Generic;

namespace ConsoleCasino
{
	class Program
	{
		public struct PlayerProfile
		{
			public int Money;
			public String Name;
			public PlayerProfile(int money, string name)
			{
				Name = name;
				Money = money;
			}

		}

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}
	}


	class BlackJack
	{
		static Program.PlayerProfile Run(Program.PlayerProfile pProfile)
		{

			bool isPlaying = true;

			while(isPlaying)
			{
				//int bet = player input < pProfile Money
				bool handIsActive = true;
				while(handIsActive)
				{
					List<CardsAndDice.Card> deck = CardsAndDice.CreateDeckOfCards();
					List<CardsAndDice.Card> playerHand = new List<CardsAndDice.Card>();
					List<CardsAndDice.Card> dealerHand = new List<CardsAndDice.Card>();

					CardsAndDice.AddCardsToHand(ref deck, ref dealerHand, 2);
					CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 2);

					//Show dealer first number

					//Show your own numbers

					bool playerSelecting = true;
					bool playerBust = false;

					int playerHandValue = CheckHandValue(playerHand);

					if(playerHandValue == 21)
						//Blackjack

					//Ask hit, stay

					while (playerSelecting)
					{
						int selection = 0;
						switch (selection)
						{
							//Stay
							case 1:
								playerSelecting = false;
								break;
							//Hit
							case 2:
								CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 1);
								break;
							//Double Down
							case 3:
								CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 1);
								playerSelecting = false;
								break;
							default:
								//Try again
								break;
						}
						if(CheckHandValue(playerHand) > 21)
						{
							playerBust = true;
							playerSelecting = false;
						}
					}

					if(playerBust)
					{
						//Lose money
						continue;
					}
					int dealerHandValue = CheckHandValue(dealerHand);
					if (dealerHandValue == 21)
					{
						//black jack
					}

					bool dealerSelecting = true;
					while(dealerSelecting)
					{
						if(dealerHandValue > 21)
						{
							//bust
							dealerSelecting = false;
						}
						else if(dealerHandValue < 17)
						{
							CardsAndDice.AddCardsToHand(ref deck, ref dealerHand, 1);
						}
						else
						{
							dealerSelecting = false;
						}
					}

					if(playerHandValue == dealerHandValue)
					{
						//tie
					}
					else if(playerHandValue > dealerHandValue)
					{
						//Player wins
					}
					else
					{
						//Dealer wins
					}

				}
			}

			return pProfile;
		}

		static int CheckHandValue(List<CardsAndDice.Card> hand)
		{
			int handValue = 0;
			int aces = 0;
			for (int i = 0; i < hand.Count; i++)
			{
				if(hand[i].Value > 9)
				{
					handValue += 10;
				}
				else if(hand[i].Value == 1)
				{
					aces++;
				}
				else
				{
					handValue += hand[i].Value;
				}
			}

			if(handValue < 11 && aces > 0)
			{
				if(aces == 1)
				{
					handValue += 11;
				}
				else if(handValue + 10 + aces < 22)
				{
					handValue += 10;
					handValue += aces;
				}
				else
				{
					handValue += aces;
				}
			}
			else
			{
				handValue += aces;
			}

			return handValue;
		}

	}


	class CardsAndDice
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

		public static List<Card> CreateDeckOfCards(int noOfDecks = 1, bool shuffled = true)
		{
			if(noOfDecks < 1)
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

			if(shuffled)
			{
				List<Card> shuffledDeck = new List<Card>();
				List<int> indexes = new List<int>();
				for (int i = 0; i < deck.Count; i++)
				{
					indexes.Add(i);
				}
				Random random = new Random();
				for (int i = 0; i < deck.Count; i++)
				{

					int randomIndex = indexes[random.Next() % indexes.Count];
					indexes.RemoveAt(randomIndex);
					shuffledDeck.Add(deck[randomIndex]);
				}
				return shuffledDeck;

			}

			return deck;
		}

		public static void AddCardsToHand(ref List<Card> deck, ref List<Card> hand, int numberOfCards)
		{
			while(numberOfCards > 0)
			{
				hand.Add(deck[0]);
				deck.RemoveAt(0);
			}
		}
	}
}
