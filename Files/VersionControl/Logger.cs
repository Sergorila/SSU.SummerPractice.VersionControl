using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Files
{
    public class Logger
    {
        private string _filesDirectory;
        private string _logDirectory;

        public Logger(string FilesDirectory, string LogDirectory)
        {
            _filesDirectory = FilesDirectory;
            _logDirectory = LogDirectory;
        }

        public void Status()
        {
            var dir1 = new DirectoryInfo(_filesDirectory);
            var logDir = new DirectoryInfo(_logDirectory);
            var logDirs = logDir.GetDirectories();
            var dir2 = logDirs[^1];
 
            var list1 = dir1.GetFiles("*.*", SearchOption.AllDirectories);
            var list2 = dir2.GetFiles("*.*", SearchOption.AllDirectories);
 
            FileCompare fileCompare = new FileCompare();
  
            bool areIdentical = list1.SequenceEqual(list2, fileCompare);

            if (areIdentical == true)
            {
                Console.WriteLine("The local files and repo files are the same");
            }
            else
            {
                Console.WriteLine("The local files and repo files are not the same");
                if (list1.Length == list2.Length)
                {
                    var queryList = list1.Intersect(list2, fileCompare);
                    Console.WriteLine("Modified files:");

                    foreach (var item in queryList)
                    {
                        Console.WriteLine(item.FullName);
                    }
                }
                else if (list1.Length > list2.Length)
                {
                    var queryList1Only = (from file in list1
                                          select file).Except(list2, fileCompare);

                    Console.WriteLine("New files:");

                    foreach (var item in queryList1Only)
                    {
                        Console.WriteLine(item.FullName);
                    }
                }
                else
                {
                    var queryList2Only = (from file in list2
                                          select file).Except(list1, fileCompare);

                    Console.WriteLine("Deleted files:");

                    foreach (var item in queryList2Only)
                    {
                        Console.WriteLine(item.FullName);
                    }
                }
            }
        }
    }
}