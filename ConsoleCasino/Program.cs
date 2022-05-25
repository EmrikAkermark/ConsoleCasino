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
			
			
			Console.WriteLine("Welcome to the console casino!");
			Console.Write("Please state your name: ");
			string name = Console.ReadLine();

			Console.Write("And your starting funds: ");
			int startingMoney;
			int.TryParse(Console.ReadLine(), out startingMoney);

			PlayerProfile pProfile = new PlayerProfile(startingMoney, name);

			bool hasMoney = true;
			bool wantsToPlay = true;
			while (hasMoney && wantsToPlay)
			{
				int selection = 0;

				Console.WriteLine("You can go to (1)BlackJack, (2)Odd or Even, and (3)Leave");

				int.TryParse(Console.ReadLine(), out selection);
				switch (selection)
				{
					case 1:
						pProfile = BlackJack.Run(pProfile);
						break;
					case 2:
						pProfile = EvenAndOdd.Run(pProfile);
						break;
					case 3:
						Console.WriteLine("Come back soon! Press any key to leave");
						wantsToPlay = false;
						break;
					default:
						Console.WriteLine("Sorry, I don't understand");
						break;
				}
				if(pProfile.Money <= 0)
				{
					hasMoney = false;
				}
			}
			if(!hasMoney)
			{
				Console.WriteLine("Get out of here " + pProfile.Name + ", and don't come back until you have some ca$h!");
			}


		}
	}


	class BlackJack
	{
		public static Program.PlayerProfile Run(Program.PlayerProfile pProfile)
		{

			Console.WriteLine("Hello " + pProfile.Name + " and welcome to BlackJack!");

			bool isPlaying = true;

			while(isPlaying)
			{
				int bet = Utilities.BuyChips(pProfile.Money);
				
				List<CardsAndDice.Card> deck = CardsAndDice.CreateDeckOfCards();
				List<CardsAndDice.Card> playerHand = new List<CardsAndDice.Card>();
				List<CardsAndDice.Card> dealerHand = new List<CardsAndDice.Card>();

				CardsAndDice.AddCardsToHand(ref deck, ref dealerHand, 2);
				CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 2);



				//Show dealer first number
				string dealerFirstCard = CardsAndDice.GetCardValueInString(dealerHand[0]);

				Console.WriteLine("Dealer face card is the " + dealerFirstCard);

				string playerFirstCard = CardsAndDice.GetCardValueInString(playerHand[0]);
				string playerSecondCard = CardsAndDice.GetCardValueInString(playerHand[1]);
					
				//Show your own number
				bool playerSelecting = true;
				bool playerBust = false;
				bool dealerBust = false;
				bool playerBlackJack = false;
				bool dealerBlackJack = false;

				int playerHandValue = CheckHandValue(playerHand);

				if(playerHandValue == 21)
				{
					Console.WriteLine("Your hand is " + GetHandValueInString(playerHand));
					Console.WriteLine("Your value is " + CheckHandValue(playerHand));
					Console.WriteLine("You have BlackJack!");
					playerBlackJack = true;
				}
				else
				{
					
					while (playerSelecting)
					{
						Console.WriteLine("Your hand is " + GetHandValueInString(playerHand));
						Console.WriteLine("Your value is " + playerHandValue);
						Console.WriteLine("Do you (1)Stay, (2)Hit, (3)Double Down");
						int selection = 0;
							
						int.TryParse(Console.ReadLine(), out selection);
						switch (selection)
						{
							//Stay
							case 1:
								playerSelecting = false;
								break;
							//Hit
							case 2:
								CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 1);
								playerHandValue = CheckHandValue(playerHand);
								break;
							//Double Down
							case 3:
								if(bet * 2 < pProfile.Money)
								{
									bet += bet;
								}
								else
								{
									bet = pProfile.Money;
								}

								CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 1);
								playerHandValue = CheckHandValue(playerHand);
								Console.WriteLine("Your hand is " + GetHandValueInString(playerHand));
								Console.WriteLine("Your value is " + playerHandValue);
								playerSelecting = false;
								break;
							default:
								Console.WriteLine("That's not right");
								break;
						}
						if (CheckHandValue(playerHand) > 21)
						{
							playerBust = true;
							playerSelecting = false;
						}
					}
				}
				if(playerBust)
				{
					Console.WriteLine("You're bust!");
					pProfile.Money -= bet;
				}
				else
				{
					int dealerHandValue = CheckHandValue(dealerHand);
					if (dealerHandValue == 21)
					{
						dealerBlackJack = true;
					}
					else
					{
						bool dealerSelecting = true;
						while (dealerSelecting)
						{
							if (dealerHandValue > 21)
							{
								dealerSelecting = false;
								dealerBust = true;
							}
							else if (dealerHandValue < 17)
							{
								CardsAndDice.AddCardsToHand(ref deck, ref dealerHand, 1);
								dealerHandValue = CheckHandValue(dealerHand);
							}
							else
							{
								dealerSelecting = false;
							}
						}
					}

					

					string allDealerCards = "The dealers hand is " + GetHandValueInString(dealerHand);
					Console.WriteLine(allDealerCards);
					Console.WriteLine("The dealers value is " + dealerHandValue);

					if (dealerBust)
					{
						pProfile.Money += bet;
						Console.WriteLine("Dealer went bust! You won " + bet);
					}
					else if (playerBlackJack && !dealerBlackJack)
					{
						pProfile.Money +=  bet * 2;
						Console.WriteLine("You won " + bet * 2);
					}
					else if (!playerBlackJack && dealerBlackJack)
					{
						pProfile.Money -= bet;
						Console.WriteLine("You lost " + bet);
					}
					else if (playerHandValue == dealerHandValue)
					{
						Console.WriteLine("Draw, money back");
					}
					else if (playerHandValue > dealerHandValue)
					{
						pProfile.Money += bet;
						Console.WriteLine("You won " + bet);
					}
					else
					{
						pProfile.Money -= bet;
						Console.WriteLine("You lost " + bet);
					}
				}
				
				if(pProfile.Money == 0)
				{
					Console.WriteLine("You are out of cash, and out of here!");
					isPlaying = false;
				}
				else
				{
					Console.WriteLine("Try another hand?");
					isPlaying = Utilities.GetConfirmation();
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

		static string GetHandValueInString(List<CardsAndDice.Card> hand)
		{
			string handInString = "the ";
			for (int i = 0; i < hand.Count; i++)
			{
				handInString += CardsAndDice.GetCardValueInString(hand[i]);
				if (i != hand.Count - 1)
				{
					handInString += " and the ";
				}
			}
			return handInString;
		}

	}

	class EvenAndOdd
	{
		static public Program.PlayerProfile Run(Program.PlayerProfile pProfile)
		{
			Console.WriteLine("Hello " + pProfile.Name + " and welcome to Even and Odd, or Chō-han!");

			bool isPlaying = true;

			while(isPlaying)
			{
				int bet = Utilities.BuyChips(pProfile.Money);

				List<int> thrownDice = CardsAndDice.DiceThrow(2);
				int result = 0;
				for (int i = 0; i < thrownDice.Count; i++)
				{
					result += thrownDice[i];
				}

				Console.WriteLine("A pair of dice has been thrown");

				Console.WriteLine("Is the result (1)even or (2)odd?");
				int selection = 0;
				bool isSelecting = true;
				bool won = false;
				while (isSelecting)
				{
					int.TryParse(Console.ReadLine(), out selection);
					switch (selection)
					{
						case 1:
							if(result %2 == 0)
							{
								won = true;
							}
							isSelecting = false;
							break;
						case 2:
							if (result % 2 == 1)
							{
								won = true;
							}
							isSelecting = false;
							break;
						default:
							Console.WriteLine("That's not right");
							break;
					}
				}
				Console.WriteLine("The dice values are " + thrownDice[0] + " and " + thrownDice[1]);
				Console.WriteLine("The result is " + result);
				if(won)
				{
					pProfile.Money += bet;
					Console.WriteLine("You won!");
				}
				else
				{
					pProfile.Money -= bet;
					Console.WriteLine("You lost!");
				}

				if(pProfile.Money > 0)
				{
					Console.WriteLine("Wanna try again?");
					isPlaying = Utilities.GetConfirmation();
				}
				else
				{
					Console.WriteLine("Thank you for giving us all your money!");
					isPlaying = false;
				}

			}

			return pProfile;
		}
	}

	class Utilities
	{
		static public int BuyChips(int availableMoney)
		{
			bool insertingMoney = true;
			Console.WriteLine("You have " + availableMoney);
			Console.WriteLine("Insert bet money");
			int betMoney = 0;
			while (insertingMoney)
			{
				
				int.TryParse(Console.ReadLine(), out betMoney);

				if(betMoney > availableMoney)
				{
					Console.WriteLine("This isn't a bank");
				}
				else if(betMoney == 0)
				{
					Console.WriteLine("This isn't practice");
				}
				else if( betMoney < 0)
				{
					Console.WriteLine("How does this even work?");
				}
				else
				{
					insertingMoney = false;
				}
			}
			return betMoney;
		}

		static public bool GetConfirmation()
		{
			bool waitingForAnswer = true;
			while (waitingForAnswer)
			{
				Console.WriteLine("(y)es/(n)o");
				string answer = Console.ReadLine().ToLower();
				if (answer == "y" || answer == "yes")
					return true;
				else if (answer == "n" || answer == "no")
					return false;
				else
					Console.WriteLine("Input was not correct, please type (y)es or (n)o");
			}
			return false;
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

		public static string GetCardValueInString(Card card)
		{

			string cardString = "";

			if(card.Value == 1 && card.Suit == 3)
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
			switch(card.Suit)
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
				for (int i = deck.Count -1; i >0; i--)
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
			while(numberOfCards > 0)
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
