using System.Collections.Generic;
using System.IO;

namespace Files
{
    public class FileCompare : IEqualityComparer<FileInfo>
    {
        public FileCompare() { }

        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return (f1.Name == f2.Name && f1.Length == f2.Length);
        }
        
        public int GetHashCode(FileInfo fi)
        {
            string s = $"{fi.Name}{fi.Length}";
            return s.GetHashCode();
        }
    }
}
