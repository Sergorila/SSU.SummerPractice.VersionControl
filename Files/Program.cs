using System;
using System.IO;
using System.Linq;

namespace Files
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input path to directory with files");
            var pathF = CheckPath();
            Console.WriteLine("Input path to directory with files");
            var pathL = CheckPath();
            UserConsole u = new UserConsole(pathF, pathL);
            u.Hub();

        }

        public static string CheckPath()
        {
            while (true)
            {
                var path = Console.ReadLine();
                if (Directory.Exists(path))
                    return path;
                Console.WriteLine("Error, this directory does not exist. Try again.");
            }
        }
    }
}
