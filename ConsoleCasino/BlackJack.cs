using System;
using System.Collections.Generic;

namespace ConsoleCasino
{
	public class BlackJack
	{
		public static Utilities.PlayerProfile Run(Utilities.PlayerProfile pProfile)
		{

			Console.WriteLine("Hello " + pProfile.Name + " and welcome to BlackJack!");
			Console.WriteLine("Dealer stands on 17");

			bool isPlaying = true;

			while (isPlaying)
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

				Console.WriteLine("Your hand is " + GetHandValueInString(playerHand));
				Console.WriteLine("Your value is " + playerHandValue);
				if (playerHandValue == 21)
				{
					Console.WriteLine("You have BlackJack!");
					playerBlackJack = true;
				}
				else
				{

					while (playerSelecting)
					{

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
								if (bet * 2 < pProfile.Money)
								{
									bet += bet;
								}
								else
								{
									bet = pProfile.Money;
								}

								CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 1);
								playerHandValue = CheckHandValue(playerHand);
								playerSelecting = false;
								break;
							default:
								Console.WriteLine("That's not right");
								break;
						}
						Console.WriteLine("Your hand is " + GetHandValueInString(playerHand));
						Console.WriteLine("Your value is " + playerHandValue);
						if (CheckHandValue(playerHand) > 21)
						{
							playerBust = true;
							playerSelecting = false;
						}
					}
				}
				if (playerBust)
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
						pProfile.Money += bet * 2;
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

				if (pProfile.Money == 0)
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
				if (hand[i].Value > 9)
				{
					handValue += 10;
				}
				else if (hand[i].Value == 1)
				{
					aces++;
				}
				else
				{
					handValue += hand[i].Value;
				}
			}

			if (handValue < 11 && aces > 0)
			{
				if (aces == 1)
				{
					handValue += 11;
				}
				else if (handValue + 10 + aces < 22)
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

	}
}
