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
           CommandsRange.Add(new ShowDic());
            CommandsRange.Add(new About());

            Console.WriteLine("Формируем список команд:");
            foreach (var command in CommandsRange)
                Console.WriteLine($"В список команд добавлена команда: {command.CommandName},/ {command.CommandCode}");
        }

        public Dictionary<long, ICommand> GetChildren (int father)
        {
            CommandControl all = new CommandControl();
            var Children = new Dictionary<long,ICommand>();
            foreach (var command in all.CommandsRange)  
                if (command.Father == father)
                    Children.Add(command.Id, command);
            //пока не уверен надо ли
            //var command2 = new Show();
            //Children.Add(command2.Id, command2);

            return Children;
        }

    }
}
