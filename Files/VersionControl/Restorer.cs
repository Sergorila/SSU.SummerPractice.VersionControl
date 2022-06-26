using System;
using System.IO;

namespace Files
{
    public class Restorer
    {
        private string _logDirectory;

        public Restorer(string FilesDirectory)
        {
            _logDirectory = FilesDirectory + @"\.versions";
        }

        public void CreateDirectory()
        {
            if (!Directory.Exists(_logDirectory))
            {
                DirectoryInfo di = Directory.CreateDirectory(_logDirectory);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }

        public void DirectoryCopy(string filesDir, string newDir, bool copySubDirs)
        {
            var dir = new DirectoryInfo(filesDir);
            var dirs = dir.GetDirectories();

            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            } 
            var files = dir.GetFiles();

            foreach (var file in files)
            {
                var tempPath = Path.Combine(newDir, file.Name);
                try
                {
                    file.CopyTo(tempPath, true);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (copySubDirs)
            {
                foreach (var subDir in dirs)
                {
                    if (subDir.FullName != _logDirectory)
                    {
                        var tempPath = Path.Combine(newDir, subDir.Name);
                        DirectoryCopy(subDir.FullName, tempPath, copySubDirs);
                    }
                }
            }
        }
    }
}