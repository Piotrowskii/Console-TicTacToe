using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolko_i_Krzyzyk___console__
{
	internal class MainMenu
	{
		private string[] Art { get; set; }

		public MainMenu()
		{
			// 1 - kolko i krzyzyk
			// 2 - dowidzenia
			// 3 - multiplayer 
			Art = new string[3] { "  _  __     _ _           _   _  __                         _    \r\n | |/ /    | | |         (_) | |/ /                        | |   \r\n | ' / ___ | | | _____    _  | ' / _ __ _____   _ _____   _| | __\r\n |  < / _ \\| | |/ / _ \\  | | |  < | '__|_  / | | |_  / | | | |/ /\r\n | . \\ (_) | |   < (_) | | | | . \\| |   / /| |_| |/ /| |_| |   < \r\n |_|\\_\\___/|_|_|\\_\\___/  |_| |_|\\_\\_|  /___|\\__, /___|\\__, |_|\\_\\\r\n                                             __/ |     __/ |     \r\n                                            |___/     |___/      ",
									  "  _____                   _     _               _       \r\n |  __ \\                 (_)   | |             (_)      \r\n | |  | | ___   __      ___  __| |_______ _ __  _  __ _ \r\n | |  | |/ _ \\  \\ \\ /\\ / / |/ _` |_  / _ \\ '_ \\| |/ _` |\r\n | |__| | (_) |  \\ V  V /| | (_| |/ /  __/ | | | | (_| |\r\n |_____/ \\___/    \\_/\\_/ |_|\\__,_/___\\___|_| |_|_|\\__,_|\r\n                                                        \r\n                                                        ",
									  "         __  __       _ _   _       _                       \r\n        |  \\/  |     | | | (_)     | |                      \r\n        | \\  / |_   _| | |_ _ _ __ | | __ _ _   _  ___ _ __ \r\n        | |\\/| | | | | | __| | '_ \\| |/ _` | | | |/ _ \\ '__|\r\n        | |  | | |_| | | |_| | |_) | | (_| | |_| |  __/ |   \r\n        |_|  |_|\\__,_|_|\\__|_| .__/|_|\\__,_|\\__, |\\___|_|   \r\n                             | |             __/ |          \r\n                             |_|            |___/           "};
		}


		public void DrawMenu(int highlighted)
		{
			Console.Clear();

			string[] options = { "\t\t  1. Tryb Jednego gracza", "\t\t  2. Kooperacja Lokalna", "\t\t  3. Wyjdź " };


			Console.WriteLine("\n\n\n");
			Console.WriteLine(Art[0]);
			Console.WriteLine("\n");

			for (int i = 0; i < options.Length; i++)
			{
				if (i == highlighted)
				{
					Console.BackgroundColor = ConsoleColor.White;
					Console.ForegroundColor = ConsoleColor.Black;
					Console.WriteLine(options[i]);
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine(options[i]);
				}
			}

			Console.WriteLine("\n");
			Console.WriteLine("\tZmień opcje strzałkami, kliknij <Enter> Aby wybrać");



		}

		public void DrawMultiplayerMenu(int highlighted)
		{
			Console.Clear();

			string[] options = { "Stwórz gre", "Dołącz do gry" };

			Console.WriteLine("\n\n\n");
			Console.WriteLine(Art[2]);
			Console.WriteLine("\n");

			for (int i = 0; i < options.Length; i++)
			{
				if (i == 0)
				{
					Console.SetCursorPosition(18, 15);
				}
				else
				{
					Console.SetCursorPosition(33, 15);
				}




				if (i == highlighted)
				{
					Console.BackgroundColor = ConsoleColor.White;
					Console.ForegroundColor = ConsoleColor.Black;
					Console.WriteLine(options[i]);
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine(options[i]);
				}
			}

		}

		public void DrawEndScreen()
		{
			Console.Clear();
			Console.WriteLine("\n\n\n" + Art[1]);
		}

	}
}
