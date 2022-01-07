using System;

namespace DiscordLayer
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Bot bot = new Bot();

            Console.WriteLine("Got here");
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
