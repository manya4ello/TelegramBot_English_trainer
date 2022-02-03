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
            
            Console.WriteLine($"последняя команда:{ mes}, статус: {BotLogic.chatstatus}");
            
            switch (BotLogic.chatstatus)
            {
                case ChatStatus.Status.Dic:
                    {
                        await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Введите значение на русском:");
                        BotLogic.chatstatus = ChatStatus.Status.AddWord;
                        break;
                    }
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
                            $"\n*Рус:*\t{BotLogic.wordtoadd.Russian}\t*Анг:*\t{BotLogic.wordtoadd.English}\t*Тема:*\t({BotLogic.wordtoadd.Topic})" +
                            $"\n Добавляем в словарь?";
                        await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: text, parseMode: ParseMode.Markdown);
                        
                        BotLogic.chatstatus = ChatStatus.Status.AddedTopic;
                        ICommand confirm = new WordConfirm();
                        confirm.Execute(botClient, conversation);
                        break;
                    }
                case ChatStatus.Status.AddedTopic:
                    {
                        if (mes == WordConfirm.Yes)
                        {
                            bool dubl = false;
                            foreach (Word word in Dictionary.Vocabulary)
                            {
                                if (word.Russian == BotLogic.wordtoadd.Russian)
                                {
                                    dubl = true;
                                    await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "К сожалению, данное слово уже есть в словаре");
                                }
                            }

                            if (!dubl)
                            { 
                                Dictionary.Vocabulary.Add(BotLogic.wordtoadd);
                                await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Слово успешно добавлено");
                            }
                        }

                        if (mes == WordConfirm.No)
                        {
                            await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "Слово не добавлено");
                        }

                        BotLogic.wordtoadd = new Word();
                        BotLogic.chatstatus = ChatStatus.Status.Dic;
                        ICommand mainmenu = new Commands.Mainmenu();
                        mainmenu.Execute(botClient, conversation);
                        ICommand show = new Commands.Show();
                        show.Execute(botClient, conversation);
                        break;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(chatId: conversation.GetId(), text: "А хз, что произошло");
                        BotLogic.chatstatus = ChatStatus.Status.Root;
                        break;
                    }
            }
           
            Console.WriteLine($"На выходе {BotLogic.chatstatus}");
        }
    }
}
