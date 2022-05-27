using System;
namespace ConsoleCasino
{
	public class EvenAndOdd
	{
		static public Program.PlayerProfile Run(Program.PlayerProfile pProfile)
		{
			Console.WriteLine("Hello " + pProfile.Name + " and welcome to Even and Odd, or Chō-han!");

			bool isPlaying = true;

			while (isPlaying)
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
							if (result % 2 == 0)
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
				if (won)
				{
					pProfile.Money += bet;
					Console.WriteLine("You won!");
				}
				else
				{
					pProfile.Money -= bet;
					Console.WriteLine("You lost!");
				}

				if (pProfile.Money > 0)
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
}
