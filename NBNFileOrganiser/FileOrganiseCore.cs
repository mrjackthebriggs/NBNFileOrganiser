using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace NBNFileOrganiser
{
	class FileOrganiseCore
	{
		string currentDirectory;

		Dictionary<FileInfo, string> commentDic;
		List<FileInfo> fileList;
		List<DirectoryInfo> directList;
		bool console = true;
		
		public void DetectFilesAndDirs()
		{
			//Gets the files and the directories in the current directory and adds them to the lists

			commentDic = new Dictionary<FileInfo, string>();
			fileList = new List<FileInfo>();
			directList = new List<DirectoryInfo>();

			currentDirectory = Directory.GetCurrentDirectory();

			foreach(string s in Directory.GetFiles(currentDirectory))
			{
				string[] sDotSplit = s.Split('.');
				if (sDotSplit[sDotSplit.Length - 1] == "jpg")
				{
					FileInfo fileinfo = new FileInfo(s);
					Stream read = new FileStream(s, FileMode.Open);
					BitmapSource bmpSource = BitmapFrame.Create(read);
					BitmapMetadata meta = (BitmapMetadata)bmpSource.Metadata;
					
					//Checks to see if the photo has a comment property
					if (meta.Comment == null)
					{
						ConsoleError(fileinfo.Name, "has no comment property", "");
					}
					else
					{
						fileList.Add(fileinfo);
						string comment = meta.Comment;
						string[] oSplit = comment.Split(':');
						string desc = oSplit[oSplit.Length - 2];
						List<string> descSplit = desc.Split(' ').ToList();
						descSplit.RemoveAt(descSplit.Count - 1);
						string finalString = string.Join(" ", descSplit);
						commentDic.Add(fileinfo, finalString);
					}

					read.Close();
				}
			}

			foreach(string s in Directory.GetDirectories(currentDirectory))
			{
				directList.Add(new DirectoryInfo(s));
			}
		}

		public void CheckNamesAndAddtoDir()
		{
			foreach(FileInfo file in fileList)
			{
				List<char> nameChar = new List<char>();

				commentDic.TryGetValue(file, out string fileDesc);

				fileDesc = fileDesc.Trim();

				//Ignores everything after the specified icons/punctuation
				fileDesc = fileDesc.Split(',')[0];

				if (file.FullName == currentDirectory + Path.DirectorySeparatorChar + System.AppDomain.CurrentDomain.FriendlyName)
				{
					continue;
				}


				//Makes the starting letter of the name a capital.
				for(int i = 0;i < fileDesc.Length; i++)
				{
					if (char.IsWhiteSpace(fileDesc[i]))
						continue;

					if (char.IsSymbol(fileDesc[i]))
						continue;
					
					if (i == 0)
						nameChar.Add(char.ToUpper(fileDesc[i]));

					else
						nameChar.Add(char.ToLower(fileDesc[i]));
				}

				fileDesc = string.Concat(nameChar);


				//Moves the File to the directory or creates one if it doesn't exist
				DirectoryInfo fileDirect = directList.Find(x => x.Name == fileDesc);
				
				if(fileDirect == null)
				{
					fileDirect = Directory.CreateDirectory(currentDirectory + Path.DirectorySeparatorChar + fileDesc);
				}
				
				string newDirForFile = fileDirect.FullName + Path.DirectorySeparatorChar + file.Name;
				if (!File.Exists(newDirForFile))
				{
					file.MoveTo(newDirForFile);
				
					if (console)
					{
						Console.BackgroundColor = ConsoleColor.DarkBlue;
						Console.Write(fileDirect.Name);
						Console.ResetColor();
				
						Console.Write(" <|--- ");
						
						Console.BackgroundColor = ConsoleColor.Blue;
						Console.Write(file.Name);
						Console.Write("\n");
						Console.ResetColor();
					}
				}
				else
				{
					ConsoleError(file.Name,fileDirect.Name,"already exists in");
				}

			}
		}

		private void ConsoleError(string name, string fileDir, string issue)
		{
			if (console)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("\n\aERROR : ");
				Console.ResetColor();

				Console.Write(string.Format("{0} {1} {2}\n",name,issue,fileDir));

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("File has been left where it is.\n");
				Console.ResetColor();
			}
		}
	}
}
