using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class DelWord : Command, ICommand
    {
        
            

        public DelWord()
        {
            CommandName = "Удалть слово";
            CommandCode = "/del";
            Id = 12;
            Father = 1;

    }
            public Task Execute()
            {
                throw new NotImplementedException();
            }
        
    }
}
