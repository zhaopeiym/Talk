using System;
using Talk.Redis;

namespace Talk.Tool
{
    class Program
    {
        private static RedisManager redis;
        static void Main(string[] args)
        {
            foreach (var item in args)
            {
                Console.WriteLine(item);
            }
        }
    }
}
