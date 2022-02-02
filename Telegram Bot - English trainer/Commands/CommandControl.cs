using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    public class CommandControl
    {
        public List<ICommand> CommandsRange;

        public CommandControl()
        {
            CommandsRange = new List<ICommand>();
            CommandsRange.Add(new Dic()); 
            CommandsRange.Add(new AddWord());
            CommandsRange.Add(new DelWord());
            CommandsRange.Add(new Test());
            CommandsRange.Add(new Show());

            ICommand test = new Show();
            Console.WriteLine($"тестовая команда:{test.CommandName}");

            Console.WriteLine("Формируем список команд:");
            foreach (var command in CommandsRange)
                Console.WriteLine($"В список команд добавлена команда: {command.CommandName},/ {command.CommandCode}");
        }

    }
}
