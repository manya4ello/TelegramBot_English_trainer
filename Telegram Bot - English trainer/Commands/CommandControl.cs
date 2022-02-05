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

            CommandsRange.Add(new Mainmenu());
            CommandsRange.Add(new Dic());
            CommandsRange.Add(new AddWord());
            CommandsRange.Add(new DelWord());
            CommandsRange.Add(new Test());
           CommandsRange.Add(new ShowDic());
            CommandsRange.Add(new About());
            CommandsRange.Add(new LoadDic());
            CommandsRange.Add(new Help());
            CommandsRange.Add(new TestStop());
            CommandsRange.Add(new Instructions());
        }

        public Dictionary<long, ICommand> GetChildren (ChatStatus.Status status)
        {
            CommandControl all = new CommandControl();
            var Children = new Dictionary<long,ICommand>();
            foreach (var command in all.CommandsRange)  
                if (command.Level == status)
                    Children.Add(command.Id, command);
           

            return Children;
        }

    }
}
