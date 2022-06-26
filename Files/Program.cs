using System;
using System.IO;


namespace Files
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input path to directory with files");
            var pathF = CheckPath();

            Restorer r = new Restorer(pathF);
            r.CreateDirectory();
            r.DirectoryCopy(pathF, pathF + @"\.versions\1", true);
            Logger l = new Logger(pathF, pathF + @"\.versions");
            l.Status();
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
