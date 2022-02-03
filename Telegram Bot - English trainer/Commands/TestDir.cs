using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class TestDir : Command, ICommand
    {
        public static readonly string Yes = "Да";
        public static readonly string No = "Нет";
        public TestDir()
        {
            CommandName = "Направление теста";
            CommandCode = "/testdir";
            Id = 200;
            Father = 20;
        }
        public Task Execute(Chat chat)
        {
            throw new NotImplementedException();
        }
    }
}
