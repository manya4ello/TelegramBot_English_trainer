using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    /// <summary>
    /// Абстрактный класс, определяющий команду бота
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Имя команды (как она выводится в меню)
        /// </summary>
        public string CommandName { get; set; }
        /// <summary>
        /// Команда, которую надо ввести для инициации команды
        /// </summary>
        public string CommandCode { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// Определяет положение команды в структуре с помощью указания ID команды верхнего уровня
        /// </summary>
        public int Father { get; set; }


        /// <summary>
        /// Определяет положение команды в структуре с помощью указания уровня (статуса работы)
        /// </summary>
        public ChatStatus.Status Level { get; set; }

        public Task<ChatStatus.Status> Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            throw new NotImplementedException();
        }
    }
}
