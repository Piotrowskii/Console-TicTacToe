using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Kolko_i_Krzyzyk___console__.Program;

namespace Kolko_i_Krzyzyk___console__
{

	internal class Program
	{
		public enum Mark
		{
			Cross,
			Circle,
			None
		}

		public class SoloGame : Game
		{
			public void AiClaimSpace()
			{

				if (GameBoard.Contains(Mark.None))
				{

					SendPrimaryMessage("Oczekiwanie na odpowiedź AI", ConsoleColor.Blue);
					Thread.Sleep(2000);

					NextTurn = PlayerMark;
					TurnNumber++;

					Mark[] testBoard = GameBoard;

					//sprawdanie czy może wygrać
					for (int i = 0; i < testBoard.Length; i++)
					{
						if (testBoard[i] == Mark.None)
						{
							testBoard[i] = EnemyMark;
							if (CheckWin(testBoard) == EnemyMark)
							{
								GameBoard[i] = EnemyMark;
								UpdateGameInterface();
								return;
							}
							testBoard[i] = Mark.None;
						}
					}

					//sprawdzanie czy może zablokować
					for (int i = 0; i < testBoard.Length; i++)
					{
						if (testBoard[i] == Mark.None)
						{
							testBoard[i] = PlayerMark;
							if (CheckWin(testBoard) == PlayerMark)
							{
								GameBoard[i] = EnemyMark;
								UpdateGameInterface();
								return;
							}
							testBoard[i] = Mark.None;
						}

					}

					//losowe miejsce
					Random rng = new Random();
					int losowa;
					while (true)
					{
						losowa = rng.Next(GameBoard.Length);
						if (GameBoard[losowa] == Mark.None)
						{
							GameBoard[losowa] = EnemyMark;
							UpdateGameInterface();
							return;
						}
					}
				}
			}

			public override void GetWinningMessage()
			{
				if (Winner == Mark.None)
				{
					SendSecondaryMessage("Niestety remis :/", ConsoleColor.Gray, false);
				}
				else if (Winner == Mark.Cross)
				{
					SendSecondaryMessage("Gratulacje wygrałeś :)", ConsoleColor.Green, false);
				}
				else if (Winner == Mark.Circle)
				{
					SendSecondaryMessage("Porażka przegrałeś :(", ConsoleColor.Red, false);
				}
			}
		}

		public class LocalGame : Game
		{
			public override void GetWinningMessage()
			{
				if (Winner == Mark.None)
				{
					SendSecondaryMessage("Niestety remis :/", ConsoleColor.Gray, false);
				}
				else if (Winner == Mark.Cross)
				{
					SendSecondaryMessage("Krzyżyk wygrał :)", ConsoleColor.Red, false);
				}
				else if (Winner == Mark.Circle)
				{
					SendSecondaryMessage("Kółko wygrało :)", ConsoleColor.Blue, false);
				}
			}
		}


		static void Main(string[] args)
		{

			MainMenu mainMenu = new MainMenu();

			bool ConsoleActive = true;

			int selected = 0;

			while (ConsoleActive)
			{
				bool inGame = true;
				bool inSinglePlayerGame = true;
				bool inMenu = true;

				// Główne Menu
				while (inMenu)
				{
					mainMenu.DrawMenu(selected);

					//Czekanie na klawisz
					ConsoleKeyInfo PressedKey = Console.ReadKey(intercept: true);
					switch (PressedKey.Key)
					{
						case ConsoleKey.UpArrow:
							selected--;
							break;
						case ConsoleKey.W:
							selected--;
							break;
						case ConsoleKey.DownArrow:
							selected++;
							break;
						case ConsoleKey.S:
							selected++;
							break;
						case ConsoleKey.Enter:
							inMenu = false;
							break;

					}

					selected = Math.Clamp(selected, 0, 4);
				}


				//Poszczególne Gry

				switch (selected)
				{

					case 0:
						while (inSinglePlayerGame)
						{
							SoloGame soloGame = new SoloGame();

							while (inGame)
							{
								soloGame.UpdateGameInterface();

								//ruch pierwszego gracza
								if (soloGame.NextTurn == soloGame.PlayerMark)
								{
									soloGame.SendPrimaryMessage("Twoja tura, wybierz pozycje: ", ConsoleColor.Green);

									//odczyt pozycji gracz 1
									string position = Console.ReadLine();
									soloGame.PlayerClaimSpace(soloGame.PlayerGetInputedSpace(position));

								}
								//ruch drugiego gracza/AI
								else if (soloGame.NextTurn == soloGame.EnemyMark)
								{
									soloGame.AiClaimSpace();
								}

								//sprawdzanie czy ktoś wygrał albo czy gra się skończyła
								if (soloGame.CheckWinOrEnd())
								{
									break;
								}


							}

							//Wyświetlanie kto wygrał
							soloGame.GetWinningMessage();

							//Pytanie o ponowną gre
							while (true)
							{
								soloGame.SendPrimaryMessage("Czy chcesz zagrać jeszcze raz ? (T/N) ", ConsoleColor.Gray);
								string countinue = Console.ReadLine();
								if (countinue == "T" || countinue == "t")
								{
									break;
								}
								else if (countinue == "N" || countinue == "n")
								{
									inSinglePlayerGame = false;
									break;
								}

								soloGame.SendSecondaryMessage("Źle podałeś decyzje", ConsoleColor.Red, true);

							}

							soloGame.Finished = true;
						}

						break;
					case 1:
						while (inSinglePlayerGame)
						{
							LocalGame localGame = new LocalGame();

							while (inGame)
							{
								localGame.UpdateGameInterface();

								//ruch pierwszego gracza
								if (localGame.NextTurn == localGame.PlayerMark)
								{
									localGame.SendPrimaryMessage("Krzyżyk, wybierz pozycje: ", ConsoleColor.Red);

									//odczyt pozycji gracz 1
									string position = Console.ReadLine();
									localGame.PlayerClaimSpace(localGame.PlayerGetInputedSpace(position));

								}
								//ruch drugiego gracza/AI
								else if (localGame.NextTurn == localGame.EnemyMark)
								{
									localGame.SendPrimaryMessage("Kółko, wybierz pozycje: ", ConsoleColor.Blue);

									string position = Console.ReadLine();
									localGame.EnemyClaimSpace(localGame.PlayerGetInputedSpace(position));
								}

								//sprawdzanie czy ktoś wygrał albo czy gra się skończyła
								if (localGame.CheckWinOrEnd())
								{
									break;
								}


							}

							//Wyświetlanie kto wygrał
							localGame.GetWinningMessage();

							//Pytanie o ponowną gre
							while (true)
							{
								localGame.SendPrimaryMessage("Czy chcesz zagrać jeszcze raz ? (T/N) ", ConsoleColor.Gray);
								string countinue = Console.ReadLine();
								if (countinue == "T" || countinue == "t")
								{
									break;
								}
								else if (countinue == "N" || countinue == "n")
								{
									inSinglePlayerGame = false;
									break;
								}

								localGame.SendSecondaryMessage("Źle podałeś decyzje", ConsoleColor.Red, true);

							}
							localGame.Finished = true;
						}
						break;
					case 2:
                        mainMenu.DrawEndScreen();
                        ConsoleActive = false;
                        Thread.Sleep(2000);
                        break;
						
				}
			}
		}
	}
}
