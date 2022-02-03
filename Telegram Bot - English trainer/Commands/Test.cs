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
            Id = 20;
            Father = 1;
            Level = ChatStatus.Status.Root;
        }
        public Task Execute(Chat chat)
        {
            throw new NotImplementedException();
        }
    }
}
