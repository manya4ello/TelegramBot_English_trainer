﻿global using System;
global using Telegram.Bot;
global using Telegram.Bot.Args;
global using Telegram.Bot.Exceptions;
global using Telegram.Bot.Extensions.Polling;
global using Telegram.Bot.Types;
global using Telegram.Bot.Types.Enums;
global using Telegram.Bot.Types.ReplyMarkups;


namespace Telegram_Bot___English_trainer
{
    class Program
    {

        static void Main()
        {

            BotWorker bot = new BotWorker();

            bot.Initialize();
            bot.Work();

            Conversation test = new Conversation(new Chat());





            Console.ReadLine();
       
        }


    }
}