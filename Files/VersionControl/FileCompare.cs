using System.Collections.Generic;
using System.IO;

namespace Files
{
    public class FileCompare : IEqualityComparer<FileInfo>
    {
        public FileCompare() { }

        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return (f1.Name == f2.Name &&
                    f1.Length == f2.Length && f1.LastWriteTime == f2.LastWriteTime);
        }
        
        public int GetHashCode(FileInfo fi)
        {
            string s = $"{fi.Name}{fi.Length}{fi.LastWriteTime}";
            return s.GetHashCode();
        }
    }
}
