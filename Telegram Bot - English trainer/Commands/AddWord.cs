using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer.Commands
{
    internal class AddWord : Command, ICommand
    {
        
        public AddWord()
        {
            CommandName = "Добавить слово";
            CommandCode = "/add";
            Id = 11;
            Father = 10;
        }
        public new async Task Execute(ITelegramBotClient botClient, Conversation conversation)
        {
            string mes = conversation.GetLastMessage();
           switch (BotLogic.chatstatus)
            {
                case ChatStatus.Status.AddWord:
                {
                        BotLogic.wordtoadd.Russian = mes;
                        await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Введите значение на английском:");
                        BotLogic.chatstatus = ChatStatus.Status.AddedRus;
                        break;
                }
                case ChatStatus.Status.AddedRus:
                 {
                        BotLogic.wordtoadd.English = mes;
                        await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Введите тему:");
                        BotLogic.chatstatus = ChatStatus.Status.AddedEng;
                        break;
                 }
                case ChatStatus.Status.AddedEng:
                    {
                        BotLogic.wordtoadd.Topic = mes;
                        string text = $"Проверьте правильность:" +
                            $"\n*Рус:*\t{BotLogic.wordtoadd.Russian}\t*Анг:*\t{BotLogic.wordtoadd.English}\t*Тема:*\t({BotLogic.wordtoadd.Topic})";
                        await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: text);
                        BotLogic.chatstatus = ChatStatus.Status.AddedTopic;
                        break;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Введите значение на русском:");
                        BotLogic.chatstatus = ChatStatus.Status.AddWord;
                        break;
                    }
            }
           
            Console.WriteLine(BotLogic.chatstatus);
        }
    }
}
