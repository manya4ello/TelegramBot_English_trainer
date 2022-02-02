using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class DelWord : Command, ICommand
    {
        
            public string CommandName = "Удалть слово";

            public string CommandCode = "/del";
            public int Id = 12;
            public int Father = 1;

            public Task Execute()
            {
                throw new NotImplementedException();
            }
        
    }
}
