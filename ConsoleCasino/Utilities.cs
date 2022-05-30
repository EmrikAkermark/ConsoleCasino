using System;
namespace ConsoleCasino
{
	//Utilities are to contain methods and structs that are not directly related
	//to the game part of hazard games.
	public class Utilities
	{
		public struct PlayerProfile
		{
			public int Money;
			public string Name;
			public PlayerProfile(int money, string name)
			{
				Name = name;
				Money = money;
			}

		}


		//Making sure the player inserts a legal amount of money
		//(or makes a legal entry) takes up space, and will be used
		//in every game. So in utilities it goes.
		static public int BuyChips(int availableMoney)
		{
			bool insertingMoney = true;
			Console.WriteLine("You have " + availableMoney);
			Console.WriteLine("Insert bet money");
			int betMoney = 0;
			while (insertingMoney)
			{

				int.TryParse(Console.ReadLine(), out betMoney);

				if (betMoney > availableMoney)
				{
					Console.WriteLine("This isn't a bank");
				}
				else if (betMoney == 0)
				{
					Console.WriteLine("This isn't practice");
				}
				else if (betMoney < 0)
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

		// Asking the player Yes/No is generic enough to use anywhere, and
		// making sure that the player gives a valid response requires some extra lines.
		// This is to make it as easy to not only read when implemented, but also to implement in the first place.
		// I want to avoid not using useful code because inserting it is tedious.
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
}
