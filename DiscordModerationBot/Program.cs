using System;

/********************************************************************************************
* Class: Program
* Description: Main class to run the bot.
* Developer: Craig Climie
* Last Update: 11/05/2022 at 5.25pm
*******************************************************************************************/

namespace DiscordModerationBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
