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

            while (true)
            {
                Console.WriteLine("Choose the number of the option\n" +
                    "1. Status\n" +
                    "2. Commit\n" +
                    "3. Backup\n" +
                    "4. Exit");

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
                        _logger.Status();
                        Console.Clear();
                        _restorer.Commit(_logger);
                        Console.WriteLine("Commit is completed");
                        break;
                    case 3:
                        var backups = _restorer.GetBackups();
                        for (int i = 0; i < backups.Length; i++)
                        {
                            Console.WriteLine("{0}. {1}", i+1, backups[i]);
                        }
                        if (int.TryParse(Console.ReadLine(), out int backnum))
                        {
                            if (backnum >= 1 && backnum <= backups.Length)
                            {
                                _restorer.Backup(backnum, _pathF);
                                _logger.Backup();
                                _logger.Status();
                                _restorer.Commit(_logger);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error. Try again");
                            continue;
                        }
                        return;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Error. Try again");
                        break;
                }
            }
        }
    }
}
