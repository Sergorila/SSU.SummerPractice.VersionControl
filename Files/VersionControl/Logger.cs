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

            if (AreIdentical(list1, list2))
            {
                Console.WriteLine("The local files and repo files are the same");
            }
            else
            {
                Console.WriteLine("The local files and repo files are not the same");

                var queryList = list1.Intersect(list2, fileCompare);

                List<FileInfo> modf = new List<FileInfo>();

                foreach(var localFile in list1)
                {
                    if (list2.Any(repoFile => repoFile.Name == localFile.Name && 
                            repoFile.Length != localFile.Length))
                    {
                        modf.Add(localFile);
                    }
                }

                GetModifiedFiles(modf);
                var queryList1Only = (from file in list1
                                      select file).Except(list2, fileCompare).Except(modf);
                GetNewFiles(queryList1Only);

                var queryList2Only = (from file in list2
                                      select file).Except(list1, fileCompare).Except(modf);
                GetDeletedFiles(queryList2Only);
            }
        }

        public bool AreIdentical(FileInfo[] list1, FileInfo[] list2)
        {
            return list1.SequenceEqual(list2, new FileCompare());
        }

        public void GetDeletedFiles(IEnumerable<FileInfo> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("deleted: {0}", item.FullName);
            }
        }

        public void GetNewFiles(IEnumerable<FileInfo> list)
        {

            foreach (var item in list)
            {
                Console.WriteLine("new file: {0}", item.FullName);
            }
        }

        public void GetModifiedFiles(IEnumerable<FileInfo> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("modified: {0}", item.FullName);
            }
        }
    }
}