using Jinyinmao.Daemon.Models;
using Jinyinmao.Daemon.Utils;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace TestConsole
{
    public class Program
    {
        private static void GetLastWorkDays()
        {
            ConfigEntity config = StorageHelper.QueryConfigTable<ConfigEntity>().Where(c => c.IsWorkday).OrderByDescending(c => c.RowKey).Take(4).LastOrDefault();

            Console.WriteLine(JsonConvert.SerializeObject(config));
        }

        private static void Main(string[] args)
        {
            GetLastWorkDays();
            Console.ReadKey();
        }
    }
}