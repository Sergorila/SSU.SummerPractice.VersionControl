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
 
            IEnumerable<FileInfo> list1 = dir1.GetFiles("*.*", SearchOption.AllDirectories);
            IEnumerable<FileInfo> list2 = dir2.GetFiles("*.*", SearchOption.AllDirectories);
 
            FileCompare fileCompare = new FileCompare();
  
            bool areIdentical = list1.SequenceEqual(list2, fileCompare);

            if (areIdentical == true)
            {
                Console.WriteLine("The two folders are the same");
            }
            else
            {
                Console.WriteLine("The two folders are not the same");
            }
            
            var queryCommonFiles = list1.Intersect(list2, fileCompare);

            if (queryCommonFiles.Any())
            {
                Console.WriteLine("The following files are in both folders:");

                foreach (var item in queryCommonFiles)
                {
                    Console.WriteLine(item.FullName);
                }
            }
            else
            {
                Console.WriteLine("There are no common files in the two folders.");
            }
  
            var queryList1Only = (from file in list1
                                  select file).Except(list2, fileCompare);

            Console.WriteLine("The following files are in list1 but not list2:");

            foreach (var item in queryList1Only)
            {
                Console.WriteLine(item.FullName);
            }
 
            Console.WriteLine("Press Enter to exit.");
            Console.ReadKey();
        }
    }
}