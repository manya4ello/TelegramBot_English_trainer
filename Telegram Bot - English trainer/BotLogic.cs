using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_Bot___English_trainer
{
    internal class BotLogic
    {
        private ITelegramBotClient botClient;

        private Dictionary<long, Conversation> chatList;
        public static Dictionary dictionary;
                

        public BotLogic(ITelegramBotClient botClientRes)
        {

            chatList = new Dictionary<long, Conversation>();
            dictionary = new Dictionary();
            dictionary.ReadFile();
            botClient = botClientRes;
                       
        }

        
                
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {

            Console.WriteLine("Я ничего не понял. Давайте по сценарию, как договаривались");

        }

        public async Task CheckIFTXTCommand(ITelegramBotClient botClient, Message message)
        {
            var chatID = message.Chat.Id;
           

            if (!chatList.ContainsKey(chatID))
            {
                var newchat = new Conversation(message.Chat);
                               

                chatList.Add(chatID, newchat);

                ICommand mainmenu = new Commands.Mainmenu();
                mainmenu.Execute(botClient,chatList[chatID]);
                               

            }

            var chat = chatList[chatID];
            var chatstatus = chat.chatStatus;
            
            chat.AddMessage(message);

            if (chatstatus == ChatStatus.Status.AddWord || chatstatus == ChatStatus.Status.AddedRus || chatstatus == ChatStatus.Status.AddedEng
                || chatstatus == ChatStatus.Status.AddedTopic)
            {
                Console.WriteLine("Чего-то добавляем");
                await AddWordLogic(botClient, chatID);
            }



            ICommand curCommand;


            if (chatList[chatID].IfCommand(message.Text, out curCommand))
            {
                Console.WriteLine($"Принята команда из IfCommand {curCommand.CommandName}");
                await WorkWithCommand(botClient, chatID, curCommand);
            }

        }

        public async Task CheckCallBackQuerry(ITelegramBotClient botClient, CallbackQuery query)
        {
            var chatID = query.Message.Chat.Id;

            if (!chatList.ContainsKey(chatID))
            {
                var newchat = new Conversation(query.Message.Chat);

                chatList.Add(chatID, newchat);

                Commands.Show show = new Commands.Show();
                show.Execute(botClient, newchat);

            }

            var chat = chatList[chatID];
            chat.AddMessage(query.Message);

            ICommand curCommand;


            if (chatList[chatID].IfCommand(query.Data, out curCommand))
            {
                Console.WriteLine($"Нажата кнопка {query.Data}");
                await WorkWithCommand(botClient, chatID, curCommand);
                await botClient.AnswerCallbackQueryAsync(query.Id);
            }

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => CheckIFTXTCommand(botClient, update.Message),
                UpdateType.EditedMessage => CheckIFTXTCommand(botClient, update.EditedMessage),
                UpdateType.CallbackQuery => CheckCallBackQuerry(botClient, update.CallbackQuery),
                //UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
                //UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }


        private async Task WorkWithCommand(ITelegramBotClient botClient, long chatid, ICommand command)
        {
            Console.WriteLine($"Выполняется команда {command.CommandName}");

            var chat = chatList[chatid];
            

            switch (command)
            {
                case (Commands.Dic):
                    {
                        chat.actualCommands.Clear();
                        chat.actualCommands = chat.Commands.GetChildren(command.Id);
                        //chatList[chatid].chatStatus = ChatStatus.Status.Dic;
                        break;
                    }
                case (Commands.Test):
                    {
                        chat.actualCommands.Clear();
                        chat.actualCommands = chat.Commands.GetChildren(command.Id);
                        break;
                    }
                case (Commands.Root):
                    {
                        chat.actualCommands.Clear();
                        chat.actualCommands = chat.Commands.GetChildren(1);
                        //chatstatus = ChatStatus.Status.Root;
                        break;
                    }
                case (Commands.AddWord):
                    {
                        Console.WriteLine($"Case пройден. Chatstatus {chatList[chatid].chatStatus}");
                        //chat.actualCommands.Clear();
                        //chat.actualCommands = chat.Commands.GetChildren(1);                       
                        break;
                    }
            }


            chatList[chatid].chatStatus = await command.Execute(botClient, chat);

        }

        private async Task AddWordLogic(ITelegramBotClient botClient, long chatid)
        {
            string mes = chatList[chatid].GetLastMessage();
            var chatstatus = chatList[chatid].chatStatus;
            var wordtoad = chatList[chatid].wordtoadd;

            Console.WriteLine($"AddWordLogic: последняя команда:{ mes}, статус: {chatstatus}");

            switch (chatstatus)
            {
                
                case ChatStatus.Status.AddWord:
                    {
                        chatList[chatid].wordtoadd.Russian = mes;
                        await botClient.SendTextMessageAsync(chatId: chatid, text: "Введите значение на английском:");
                        chatstatus = ChatStatus.Status.AddedRus;
                        break;
                    }
                case ChatStatus.Status.AddedRus:
                    {
                        chatList[chatid].wordtoadd.English = mes;
                        await botClient.SendTextMessageAsync(chatId: chatid, text: "Введите тему:");
                        chatstatus = ChatStatus.Status.AddedEng;
                        break;
                    }
                case ChatStatus.Status.AddedEng:
                    {
                        chatList[chatid].wordtoadd.Topic = mes;
                        string text = $"Проверьте правильность:" +
                            $"\n*Рус:*\t{chatList[chatid].wordtoadd.Russian}\t*Анг:*\t{chatList[chatid].wordtoadd.English}\t*Тема:*\t({chatList[chatid].wordtoadd.Topic})" +
                            $"\n Добавляем в словарь?";
                        await botClient.SendTextMessageAsync(chatId: chatid, text: text, parseMode: ParseMode.Markdown);

                        chatstatus = ChatStatus.Status.AddedTopic;
                        ICommand confirm = new Commands.WordConfirm();
                        confirm.Execute(botClient, chatList[chatid]);
                        break;
                    }
                case ChatStatus.Status.AddedTopic:
                    {
                        if (mes == Commands.WordConfirm.Yes)
                        {
                            bool dubl = false;
                            foreach (Word word in Dictionary.Vocabulary)
                            {
                                if (word.Russian == chatList[chatid].wordtoadd.Russian)
                                {
                                    dubl = true;
                                    await botClient.SendTextMessageAsync(chatId: chatid, text: "К сожалению, данное слово уже есть в словаре");
                                }
                            }

                            if (!dubl)
                            {
                                Dictionary.Vocabulary.Add(chatList[chatid].wordtoadd);
                                await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово успешно добавлено");
                            }
                        }

                        if (mes == Commands.WordConfirm.No)
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово не добавлено");
                        }

                        chatList[chatid].wordtoadd = new Word();
                        chatstatus = ChatStatus.Status.Dic;

                        ICommand mainmenu = new Commands.Mainmenu();
                        mainmenu.Execute(botClient, chatList[chatid]);

                        ICommand show = new Commands.Show();
                        show.Execute(botClient, chatList[chatid]);
                        break;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(chatId: chatid, text: "А хз, что произошло");
                        chatstatus = ChatStatus.Status.Root;
                        break;
                    }
            }

            Console.WriteLine($"AddWordLogic: На выходе {chatstatus}");
            chatList[chatid].chatStatus = chatstatus;
        }
    }
}
