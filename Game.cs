using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kolko_i_Krzyzyk___console__.Program;

namespace Kolko_i_Krzyzyk___console__
{
	internal class Game
	{
		public Mark[] GameBoard = new Mark[9] { Mark.None, Mark.None, Mark.None, Mark.None, Mark.None, Mark.None, Mark.None, Mark.None, Mark.None };
		public Mark PlayerMark = Mark.Cross;
		public Mark EnemyMark = Mark.Circle;
		public int TurnNumber = 0;
		public bool Finished = false;
		public bool BusyDrawing = false;
		public Mark NextTurn;
		public Mark Winner;

		private GameMessage primaryMessage = new GameMessage("", ConsoleColor.Green);
		public GameMessage PrimaryMessage
		{
			get => primaryMessage;
			set
			{
				primaryMessage = value;
				UpdateGameInterface();
			}
		}
		private GameMessage secondaryMessage = new GameMessage("", ConsoleColor.Red);

		public GameMessage SecondaryMessage
		{
			get => secondaryMessage;
			set
			{
				secondaryMessage = value;
				UpdateGameInterface();
			}
		}
		public int CoursorInputPosition { get; set; }

		public Game()
		{
			NextTurn = PlayerMark;
		}


		public void DrawBoardSpace(int horizontalPosition, int verticalPosition, Mark mark, int position)
		{
			// szerokosc - 8
			// wysokos - 5

			Console.CursorTop = verticalPosition;

			switch (mark)
			{
				case Mark.Cross:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.CursorLeft = horizontalPosition;
					Console.Write("██   ██ \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write(" ██ ██  \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write("  ███   \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write(" ██ ██  \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write("██   ██ ");
					break;
				case Mark.Circle:
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.CursorLeft = horizontalPosition;
					Console.Write(" █████ \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write("██   ██ \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write("██   ██ \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write("██   ██ \r\n");
					Console.CursorLeft = horizontalPosition;
					Console.Write(" █████ ");
					break;
				case Mark.None:
					if (NextTurn == Mark.Cross) Console.ForegroundColor = ConsoleColor.Red;
					else if (NextTurn == Mark.Circle) Console.ForegroundColor = ConsoleColor.Blue;
					Console.Write($"       \n");
					Console.CursorLeft = horizontalPosition;
					Console.CursorTop += 1;
					Console.Write($"  [{position}]  ");
					break;
			}

			Console.ResetColor();

		}


		public void DrawGameBoard()
		{

			int startringHorizontalPosition = 18;
			int startingverticalPosition = 4;


			//rysowanie znaków
			for (int i = 0; i < GameBoard.Length; i++)
			{
				DrawBoardSpace(startringHorizontalPosition + (i * 10) - ((i / 3) * 30), startingverticalPosition + ((i / 3) * 8), GameBoard[i], i + 1);
			}

			//rysowanie planszy
			for (int i = 0; i < 2; i++)
			{
				Console.SetCursorPosition(startringHorizontalPosition, startingverticalPosition + 6 + (i * 8));
				for (int j = 0; j < 27; j++)
				{
					Console.Write("|");
				}

				Console.SetCursorPosition(startringHorizontalPosition + 8, startingverticalPosition);
				for (int j = 0; j < 21; j++)
				{
					Console.CursorLeft = startringHorizontalPosition + 8 + (i * 10);
					Console.Write("|\n");
				}
			}

			//update tury
			DrawTurnNumber();

			Console.WriteLine("\n");
		}

		public void DrawGameBoardPrimaryMessage(GameMessage message)
		{
			if (message != null)
			{
				Console.SetCursorPosition(0, 27);
				Console.ForegroundColor = message.Color;
				Console.Write(message.Content);
				CoursorInputPosition = Console.GetCursorPosition().Left;
				Console.ResetColor();
			}
		}

		public void DrawGameBoardSecondaryMessage(GameMessage message)
		{
			if (message != null)
			{
				Console.SetCursorPosition(0, 28);
				Console.ForegroundColor = message.Color;
				Console.Write(message.Content);
				Console.ResetColor();
			}
		}

		public void DrawTurnNumber()
		{
			Console.SetCursorPosition(28, 2);
			Console.Write($"Tura: {TurnNumber}");
		}

		public void UpdateGameInterface()
		{
			if (!BusyDrawing)
			{
				BusyDrawing = true;
				Console.Clear();
				DrawGameBoard();
				DrawTurnNumber();
				DrawGameBoardPrimaryMessage(PrimaryMessage);
				DrawGameBoardSecondaryMessage(SecondaryMessage);
				Console.SetCursorPosition(CoursorInputPosition, 27);
				BusyDrawing = false;
			}

		}

		public void SendPrimaryMessage(string message, ConsoleColor color)
		{
			PrimaryMessage = new GameMessage(message, color);
		}

		public void SendSecondaryMessage(string message, ConsoleColor color, bool disappear)
		{
			SecondaryMessage = new GameMessage(message, color);

			if (disappear)
			{
				ClearSecondaryMessage(SecondaryMessage);
			}
		}

		public async Task ClearSecondaryMessage(GameMessage message)
		{
			await Task.Delay(2000);
			if (SecondaryMessage == message && Finished == false)
			{
				SecondaryMessage = new GameMessage("", ConsoleColor.White);
			}
		}

		public int PlayerGetInputedSpace(string position)
		{
			if (int.TryParse(position, out int playerSelected))
			{
				return playerSelected;
			}
			else
			{
				SendSecondaryMessage("Zle wpisałeś pozycje", ConsoleColor.Red, true);
			}

			return -1;
		}

		public bool PlayerClaimSpace(int index)
		{
			index--;

			if (index >= 0 && index < GameBoard.Length && GameBoard[index] == Mark.None)
			{
				TurnNumber++;
				GameBoard[index] = PlayerMark;
				NextTurn = EnemyMark;
				UpdateGameInterface();
				return true;
			}
			else
			{
				SendSecondaryMessage("Wybrałeś złą pozycje", ConsoleColor.Red, true);
				return false;
			}
		}

		public virtual void EnemyClaimSpace(int index)
		{
			index--;

			if (index >= 0 && index < GameBoard.Length && GameBoard[index] == Mark.None)
			{
				TurnNumber++;
				GameBoard[index] = EnemyMark;
				NextTurn = PlayerMark;
				UpdateGameInterface();
			}
			else
			{
				SendSecondaryMessage("Wybrałeś złą pozycje", ConsoleColor.Red, true);
			}
		}


		public Mark CheckWin(Mark[]? chekingBoard = null)
		{
			if (chekingBoard == null)
			{
				chekingBoard = GameBoard;
			}

			for (int i = 0; i < 2; i++)
			{
				Mark current;
				if (i == 0) current = Mark.Circle;
				else current = Mark.Cross;

				for (int j = 0; j < 3; j++)
				{
					//sprawdzanie pionowe
					if (chekingBoard[0 + j] == current && chekingBoard[3 + j] == current && chekingBoard[6 + j] == current)
					{
						return current;
					}

					//sprawdzanie poziome
					if (chekingBoard[0 + (j * 3)] == current && chekingBoard[1 + (j * 3)] == current && chekingBoard[2 + (j * 3)] == current)
					{
						return current;
					}
				}

				//sprawdzanie ukośne
				for (int l = 0; l < 2; l++)
				{
					if (chekingBoard[0 + (2 * l)] == current && chekingBoard[4] == current && chekingBoard[8 - (2 * l)] == current)
					{
						return current;
					}
				}


			}
			return Mark.None;
		}

		public bool CheckWinOrEnd()
		{
			if (CheckWin() != Mark.None || TurnNumber == 9)
			{
				Winner = CheckWin();
				Finished = true;
				return true;
			}
			else
			{
				return false;
			}
		}

		public void ClearGameBoard()
		{
			for(int i = 0; i < GameBoard.Length; i++)
			{
				GameBoard[i] = Mark.None;
			}

			TurnNumber = 0;
			Winner = Mark.None;
			Finished = false;
		}

		public void ReversePlayerMarks()
		{
			Mark temp = PlayerMark;
			PlayerMark = EnemyMark;
			EnemyMark = temp;
		}

		public void ReverseOrder()
		{
			if (NextTurn == Mark.Cross)
			{
				NextTurn = Mark.Circle;
			}
			else if (NextTurn == Mark.Circle)
			{
				NextTurn = Mark.Cross;
			}
		}

		public virtual void GetWinningMessage()
		{

		}

	}
}
