using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBNFileOrganiser
{
	class RepeatTimer
	{
		public static void StartRepeatFor(ref FileOrganiseCore FO)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("\n	SET FOR TIME SELECTED	\n");
			Console.ResetColor();

			int[] timeFor = ParseTime("Input time duration (format => hr:min)");
			int freqTime = ParseTime("Input frequency in minutes")[1];

			DateTime Now = DateTime.Now;

			DateTime finishTime = Now + new TimeSpan(timeFor[0],timeFor[1],0);

			while(DateTime.Now < finishTime)
			{

			}
		}

		public static void StartRepeatTill(ref FileOrganiseCore FO)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("\n	SET TILL TIME SELECTED	\n");
			Console.ResetColor();
		}

		private static int[] ParseTime(string message)
		{
			string timeForString;
			int hrsInt = 0;
			int minInt = 0;
			bool noError1 = true;
			bool noError2 = true;

			while (true)
			{
				Console.WriteLine(message);
				timeForString = Console.ReadLine();
				string[] timeStringArray = timeForString.Split(':');

				if (timeStringArray.Length > 2)
				{
					ErrorMessage("IMPROPPER FORMAT, PLEASE TRY AGAIN");
					continue;
				}
				else if(timeStringArray.Length == 2)
				{
					noError1 = int.TryParse(timeStringArray[0],out hrsInt);
					noError2 = int.TryParse(timeStringArray[1], out minInt);
				}
				else if(timeStringArray.Length == 1)
				{
					noError1 = int.TryParse(timeStringArray[1], out minInt);
				}

				if(hrsInt > 24 | minInt > 60)
				{
					ErrorMessage("TIME INPUT IS TOO LARGE (HRS MAX = 24 , MINS MAX = 60)");
					continue;
				}

				if(!noError1 | noError2)
				{
					ErrorMessage("INPUT INVALID, PLEASE TRY AGAIN");
					continue;
				}

				return new int[] { hrsInt, minInt };
			}
		}
		private static void ErrorMessage(string errormessage)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(string.Format("\n\t{0}\t\n",errormessage));
			Console.ResetColor();
		}
	}
}
