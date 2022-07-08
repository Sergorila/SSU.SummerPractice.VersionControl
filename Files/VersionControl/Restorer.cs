using System;
using System.IO;

namespace Files
{
    public class Restorer
    {
        private string _logDirectory;

        public Restorer(string logDirectory)
        {
            _logDirectory = logDirectory;
            var di = new DirectoryInfo(_logDirectory).GetDirectories().Length;
            {
                if (di == 0)
                {
                    Comnum = 1;
                }
                else
                {
                    Comnum = di + 1;
                }
            }
            
        }

        public int Comnum { get; set; }

        public void CreateDirectory()
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
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

        public void Commit(Logger log)
        {
            string path = _logDirectory + @"\" + Comnum;
            DirectoryInfo deld = new DirectoryInfo(path + @"\del");
            Directory.CreateDirectory(path + @"\del");
            DirectoryInfo newd = new DirectoryInfo(path + @"\new");
            Directory.CreateDirectory(path + @"\new");
            DirectoryInfo modd = new DirectoryInfo(path + @"\mod");
            Directory.CreateDirectory(path + @"\mod");
            var delf = log.Delfiles;
            var newf = log.Newfiles;
            var modf = log.Modfiles;
            foreach (var item in modf)
            {
                FileAttributes attr = File.GetAttributes(item.FullName);
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    item.CopyTo(modd.FullName + @"\" + item.Name);
                }
                
            }
            foreach (var item in delf)
            {
                FileAttributes attr = File.GetAttributes(item.FullName);
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    item.CopyTo(deld.FullName + @"\" + item.Name);
                }
            }
            foreach (var item in newf)
            {
                FileAttributes attr = File.GetAttributes(item.FullName);
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    item.CopyTo(newd.FullName + @"\" + item.Name);
                }
            }
            CreateHistory(log, Comnum);
            Comnum++;
        }

        public void CreateHistory(Logger log, int num)
        {
            log.CreateHistory(num);
        }

        public string BackupName()
        {
            DirectoryInfo di = new DirectoryInfo(_logDirectory);
            return _logDirectory + @"\" + (di.GetDirectories().Length + 1).ToString();
        }
        public bool FirstCommit()
        {
            if (new DirectoryInfo(_logDirectory).GetDirectories().Length == 0)
            {
                return true;
            }

            return false;
        }
    }
}