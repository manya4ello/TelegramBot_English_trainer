using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class Test : Command, ICommand
    {
        
        public Test()
        {
            CommandName = "Тест";
            CommandCode = "/test";
            Id = 2;
            Father = 0;
        }
        public Task Execute(Chat chat)
        {
            throw new NotImplementedException();
        }
    }
}
