using System;

namespace ConsoleCasino
{
	public class Program
	{
		static void Main(string[] args)
		{
			
			
			Console.WriteLine("Welcome to the console casino!");
			Console.Write("Please state your name: ");
			string name = Console.ReadLine();

			Console.Write("And your starting funds: ");
			bool insertingMoney = true;
			int startingMoney = 0;

			//A safeguard to ensure the player inserts actual money
			while(insertingMoney)
			{
				int.TryParse(Console.ReadLine(), out startingMoney);
				if(startingMoney < 0)
				{
					Console.WriteLine("How does that even work?");
				}
				else if(startingMoney == 0)
				{
					Console.WriteLine("Very funny, give us an actual number");
				}
				else
				{
					insertingMoney = false;
				}

			}

			Utilities.PlayerProfile pProfile = new Utilities.PlayerProfile(startingMoney, name);

			bool hasMoney = true;
			bool wantsToPlay = true;
			while (hasMoney && wantsToPlay)
			{
				int selection;

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
						Console.WriteLine("Come back soon!");
						Console.Read();
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
				Console.Read();
			}


		}
	}
}
