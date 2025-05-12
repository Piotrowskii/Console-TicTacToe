using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolko_i_Krzyzyk___console__
{
	public class GameMessage
	{
		public string Content { get; set; }
		public ConsoleColor Color { get; set; }

		public GameMessage(string content, ConsoleColor color)
		{
			Content = content;
			Color = color;
		}
	}
}
