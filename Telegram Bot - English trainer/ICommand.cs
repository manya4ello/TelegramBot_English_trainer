using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    internal interface ICommand
    {
        string CommandName { get; }
        string CommandCode { get; }

        void Execute();
    }
}
