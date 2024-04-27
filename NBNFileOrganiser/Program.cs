using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBNFileOrganiser
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "CNS NBN File Organiser";
			FileOrganiseCore cont = new FileOrganiseCore();

			cont.DetectFilesAndDirs();
			cont.CheckNamesAndAddtoDir();

			Console.Title += " - Completed!";
			Console.WriteLine("\nDone! Press Enter to Exit");
			Console.Read();
		}
	}
}
