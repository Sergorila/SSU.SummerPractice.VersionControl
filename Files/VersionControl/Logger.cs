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
        private Dictionary<string, FileIndex> _history;

        public List<FileInfo> Newfiles { get; set; }
        public List<FileInfo> Delfiles { get; set; }
        public List<FileInfo> Modfiles { get; set; }

        public Logger(string FilesDirectory, string LogDirectory)
        {
            _filesDirectory = FilesDirectory;
            _logDirectory = LogDirectory;
            Newfiles = new List<FileInfo>();
            Delfiles = new List<FileInfo>();
            Modfiles = new List<FileInfo>();
            _history = new Dictionary<string, FileIndex>();
            FileInfo hist = new FileInfo(_filesDirectory + @"\index.txt");
            if (hist.Exists)
            {
                using (StreamReader sr = new StreamReader(_filesDirectory + @"\index.txt"))
                {
                    string input;
                    while((input = sr.ReadLine()) != null)
                    {
                        var info = input.Split(";");
                        FileIndex elem = new FileIndex(info[1], info[2], info[3], info[4]);
                        _history.Add(info[0], elem);
                    }
                    sr.Close();
                }
            }
            else
            {
                var fs = hist.Create();
                fs.Close();
            }
        }

        public void Status()
        {
            Newfiles = new List<FileInfo>();
            Delfiles = new List<FileInfo>();
            Modfiles = new List<FileInfo>();
            var dir1 = new DirectoryInfo(_filesDirectory);
            var logDir = new DirectoryInfo(_logDirectory);
            var logDirs = logDir.GetDirectories();
            FileCompare fileCompare = new FileCompare();
            if (logDirs.Length != 0)
            {
                var dir2 = logDirs[^1];

                var list1 = dir1.GetFiles("*.*", SearchOption.AllDirectories);
                var list2 = dir2.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (var localFile in list1)
                {
                    if (!localFile.FullName.Contains("index.txt"))
                    {
                        if (_history.ContainsKey(localFile.Name))
                        {
                            if (localFile.FullName == _history[localFile.Name].FullPath)
                            {
                                if (fileCompare.CalculateMD5(localFile.FullName) != _history[localFile.Name].Hash)
                                {
                                    Modfiles.Add(localFile);
                                }
                            }
                        }
                        else
                        {
                            Newfiles.Add(localFile);
                        }
                    }
                    
                }

                foreach(var key in _history.Keys)
                {
                    FileInfo file = new FileInfo(_history[key].FullPath);
                    if (!file.Exists)
                    {
                        var delPath = logDirs[int.Parse(_history[key].Version) - 1];
                        var files = delPath.GetFiles("*.*", SearchOption.AllDirectories);
                        foreach (var delfile in files)
                        {
                            if (delfile.Name == file.Name)
                            {
                                
                                Delfiles.Add(delfile);
                            }
                        }
                    }
                }

                GetModifiedFiles(Modfiles);
                GetNewFiles(Newfiles);
                GetDeletedFiles(Delfiles);

            }
            else
            {
                Console.WriteLine("The first commit");
                Newfiles = dir1.GetFiles("*.*", SearchOption.AllDirectories).ToList();
                foreach (var file in Newfiles)
                {
                    if (file.FullName.Contains("index.txt"))
                    {
                        Newfiles.Remove(file);
                        break;
                    }
                }
                GetNewFiles(Newfiles);
            }
            
        }

        public void CreateHistory(int num)
        {
            FileCompare fileCompare = new FileCompare();
            foreach (var file in Newfiles)
            {
                FileIndex temp = new FileIndex(file.FullName, num.ToString(), fileCompare.CalculateMD5(file.FullName), "new");
                _history.Add(file.Name, temp);
            }
            foreach(var file in Modfiles)
            {
                _history[file.Name].Version = num.ToString();
                _history[file.Name].Hash = fileCompare.CalculateMD5(file.FullName);
                _history[file.Name].Index = "mod";
            }

            foreach (var file in Delfiles)
            {
                _history[file.Name].Version = num.ToString();
                _history[file.Name].Hash = fileCompare.CalculateMD5(file.FullName);
                _history[file.Name].Index = "del";
            }

            using (StreamWriter sw = new StreamWriter(_logDirectory + @"\" + num + @"\index.txt", false))
            {
                foreach (var file in _history.Keys)
                {
                    sw.WriteLine(file + ";" + _history[file].FullPath + ";" + _history[file].Version + ";" + _history[file].Hash
                        + ";" + _history[file].Index);
                }
                sw.Close();
            }

            foreach (var file in Delfiles)
            {
                _history.Remove(file.Name);
            }

            using (StreamWriter sw = new StreamWriter(_filesDirectory + @"\index.txt", false))
            {
                foreach (var file in _history.Keys)
                {
                    sw.WriteLine(file + ";" + _history[file].FullPath + ";" + _history[file].Version + ";" + _history[file].Hash);
                }
                sw.Close();
            }
            Newfiles.Clear();
            Modfiles.Clear();
            Delfiles.Clear();
        }

        public void Backup()
        {
            Newfiles.Clear();
            Modfiles.Clear();
            Delfiles.Clear();
        }

        public void GetDeletedFiles(IEnumerable<FileInfo> list)
        {
            foreach (var item in Delfiles)
            {
                Console.WriteLine("deleted: {0}", item.FullName);
            }
        }

        public void GetNewFiles(IEnumerable<FileInfo> list)
        {

            foreach (var item in Newfiles)
            {
                Console.WriteLine("new file: {0}", item.FullName);
            }
        }

        public void GetModifiedFiles(IEnumerable<FileInfo> list)
        {
            foreach (var item in Modfiles)
            {
                Console.WriteLine("modified: {0}", item.FullName);
            }
        }

        public List<FileInfo> GetDeletedFile()
        {
            return Delfiles;
        }

        public List<FileInfo> GetNewFile()
        {
            return Newfiles;
        }

        public List<FileInfo> GetModifiedFile()
        {
            return Modfiles;
        }
    }
}