using System;
namespace ConsoleCasino
{
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
