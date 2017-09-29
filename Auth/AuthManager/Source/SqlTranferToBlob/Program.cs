using Jinyinmao.AuthManager.Domain.Core.Models;
using Microsoft.Azure;
using Moe.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SqlTranferToBlob
{
    internal class Program
    {
        private static readonly string connectionString = CloudConfigurationManager.GetSetting("JymDbConnectionString");

        private static async Task<List<string>> GetAllIds(string databaseIndex, string grainsIndex, string timeStamp)
        {
            using (JYMDBContext context = new JYMDBContext(connectionString.FormatWith(databaseIndex)))
            {
                try
                {
                    if (timeStamp.IsNotNullOrEmpty())
                    {
                        return await context.Database.SqlQuery<string>("select [Id] from(select Id, TimeStamp, row_number() over(partition by Id order by TimeStamp desc) as rowId from Grains_{0}) t where t.rowId = 1 AND TimeStamp >= {1} ORDER BY [TimeStamp]".FormatWith(grainsIndex, timeStamp)).ToListAsync();
                    }
                    // string strSql = "select DISTINCT Id from Grains_{0}  where TimeStamp >= {1} ORDER BY [TimeStamp]".FormatWith(grainsIndex, timeStamp);
                    return await context.Database.SqlQuery<string>("select [Id] from(select Id, TimeStamp, row_number() over(partition by Id order by TimeStamp desc) as rowId from Grains_{0}) t where t.rowId = 1 ORDER BY [TimeStamp]".FormatWith(grainsIndex)).ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetExceptionString());
                    throw;
                }
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("please input database index");
            string databaseIndex = Console.ReadLine();
            Console.WriteLine("please input grains index");
            string grainsIndex = Console.ReadLine();
            Console.WriteLine("please input timestamp");
            string timeStamp = Console.ReadLine();
            try
            {
                TransferTask(databaseIndex, grainsIndex, timeStamp).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine(ex.GetExceptionString());
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                throw;
            }
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadKey();
        }

        private static async Task TransferTask(string databaseIndex, string grainsIndex, string timeStamp)
        {
            //string str = "select [Id],[LongId],[Key],[Type],[TimeStamp],[Data] from (select *,row_number() over(partition by Id order by TimeStamp desc) as rowId from Grains_{0}) t where t.rowId = 1 ORDER BY t.TimeStamp".FormatWith(grainsIndex);
            //if (timeStamp.IsNotNullOrEmpty())
            //{
            //    str = "select [Id],[LongId],[Key],[Type],[TimeStamp],[Data] from (select *,row_number() over(partition by Id order by TimeStamp desc) as rowId from Grains_{0}) t where t.rowId = 1 and  TimeStamp >= {1} ORDER BY t.TimeStamp".FormatWith(grainsIndex, timeStamp);
            //}

            //using (JYMDBContext context = new JYMDBContext(connectionString.FormatWith(databaseIndex)))
            //{
            //    List<GrainStateRecord> list = await context.Database.SqlQuery<GrainStateRecord>(str).ToListAsync();

            //    Console.WriteLine("Start grains_{0}".FormatWith(grainsIndex));
            //    long time = 0L;
            //    try
            //    {
            //        for (int i = 0; i < list.Count; i++)
            //        {
            //            GrainStateRecord record = list[i];
            //            await BlobHelper.WriteStateToBlob(record);
            //            time = record.TimeStamp;
            //            if ((i + 1) % 100 == 0 || i + 1 == list.Count)
            //            {
            //                Console.WriteLine("Database:jym-grains-{0}, Table:grains_{1}, Count:{2}".FormatWith(databaseIndex, grainsIndex, i + 1));
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(time);
            //        Console.WriteLine("----------------------------------------------------------------------------------------------------");
            //        Console.WriteLine("----------------------------------------------------------------------------------------------------");
            //        Console.WriteLine("----------------------------------------------------------------------------------------------------");
            //        Console.WriteLine("----------------------------------------------------------------------------------------------------");
            //        Console.WriteLine("----------------------------------------------------------------------------------------------------");
            //        Console.WriteLine("----------------------------------------------------------------------------------------------------");
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "stop at timestamp:{0}".FormatWith(time));
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
            //        File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", ex.GetExceptionString());

            //        Console.WriteLine();
            //        throw;
            //    }
            //    Console.WriteLine("End grains_{0}".FormatWith(grainsIndex));
            //}
            List<string> list = await GetAllIds(databaseIndex, grainsIndex, timeStamp);
            using (JYMDBContext context = new JYMDBContext(connectionString.FormatWith(databaseIndex)))
            {
                string sqlTemplate = "select top 1 * from Grains_{0} where Id = '{1}' ORDER BY [TimeStamp] desc";

                Console.WriteLine("Start grains_{0}".FormatWith(grainsIndex));
                long time = 0L;
                try
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        GrainStateRecord record = await context.Database.SqlQuery<GrainStateRecord>(sqlTemplate.FormatWith(grainsIndex, list[i])).FirstOrDefaultAsync();
                        await BlobHelper.WriteStateToBlob(record);
                        time = record.TimeStamp;
                        if ((i + 1) % 100 == 0 || i + 1 == list.Count)
                        {
                            Console.WriteLine("Database:jym-grains-{0}, Table:grains_{1}, Count:{2}".FormatWith(databaseIndex, grainsIndex, i + 1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(time);
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "stop at timestamp:{0}".FormatWith(time));
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", "-------------------------------------------------------");
                    File.AppendAllText(Directory.GetCurrentDirectory() + $"{databaseIndex}-{grainsIndex}log.txt", ex.GetExceptionString());

                    Console.WriteLine();
                    throw;
                }
                Console.WriteLine("End grains_{0}".FormatWith(grainsIndex));
            }
        }
    }
}