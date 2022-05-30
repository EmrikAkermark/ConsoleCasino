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

			List<CardsAndDice.Card> deck = CardsAndDice.CreateDeckOfCards(2);
			List<CardsAndDice.Card> playerHand = new List<CardsAndDice.Card>();
			List<CardsAndDice.Card> dealerHand = new List<CardsAndDice.Card>();

			int deckSize = deck.Count;
			while (isPlaying)
			{
				int bet = Utilities.BuyChips(pProfile.Money);
				//Follows blackjack customs of shuffling after half the cards have been used
				if(deck.Count < deckSize / 2)
				{
					deck = CardsAndDice.CreateDeckOfCards(2);
				}

				CardsAndDice.AddCardsToHand(ref deck, ref dealerHand, 2);
				CardsAndDice.AddCardsToHand(ref deck, ref playerHand, 2);

				Console.WriteLine("Dealer face card is the " + CardsAndDice.GetCardInString(dealerHand[0]));

				bool playerSelecting = true;
				bool playerBust = false;
				bool dealerBust = false;
				bool playerBlackJack = false;
				bool dealerBlackJack = false;

				int playerHandValue = CheckHandValue(playerHand);

				Console.WriteLine("Your hand is " + CardsAndDice.GetHandInString(playerHand));
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

						Console.WriteLine("Do you (1)Stay, (2)Hit, (3)Double Down?");
						int selection;

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

						Console.WriteLine("Your hand is " + CardsAndDice.GetHandInString(playerHand));
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

					Console.WriteLine("The dealers hand is " + CardsAndDice.GetHandInString(dealerHand));
					Console.WriteLine("The dealers value is " + dealerHandValue);

					if (playerBlackJack && !dealerBlackJack)
					{
						pProfile.Money += bet * 2;
						Console.WriteLine("You won " + bet * 2);
					}
					else if (!playerBlackJack && dealerBlackJack)
					{
						pProfile.Money -= bet;
						Console.WriteLine("You lost " + bet);
					}
					else if (dealerBust)
					{
						pProfile.Money += bet;
						Console.WriteLine("Dealer went bust! You won " + bet);
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

		// This hand value check is not in Cards and Dice since it's only relevant to blackjack.
		static int CheckHandValue(List<CardsAndDice.Card> hand)
		{
			int handValue = 0;
			int aces = 0;
			for (int i = 0; i < hand.Count; i++)
			{
				//Adds the value of face cards and the 10 values.
				if (hand[i].Value > 9)
				{
					handValue += 10;
				}
				//Aces gets handled later due to fun black jack rules
				else if (hand[i].Value == 1)
				{
					aces++;
				}
				else
				{
					handValue += hand[i].Value;
				}
			}
			// This checks if hand is small enough to give the aces
			// the value 11.
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
			// This will trigger even when no aces are present, but adding 0
			// is essentially the same as skipping it so checking for that is just
			// adding needless complications to the code
			else
			{
				handValue += aces;
			}

			return handValue;
		}
	}
}
