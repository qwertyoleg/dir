using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;


namespace DirectoryManager
{
    class MyDirecory
    {
        private List<string> foundFiles;
        private List<string> foundDirectories; 
        private string currentPath;
        private ArrayList listOfFiles;
        private ArrayList listOfFolders;
        private List<List<string>> linesFound;
        public MyDirecory()
        {
            this.foundDirectories = new List<string>();
            this.foundFiles = new List<string>();
            this.currentPath = Directory.GetCurrentDirectory();
            RefreshListOfFolders();
            RefreshListOfFiles();
        }
        private void ClearListOfFoundDirectories()
        {
            this.foundDirectories = new List<string>();
        }
        private void ClearListOfFoundFiles()
        {
            this.foundFiles = new List<string>();
        }
        private void ChangeFolderName(string dorectoryPath, string newDirectoryName)
        {
            DirectoryInfo dir = new DirectoryInfo(dorectoryPath);
            
            string newName = dir.Parent.FullName;
            Directory.Move(dorectoryPath, Path.Combine(newName, newDirectoryName));
        }
        private void ShowFoundFiles()
        {
            if (this.foundFiles.Count == 0)
            {
                Console.WriteLine("Files are not found");
                return;
            }
            Console.WriteLine("Found {0} files: ", this.foundFiles.Count);
            foreach (var file in this.foundFiles)
                Console.WriteLine(file);

        }
        private void ShowFoundDerectories()
        {
            if (this.foundDirectories.Count==0)
            {
                Console.WriteLine("Folders are not found");
                return;
            }
            Console.WriteLine("Found {0} directories: ", this.foundDirectories.Count);
            foreach (var folderPath in foundDirectories)
            {
                Console.WriteLine(folderPath);
            }
        }
      
