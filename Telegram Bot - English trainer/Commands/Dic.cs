﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class Dic : Command, ICommand
    {
        public string CommandName = "Словарь";

        public string CommandCode = "/dic";
        public int Id = 1;
        public int Father = 0;

        public Dic()
            {
            CommandName = "Словарь";
            CommandCode = "/dic";
            Id = 1;
            Father = 0;
            Console.WriteLine($"Создана команда {CommandName}");
        }
        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}