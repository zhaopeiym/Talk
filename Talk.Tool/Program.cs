using IoTClient.Clients.PLC;
using IoTClient.Common.Enums;
using System;
using Talk.Redis;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Talk.Tool
{
    class Program
    {
        private static RedisManager redis;
        private static SiemensClient client;
        static void Main(string[] args)
        {
            foreach (var item in args)
            {
                Console.WriteLine(item);
            }
            newStart:
            Console.WriteLine("您可以连接redis或者plc");
            var command = ReadLine();

            #region redis
            if (command.StartsWith("redis"))
            {
                var items = command.Split(' ');
                var ip_port = items.Length >= 2 ? items[1] : "127.0.0.1:6379";
                var ip_port_split = ip_port.Split(':');
                var ip = ip_port_split[0];
                var port = ip_port_split.Length >= 2 ? ip_port_split[1] : "6379";
                var dbindex = items.Length >= 3 ? items[2] : "1";
                var config = $"{ip}:{port},allowAdmin=true,password=,syncTimeout=15000,defaultdatabase={dbindex}";
                var initialTime = DateTime.Now;
                try
                {
                    redis = new RedisManager(config, int.Parse(dbindex));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"连接失败，{ex.Message}");
                    goto newStart;
                }
                var timeConsuming = (DateTime.Now - initialTime).TotalMilliseconds;
                Console.WriteLine($"连接成功\t\t\t\t耗时：{timeConsuming}ms");
                getOrSet:
                command = ReadLine();
                items = command.Split(' ');
                if (command.StartsWith("get"))
                {
                    var key = items.Length >= 2 ? items[1] : "";

                    var numberList = command.Split("-n");
                    var number = numberList.Length >= 2 ? int.Parse(numberList[1]) : 1;
                    double sunTime = 0;
                    for (int i = 0; i < number; i++)
                    {
                        initialTime = DateTime.Now;
                        var value = redis.GetString(key);
                        timeConsuming = (DateTime.Now - initialTime).TotalMilliseconds;
                        sunTime += timeConsuming;
                        Console.WriteLine($"[读取 {key} 成功]：{value}\t\t耗时：{timeConsuming}ms");
                    }
                    if (number > 1)
                        Console.WriteLine($"\t\t\t\t\t平均耗时：{sunTime / number}ms");
                }
                else if (command.StartsWith("set"))
                {
                    var key = items.Length >= 2 ? items[1] : "";
                    var value = items.Length >= 3 ? items[2] : "";
                    var numberList = command.Split("-n");
                    var number = numberList.Length >= 2 ? int.Parse(numberList[1]) : 1;
                    double sunTime = 0;
                    for (int i = 0; i < number; i++)
                    {
                        initialTime = DateTime.Now;
                        var isOK = redis.Set(key, value);
                        timeConsuming = (DateTime.Now - initialTime).TotalMilliseconds;
                        sunTime += timeConsuming;
                        if (isOK)
                            Console.WriteLine($"[写入 {key} 成功]：{value}\t\t耗时：{timeConsuming}ms");
                        else
                            Console.WriteLine($"[写入 {key} 失败]：{value}\t\t耗时：{timeConsuming}ms");
                    }
                    if (number > 1)
                        Console.WriteLine($"\t\t\t\t\t平均耗时：{sunTime / number}ms");
                }
                goto getOrSet;
            }
            #endregion

            else if (command.StartsWith("plc"))
            {
                var items = command.Split(' ');
                var ip_port = items.Length >= 2 ? items[1] : "127.0.0.1:102";
                var ip_port_split = ip_port.Split(':');
                var ip = ip_port_split[0];
                var port = ip_port_split.Length >= 2 ? ip_port_split[1] : "102";
                Console.WriteLine("请输入您好连接的版本：");
                Console.WriteLine("1、S7-200Smar");
                Console.WriteLine("2、S7-200");
                var version = ReadLine();
                switch (version)
                {
                    case "1":
                        client = new SiemensClient(SiemensVersion.S7_200Smart, ip, int.Parse(port));
                        break;
                    case "2":
                        client = new SiemensClient(SiemensVersion.S7_200, ip, int.Parse(port));
                        break;
                    default:
                        client = new SiemensClient(SiemensVersion.S7_200Smart, ip, int.Parse(port));
                        break;
                }

                var initialTime = DateTime.Now;
                var result = client.Open();
                var timeConsuming = (DateTime.Now - initialTime).TotalMilliseconds;
                if (result.IsSucceed)
                    Console.WriteLine($"连接成功\t\t\t\t耗时：{timeConsuming}ms");
                else
                {
                    Console.WriteLine($"连接失败，{result.Err}");
                    goto newStart;
                }

                getOrSet:
                command = ReadLine();
                items = command.Split(' ');
                if (command.StartsWith("get"))
                {
                    var key = items.Length >= 2 ? items[1] : "";

                    var numberList = command.Split("-n");
                    var number = numberList.Length >= 2 ? int.Parse(numberList[1]) : 1;
                    double sunTime = 0;
                    for (int i = 0; i < number; i++)
                    {
                        initialTime = DateTime.Now;
                        var resultValue = client.ReadFloat(key);
                        timeConsuming = (DateTime.Now - initialTime).TotalMilliseconds;
                        sunTime += timeConsuming;
                        if (resultValue.IsSucceed)
                            Console.WriteLine($"[读取 {key} 成功]：{resultValue.Value}\t\t耗时：{timeConsuming}ms");
                        else
                            Console.WriteLine($"[读取 {key} 失败]：{resultValue.Value}\t\t耗时：{timeConsuming}ms");

                    }
                    if (number > 1)
                        Console.WriteLine($"\t\t\t\t平均耗时：{sunTime / number}ms");

                    var readList = command.Split("-r");
                    var readNumber = readList.Length >= 2 ? ushort.Parse(readList[1]) : (ushort)1;
                    var resules = client.ReadUInt32(key, readNumber);
                    foreach (var item in resules.Value)
                    {
                        Console.WriteLine($"[读取 {item.Key} 成功]：{item.Value}\t\t耗时：{resules.TimeConsuming}ms");
                    }
                }
                goto getOrSet;
            }

            goto newStart;
        }

        static List<string> CommandList = new List<string>();
        static int CommandIndex = 0;
        static int GetCommandIndex(bool up)
        {
            if (up)
            {
                CommandIndex--;
                if (CommandIndex < 0) CommandIndex = CommandList.Count - 1;
            }
            else
            {
                CommandIndex++;
                if (CommandIndex >= CommandList.Count) CommandIndex = 0;
            }
            return CommandIndex;
        }
        #region ReadLine
        static string ReadLine()
        {
            StringBuilder readString = new StringBuilder();
            while (true)
            {
                var i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (i.Key == ConsoleKey.Backspace)
                {
                    if (readString.Length > 0)
                    {
                        readString.Remove(readString.Length - 1, 1);
                    }
                    Console.Write("\b \b");
                }
                else if (i.Key == ConsoleKey.UpArrow)
                {
                    if (CommandList.Any())
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            Console.Write("\b \b");
                        }
                        Console.Write(CommandList[GetCommandIndex(true)]);
                    }
                }
                else if (i.Key == ConsoleKey.DownArrow)
                {
                    if (CommandList.Any())
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            Console.Write("\b \b");
                        }
                        Console.Write(CommandList[GetCommandIndex(false)]);
                    }
                }
                else
                {
                    readString.Append(i.KeyChar);
                    //Console.WriteLine(i.Key);
                    Console.Write(i.KeyChar);
                }
            }
            var command = readString.ToString();
            CommandList.Add(command);
            CommandIndex = CommandList.Count - 1;
            return command;
        }
        #endregion
    }
}