        //Show catalog files and folders in directory
        private void FileInf(string path)
        {
            FileInfo theFile = new FileInfo(path);
            Console.WriteLine(theFile.Name);
            Console.WriteLine(theFile.CreationTime.ToString());
            Console.WriteLine(theFile.LastAccessTime.ToString());
            Console.WriteLine(theFile.LastWriteTime.ToString());
            Console.WriteLine(theFile.Length + " bytes");  //file size
        }
        private void DisplayCurrentFolderList()
        {
            //DisplayFolderContents(this.currentPath);
            foreach (DirectoryInfo folder in this.listOfFolders)
            {
                Console.WriteLine(folder.Name);
            }
            foreach (FileInfo file in listOfFiles)
            {
                Console.WriteLine(file.Name);
            }
        }
        private void DisplayFolderContents(string currentPath)
        {
            //Displays all folder in current path
            DirectoryInfo dInf = new DirectoryInfo(currentPath);
            foreach (DirectoryInfo nextFolder in dInf.GetDirectories())
                Console.WriteLine(nextFolder.Name);
            FileInfo[] finf = dInf.GetFiles();
            foreach (var fileInfo in finf)
                Console.WriteLine(fileInfo.Name);
        }
        //creates new directory,
        private void CreateNewDirectory(string name)
        {
            bool folderAlreadyExists = false;
            foreach (string folder in listOfFolders)
            {
                if (folder==name)
                {
                    folderAlreadyExists = true;
                }
            }
            if (folderAlreadyExists)
            {
                Console.WriteLine("Folder with name \"{0}\" is already exists in current directory!",name);
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(".");
                dir.CreateSubdirectory(name);
            }
        }
        //Manipulations with founded files
        private void RefreshListOfFolders()
        {
            //getting list of folders
            this.listOfFolders = new ArrayList();
            DirectoryInfo dInf = new DirectoryInfo(this.currentPath);
            foreach (DirectoryInfo folder in dInf.GetDirectories())
                this.listOfFolders.Add(folder);
        }
        private void RefreshListOfFiles()
        {
            //getting list of files
            this.listOfFiles = new ArrayList();
            DirectoryInfo dInf = new DirectoryInfo(this.currentPath);
            FileInfo[] finf = dInf.GetFiles();
            foreach (FileInfo file in finf)
                this.listOfFiles.Add(file);
        }
        private void Explorer()
        {
            int selectedFolderIndex=0;
            int i;
            string currentFolderPath="";
            ConsoleKeyInfo cki;
            do
            {
                i = 0;
                Console.Clear();
                DirectoryInfo theFolder = new DirectoryInfo(this.currentPath);
                foreach (DirectoryInfo nextFolder in theFolder.GetDirectories())
                {
                    if (i==selectedFolderIndex)
                    {
                        Console.WriteLine("["+nextFolder.Name+"]");
                        currentFolderPath = nextFolder.FullName;
                    }
                    else
                    Console.WriteLine(nextFolder.Name);
                    i++;
                }
                Console.WriteLine("\n Press [F] to start search from this directory.");
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    selectedFolderIndex++;
                    if (selectedFolderIndex >= listOfFolders.Count)
                        selectedFolderIndex--;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    selectedFolderIndex--;
                    if (selectedFolderIndex < 0)
                        selectedFolderIndex++;
                }
                if (cki.Key == ConsoleKey.RightArrow)
                {
                    string path = Path.Combine(this.currentPath, currentFolderPath);
                    this.currentPath = path;
                    RefreshListOfFolders();
                }
                if (cki.Key == ConsoleKey.Enter)
                {
                    return;
                }
                if (cki.Key == ConsoleKey.LeftArrow)
                {
                    DirectoryInfo dir = new DirectoryInfo(this.currentPath);
                    DirectoryInfo pInfo = dir.Parent;
                    this.currentPath = pInfo.FullName;
                    RefreshListOfFolders();
                }
                //file search
                if (cki.Key == ConsoleKey.F)
                {
                    ExploreMenu();
                    return;
                }
            } while (cki.Key != ConsoleKey.Escape);
        
        }
        private void DeleteFoundFiles()
        {
            foreach (string file in this.foundFiles)
            {
                var fi=new FileInfo(file);
                fi.Delete();
            }
            this.foundFiles.Clear();
        }
        private void DeleteDirectory(string dir)
        {
            foreach (string directory in Directory.GetDirectories(dir))
            {
                if (Directory.GetDirectories(directory).Length == 0)
                {
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        var fi = new FileInfo(file);
                        fi.Delete();
                    }
                    Directory.Delete(directory);
                    return;
                }
                DeleteDirectory(directory);
            }
        }
        private void MoveFoundFiles()
        {
            foreach (string fPath in this.foundFiles)
            {
                //copy
                string fileName = Path.GetFileName(fPath);
                string destPath = System.IO.Path.Combine(this.currentPath, fileName);
                File.Copy(fPath, destPath, true);
                //delete
                var fi = new FileInfo(fPath);
                fi.Delete();
            }
        }
        /// <summary>
        ///
        /// </summary> 
        /// <param name="dir"></param>
        private void MoveDirectory(string dir)
        {
            foreach (string sDir in Directory.GetDirectories(dir))
            {
                if (Directory.GetDirectories(sDir).Length == 0)
                {
                    foreach (string file in Directory.GetFiles(sDir))
                    {
                        var fi = new FileInfo(file);
                        fi.Delete();
                    }
                    Directory.Delete(sDir);
                    return;
                }
                DeleteDirectory(sDir);
            }
        }
        /// <summary>
        ///
        /// </summary>
        private void CopyFoundFiles()
        {
            foreach (string fPath in this.foundFiles)
            {
                string fileName= Path.GetFileName(fPath);
                string destPath = System.IO.Path.Combine(this.currentPath, fileName);
                File.Copy(fPath, destPath,true);
            }
        }
        private void EditFoundFiles()
        {
            Console.Clear();
            if (this.foundFiles.Count==0)
            {
                Console.WriteLine("List of founded files is empty!");
                return;
            }
            ShowFoundFiles();
            Console.ReadLine();

            ConsoleKeyInfo answer = new ConsoleKeyInfo();
            do
            {
                Console.Clear();
                Console.WriteLine("What do you want to do with found files?");
                Console.WriteLine("[D]elete files, [M]ove files or [C]opy files");
                Console.Write("->");
                answer = Console.ReadKey();
                switch (answer.Key)
                {
                    case ConsoleKey.D:
                        Console.Clear();
                        Console.WriteLine("Do you really want to delete this files:");
                        ShowFoundFiles();
                        Console.Write("press 'Y' or 'N' ->");
                        answer = Console.ReadKey();
                        if (answer.Key == ConsoleKey.Y)
                        {
                            DeleteFoundFiles();
                            Console.Clear();
                            Console.WriteLine("Files have been deleted!");
                        }
                        if (answer.Key == ConsoleKey.N)
                        {
                            Console.WriteLine("You decide not to delete files found.");
                        }
                        break;
                    case ConsoleKey.M:
                        Console.WriteLine("Do you really want to move this files:");
                        ShowFoundFiles();
                        Console.Write("press 'Y' or 'N' ->");
                        answer = Console.ReadKey();
                        if (answer.Key == ConsoleKey.Y)
                        {
                            Console.Clear();
                            Console.WriteLine("Chose distention folder:");
                            Console.WriteLine("Use arrow keys to navigate and press [Enter].");
                            Console.Read();
                            Explorer();
                            MoveFoundFiles();
                            Console.WriteLine("Files have been moved!");
                            Console.Read();
                        }
                        if (answer.Key == ConsoleKey.N)
                        {
                            Console.Clear();
                            Console.WriteLine("You decide not to move files found.");
                            Console.Read();
                        }
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        Console.WriteLine("Do you really want to copy this files:");
                        ShowFoundFiles();
                        Console.Write("press 'Y' or 'N' ->");
                        answer = Console.ReadKey();
                        if (answer.Key == ConsoleKey.Y)
                        {
                            Console.Clear();
                            Console.WriteLine("Chose distention folder:");
                            Console.WriteLine("Use arrow keys to navigate and press [Enter].");
                            Console.Read();
                            Explorer();
                            CopyFoundFiles();
                            Console.Clear();
                            Console.WriteLine("Files have been copied!");
                        }
                        if (answer.Key == ConsoleKey.N)
                        {
                            Console.Clear();
                            Console.WriteLine("You decide not to copy files found.");
                            Console.Read();
                        }
                        break;
                }
            } while (answer.Key!=ConsoleKey.Escape);            
        }
        private void EditFoundDirectories()
        {
            Console.Clear();
            if (this.foundDirectories.Count == 0)
            {
                Console.WriteLine("List of founded folders is empty!");
                return;
            }
            else
            {
                ShowFoundDerectories();
            }
            Console.ReadLine();

            ConsoleKeyInfo answer = new ConsoleKeyInfo();
            do
            {
                Console.Clear();
                Console.WriteLine("What do you want to do with found folders?");
                Console.WriteLine("[D]elete files, [M]ove files or [C]opy folders");
                Console.Write("->");
                answer = Console.ReadKey();
                switch (answer.Key)
                {
                    case ConsoleKey.D:
                        Console.Clear();
                        Console.WriteLine("Do you really want to delete this folders:");
                        ShowFoundDerectories();
                        Console.Write("press 'Y' or 'N' ->");
                        answer = Console.ReadKey();
                        if (answer.Key == ConsoleKey.Y)
                        {
                            foreach (string foundDirectory in this.foundDirectories)
                            {
                                DeleteDirectory(foundDirectory);
                            }
                            this.foundDirectories.Clear();
                            Console.Clear();
                            Console.WriteLine("Folders have been deleted!");
                        }
                        if (answer.Key == ConsoleKey.N)
                        {
                            Console.WriteLine("You decide not to delete folders found.");
                        }
                        break;
                    case ConsoleKey.M:
                        Console.WriteLine("Do you really want to move this folders:");
                        ShowFoundDerectories();
                        Console.Write("press 'Y' or 'N' ->");
                        answer = Console.ReadKey();
                        if (answer.Key == ConsoleKey.Y)
                        {
                            Console.Clear();
                            Console.WriteLine("Chose distention folder:");
                            Console.WriteLine("Use arrow keys to navigate and press [Enter].");
                            Console.Read();
                            Explorer();
                            MoveFoundFiles();
                            Console.WriteLine("Folders have been moved!");
                            Console.Read();
                        }
                        if (answer.Key == ConsoleKey.N)
                        {
                            Console.Clear();
                            Console.WriteLine("You decide not to move folders found.");
                            Console.Read();
                        }
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        Console.WriteLine("Do you really want to copy this folders:");
                        ShowFoundFiles();
                        Console.Write("press 'Y' or 'N' ->");
                        answer = Console.ReadKey();
                        if (answer.Key == ConsoleKey.Y)
                        {
                            Console.Clear();
                            Console.WriteLine("Chose distention folder:");
                            Console.WriteLine("Use arrow keys to navigate and press [Enter].");
                            Console.Read();
                            Explorer();
                            CopyFoundFiles();
                            Console.Clear();
                            Console.WriteLine("Folders have been copied!");
                        }
                        if (answer.Key == ConsoleKey.N)
                        {
                            Console.Clear();
                            Console.WriteLine("You decide not to copy folders found.");
                            Console.Read();
                        }
                        break;
                }
            } while (answer.Key != ConsoleKey.Escape);
        }
        public void ExploreMenu()
        {
           do
           {
               Explorer();
           Console.Clear();
           Console.WriteLine("What do you want to find? ");
           Console.WriteLine("File   [1],\nFolder [2]");
           int answer1 = Convert.ToInt32(Console.ReadLine());
           if(answer1 == 1)
           {
                   Console.Clear();
                   Console.WriteLine("Search file by name:                [1]");
                   Console.WriteLine("Search file by creation time:       [2];");
                   Console.WriteLine("Search file by last written time:   [3]");
                   Console.WriteLine("Search file by last accessed time:  [4]");
                   Console.WriteLine("Search file by  size                [5]");
                   Console.WriteLine("Search text file by contents string:[6]");
                   int answer2 = Convert.ToInt32(Console.ReadLine());
                   DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
                   DirectoryInfo s = di.Parent;
                   switch (answer2)
                   {
                       case 1:
                           {
                               string fileName = "";
                               do
                               {
                               Console.Clear();
                               Console.Write("Input file name->");
                               fileName = Console.ReadLine();
                               } while (fileName=="");
                               
                               SearchFilesByName(s.FullName, fileName);
                               EditFoundFiles();
                           }
                           break;
                       case 2:
                           {
                               Console.Clear();
                               Console.Write("Input creation date dd.mm.yyyy ->");
                               string time = Console.ReadLine();
                               SearchFilesByCreationTime(s.FullName, time);
                               EditFoundFiles();
                           }
                           break;
                       case 3:
                           {
                               Console.Clear();
                               Console.Write("Input last written date dd.mm.yyyy ->");
                               string time = Console.ReadLine();
                               SearchFilesByWriteTime(s.FullName, time);
                               EditFoundFiles();
                           }
                           break;
                       case 4:
                           {
                               Console.Clear();
                               Console.Write("Input last accessed date dd.mm.yyyy ->");
                               string time = Console.ReadLine();
                               SearchFilesByAccessTime(s.FullName, time);
                               EditFoundFiles();
                           }
                           break;
                       case 5:
                           {
                               Console.Clear();
                               Console.Write("Input min file size ->");
                               Int64 minSize = Convert.ToInt64(Console.ReadLine());
                               Console.Write("Input max file size ->");
                               Int64 maxSize = Convert.ToInt64(Console.ReadLine());
                               SearchFilesBySize(s.FullName, maxSize, minSize);
                               EditFoundFiles();
                           }
                           break;
                       case 6:
                            {
                               Console.Clear();
                               Console.Write("Input string ->");
                               string str = Console.ReadLine();
                               SearchTextFilesByContents(s.FullName,str);
                               Console.WriteLine("Do you want to replace founded string?[Y/N]");
                               ConsoleKeyInfo key=Console.ReadKey();
                               do
                               {
                                   if (key.Key == ConsoleKey.Y)
                                   {
                                       Console.WriteLine("Input new string");
                                       string newStr = Console.ReadLine();
                                       ReplaceSubstringInFoundFiles(newStr, str);
                                       break;
                                   }
                                   if (key.Key == ConsoleKey.N)
                                       break;
                               } while (true);
                               EditFoundFiles();
                           }
                           break;
                   }
               return;
               }
               if (answer1 == 2)
               {
                   Console.Clear();
                   Console.WriteLine("Search folder by name:              [1]");
                   Console.WriteLine("Search folder by creation time:     [2]");
                   Console.WriteLine("Search folder by last written time: [3]");
                   Console.WriteLine("Search folder by last accessed time:[4]");
                   int answer2 = Convert.ToInt32(Console.ReadLine());
                   DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
                   DirectoryInfo s = di.Parent;
                   switch (answer2)
                   {
                       case 1:
                           {
                               string fileName = "";
                               do
                               {
                                   Console.Clear();
                                   Console.Write("Input folder name->");
                                   fileName = Console.ReadLine();
                               } while (fileName == "");

                               SearchDirectoriesByName(s.FullName, fileName);
                               EditFoundFiles();
                           }
                           break;
                       case 2:
                           {
                               Console.Clear();
                               Console.Write("Input creation date dd.mm.yyyy ->");
                               string time = Console.ReadLine();
                               SearchFilesByCreationTime(s.FullName, time);
                               EditFoundFiles();
                           }
                           break;
                       case 3:
                           {
                               Console.Clear();
                               Console.Write("Input last written date dd.mm.yyyy ->");
                               string time = Console.ReadLine();
                               SearchDirectoriesByWriteTime(s.FullName, time);
                               EditFoundFiles();
                           }
                           break;
                       case 4:
                           {
                               Console.Clear();
                               Console.Write("Input last accessed date dd.mm.yyyy ->");
                               string time = Console.ReadLine();
                               SearchDirectoriesByAccessTime(s.FullName, time);
                               EditFoundFiles();
                           }
                           break;
                   }
                   return;
               }
           } while (true);
        }

        private void ReplaceSubstringInFoundFiles(string newString, string oldString)
        {
            List<string> text; 
            int count;
            foreach (string fFile in this.foundFiles)
            {
                text = new List<string>();
                if (fFile.Contains("*.txt"))
                {
                    string line;
                    count = 0;
                    StreamReader fileRead = new StreamReader(fFile);
                    while ((line = fileRead.ReadLine()) != null)
                    {
                        if (line.Contains(oldString))
                        {
                            Console.WriteLine(line);
                            this.foundFiles.Add(fFile);
                            text.Add(line.Replace(oldString, newString));
                            count++;
                        }
                        else
                            text.Add(line);
                        count++;
                    }
                    fileRead.Close();
                    StreamWriter fileWrite = new StreamWriter(fFile);
                    for (int i = 0; i < count; i++)
                    {
                        fileWrite.Write(text[i]);
                    }
                    fileWrite.Close();
                    Console.WriteLine("In file \"{0}\" was replased {1} substrings",fFile,count);
                }
                else
                    Console.WriteLine("It is not *.txt file!");
            }
        }
        private void SearchTextFilesByContents(string dir, string contentStr)
        {
            int i=0;
                foreach (string sFile in Directory.GetFiles(dir, "*.txt", searchOption: SearchOption.AllDirectories))
                {
                    Console.WriteLine(sFile);
                    string line;
                    StreamReader fileStreamReader = new StreamReader(sFile);
                    while ((line = fileStreamReader.ReadLine()) != null)
                    {
                        if (line.Contains(contentStr))
                        {
                            Console.WriteLine(line);
                            this.foundFiles.Add(sFile);
                            break;
                        }
                    }
                    fileStreamReader.Close();
                }
        }
        //Search files by name
        private void SearchFilesByName(string dir, string fileName)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string fFile in Directory.GetFiles(directory, "*" + fileName + "*"))
                    {
                        foundFiles.Add(fFile);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchFilesByName(directory, fileName);
                }
        }

        //search directories by name
        private void SearchDirectoriesByName(string dir,string name)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string fDir in Directory.GetDirectories(directory, "*" + name + "*"))
                    {
                       // Console.WriteLine(foundDirectory);
                        foundDirectories.Add(fDir);
                       // Console.ReadLine();
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if(strMass.Length==0)
                        return;
                    SearchDirectoriesByName(directory, name);
                }
        }
        private void SearchFilesByCreationTime(string dir, string creationTime)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string fS in Directory.GetFiles(directory))
                    {
                        if (File.GetCreationTime(fS).ToString().Contains(creationTime))
                            foundFiles.Add(fS);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchFilesByCreationTime(directory, creationTime);
                }
        }
        private void SearchFilesByAccessTime(string dir, string accessTime)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string sFile in Directory.GetFiles(directory))
                    {
                        if (File.GetLastAccessTime(sFile).ToString().Contains(accessTime))
                            foundFiles.Add(sFile);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchFilesByAccessTime(directory, accessTime);
                }
        }
        private void SearchFilesByWriteTime(string dir, string writeTime)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string sFile in Directory.GetFiles(directory))
                    {
                        if (File.GetLastWriteTime(sFile).ToString().Contains(writeTime))
                            foundFiles.Add(sFile);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchFilesByWriteTime(directory, writeTime);
                }
        }
        //dd.mm.yyyy search
        private void SearchDirectoriesByCreationTime(string dir, string creationTime)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string sDir in Directory.GetDirectories(directory))
                    {
                        if (Directory.GetCreationTime(sDir).ToString().Contains(creationTime))
                            foundDirectories.Add(sDir);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchDirectoriesByCreationTime(directory, creationTime);
                }
        }
        private void SearchDirectoriesByAccessTime(string dir, string accessTime)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string sDir in Directory.GetDirectories(directory))
                    {
                        if (Directory.GetLastAccessTime(sDir).ToString().Contains(accessTime))
                            foundDirectories.Add(sDir);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchDirectoriesByAccessTime(directory, accessTime);
                }
        }
        private void SearchDirectoriesByWriteTime(string dir, string writeTime)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string sDir in Directory.GetDirectories(directory))
                    {
                        if (Directory.GetLastWriteTime(sDir).ToString().Contains(writeTime))
                            foundDirectories.Add(sDir);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchDirectoriesByWriteTime(directory, writeTime);
                }
        }
        private void SearchFilesBySize(string dir, long maxSize, long minSize)
        {
                foreach (string directory in Directory.GetDirectories(dir))
                {
                    foreach (string sFile in Directory.GetFiles(directory))
                    {
                        FileInfo fileInfo = new FileInfo(sFile);
                        if (fileInfo.Length >= minSize * 1024 && fileInfo.Length <= maxSize)
                            foundFiles.Add(sFile);
                    }
                    string[] strMass = Directory.GetDirectories(directory);
                    if (strMass.Length == 0)
                        return;
                    SearchFilesBySize(directory, maxSize, minSize);
                }
        }
    
    }

    class Program
    {
        static void Main(string[] args)
        {
            //DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            //DirectoryInfo s=di.Parent;
            var dirObj = new MyDirecory();
            dirObj.ExploreMenu();
            Console.Read();
        }
    }
}
