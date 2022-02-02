using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class AddWord : Command, ICommand
    {
        
        public AddWord()
        {
            CommandName = "Добавить слово";
            CommandCode = "/add";
            Id = 11;
            Father = 1;
        }
        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
