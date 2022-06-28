using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files
{
    public class UserConsole
    {
        private string _pathF;
        private string _pathL;
        private Restorer _restorer;
        private Logger _logger;

        public UserConsole(string pathF, string pathL)
        {
            _pathF = pathF;
            _pathL = pathL;
            _restorer = new Restorer(pathL);
            _logger = new Logger(pathF, pathL);
        }
        public void Hub()
        {
            if (_restorer.FirstCommit())
            {
                _restorer.DirectoryCopy(_pathF, _restorer.BackupName(), true);
            }

            while (true)
            {
                Console.WriteLine("Choose the number of the option\n" +
                    "1. Status\n" +
                    "2. Commit\n" +
                    "3. Exit");

                if (!int.TryParse(Console.ReadLine(), out int choose))
                {
                    Console.WriteLine("Error. Try again");
                    continue;
                }

                switch (choose)
                {
                    case 1:
                        Console.Clear();
                        _logger.Status();
                        break;
                    case 2:
                        Console.Clear();
                        _restorer.DirectoryCopy(_pathF, _restorer.BackupName(), true);
                        Console.WriteLine("Commit is completed");
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Error. Try again");
                        break;
                }
            }
        }
    }
}
