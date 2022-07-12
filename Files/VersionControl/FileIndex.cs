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
        public string Hash { get; set; }

        public string Index { get; set; }

        public FileIndex(string fullPath, string version, string hash, string index)
        {
            FullPath = fullPath;
            Version = version;
            Hash = hash;
            Index = index;
        }
    }
}
