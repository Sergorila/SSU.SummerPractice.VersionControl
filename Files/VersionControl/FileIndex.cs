using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files
{
    public class FileIndex
    {
        public string FullPath {get; set;}
        public string Version { get; set; }
        public int Hash { get; set; }

        public FileIndex(string fullPath, string version, int hash)
        {
            FullPath = fullPath;
            Version = version;
            Hash = hash;
        }
    }
}
