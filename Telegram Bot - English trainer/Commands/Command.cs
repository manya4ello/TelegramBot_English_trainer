using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal abstract class Command : ICommand
    {
        public string CommandName => throw new NotImplementedException();

        public string CommandCode => throw new NotImplementedException();

        public int Id => throw new NotImplementedException();

        public int Father => throw new NotImplementedException();

        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
