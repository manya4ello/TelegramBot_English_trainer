﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class AddWord : Command, ICommand
    {
        public string CommandName ="Добавить слово";

        public string CommandCode = "/add";
        public int Id = 11;
        public int Father = 1;

        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
