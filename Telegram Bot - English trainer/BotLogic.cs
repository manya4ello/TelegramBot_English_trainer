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
       
                

        public BotLogic(ITelegramBotClient botClientRes)
        {

            chatList = new Dictionary<long, Conversation>();
            
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

           
            ICommand curCommand;


            if (chatList[chatID].IfCommand(message.Text, out curCommand))
            {
                Console.WriteLine($"Принята команда из IfCommand {curCommand.CommandName}");
                await WorkWithCommand(botClient, chatID, curCommand);
            }
            else
            {
                if (chatstatus == ChatStatus.Status.AddWord || chatstatus == ChatStatus.Status.AddedRus || chatstatus == ChatStatus.Status.AddedEng
               || chatstatus == ChatStatus.Status.AddedTopic)
                {
                    Console.WriteLine("Чего-то добавляем");
                    await AddWordLogic(botClient, chatID);
                }
                if (chatstatus == ChatStatus.Status.DelWord || chatstatus == ChatStatus.Status.DelConf)
                {
                    Console.WriteLine("Чего-то удаляем");
                    await DelWordLogic(botClient, chatID);
                }
                if (chatstatus == ChatStatus.Status.Test || chatstatus == ChatStatus.Status.TestInProcess)
                {
                    Console.WriteLine("Чего-то тестируем");
                    await TestLogic(botClient, chatID);
                }
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
            Console.WriteLine($"Выполняется команда {command.CommandName}, статус чата: {chatList[chatid].chatStatus}");

            var chat = chatList[chatid];

            chat.actualCommands = chat.Commands.GetChildren(chatList[chatid].chatStatus);

            chatList[chatid].chatStatus = await command.Execute(botClient, chat);
            
            //chat.actualCommands.Clear();
            chat.actualCommands = chat.Commands.GetChildren(chatList[chatid].chatStatus);

            ICommand check = new Commands.Mainmenu();
            if (command.Id == check.Id)
                return;
             check = new Commands.Show();
            if (command.Id != check.Id)                        
               chatList[chatid].chatStatus = await check.Execute(botClient, chat);
            
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
                            foreach (Word word in chatList[chatid].dictionary.Vocabulary)
                            {
                                if (word.Russian == chatList[chatid].wordtoadd.Russian)
                                {
                                    dubl = true;
                                    await botClient.SendTextMessageAsync(chatId: chatid, text: "К сожалению, данное слово уже есть в словаре");
                                }
                            }

                            if (!dubl)
                            {
                                chatList[chatid].dictionary.Vocabulary.Add(chatList[chatid].wordtoadd);
                                await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово успешно добавлено");
                            }
                        }

                        if (mes == Commands.WordConfirm.No)
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово не добавлено");
                        }

                        chatList[chatid].wordtoadd = new Word();
                        chatList[chatid].chatStatus = ChatStatus.Status.Dic;

                        ICommand mainmenu = new Commands.Mainmenu();
                        await WorkWithCommand(botClient, chatid, mainmenu);
                        ICommand dic = new Commands.Dic();
                        await WorkWithCommand(botClient, chatid, dic);

                        


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

        private async Task TestLogic(ITelegramBotClient botClient, long chatid)
        {
           Random random = new Random();    
            string mes = chatList[chatid].GetLastMessage();
            
            var wordtoad = chatList[chatid].wordtoadd;

            Console.WriteLine($"TestLogic: последняя команда:{ mes}, статус: {chatList[chatid].chatStatus}");

            switch (chatList[chatid].chatStatus)
            {

                case ChatStatus.Status.Test:
                    {
                        chatList[chatid].test.score = 0;
                        chatList[chatid].test.CurQuest = 1;

                        if (mes == Commands.TestDir.RusEng)
                        {
                            chatList[chatid].test.direction = Test.Direction.RusEng;
                                                       
                        }

                        if (mes == Commands.TestDir.EngRus)
                        {
                            chatList[chatid].test.direction = Test.Direction.EngRus;

                        }

                        if (mes == Commands.TestDir.Rand)
                        {
                            chatList[chatid].test.direction = Test.Direction.Rand;

                        }

                        await botClient.SendTextMessageAsync(chatId: chatid, text: $"Приступаем к тесту. Вам предстоит ответить на {chatList[chatid].test.MaxNofQuestions} вопросов");
                        chatList[chatid].chatStatus = ChatStatus.Status.TestInProcess;

                        break;
                    }                
                case ChatStatus.Status.TestInProcess:
                    {
                        
                        if (chatList[chatid].test.CurQuest == chatList[chatid].test.MaxNofQuestions)
                        {
                            string text = $"Ваш результат - {chatList[chatid].test.score} правильных ответов из {chatList[chatid].test.MaxNofQuestions}";
                            await botClient.SendTextMessageAsync(chatId: chatid, text: text);
                            chatList[chatid].test.score = 0;
                            chatList[chatid].test.CurQuest = 1;

                            break;
                        }

                        var wrong = new List<string>();
                        int rnd = random.Next(chatList[chatid].dictionary.Vocabulary.Count);
                        switch (chatList[chatid].test.direction)
                        {
                            case Test.Direction.RusEng:
                                {
                                    wrong = chatList[chatid].dictionary.GetRandQuestion(true, chatList[chatid].dictionary.Vocabulary[rnd], 7);
                                    wrong.Add(chatList[chatid].dictionary.Vocabulary[rnd].English);
                                    break;
                                }
                            case Test.Direction.EngRus: 
                                { wrong = chatList[chatid].dictionary.GetRandQuestion(false, chatList[chatid].dictionary.Vocabulary[rnd], 7);
                                    wrong.Add(chatList[chatid].dictionary.Vocabulary[rnd].Russian);
                                    break; 
                                }
                                
                        }
                        wrong.Sort();

                        KeyboardButton[][] buttonList = new KeyboardButton[4][];
                        
                        
                        for (int i=0; i<wrong.Count;)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                buttonList[j][i] = wrong[i];

                            }
                            i += 4;                                
                        }


                        

                       

                        ReplyKeyboardMarkup replyKeyboardMarkup = new(buttonList)
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatid,
                            text: "Подтверждаете?",
                            replyMarkup: replyKeyboardMarkup
                            );


                        chatList[chatid].test.CurQuest++;


                        break;
                    }
                //default:
                //    {
                //        await botClient.SendTextMessageAsync(chatId: chatid, text: "А хз, что произошло");
                //        chatList[chatid].chatStatus = ChatStatus.Status.Root;
                //        break;
                //    }
            }

            Console.WriteLine($"TestLogic: На выходе {chatList[chatid].chatStatus}");
            
        }

        private async Task DelWordLogic(ITelegramBotClient botClient, long chatid)
        {
            string mes = chatList[chatid].GetLastMessage();

            var wordtoad = chatList[chatid].wordtoadd;

            Console.WriteLine($"DelWordLogic: последняя команда:{ mes}, статус: {chatList[chatid].chatStatus}");

            switch (chatList[chatid].chatStatus)
            {

                case ChatStatus.Status.DelWord:
                    {

                        chatList[chatid].wordtodell = mes;
                        bool check = false;
                        string text = string.Empty;

                        foreach (var word in chatList[chatid].dictionary.Vocabulary)
                            if (word.Russian == mes)
                            {
                                text = $"\n*Русское значение:* {word.Russian}\t-\t*Английское значение:* {word.English} \t/\t*Тема:* {word.Topic}";
                                check = true;
                            }
                        if (!check)
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: "Такого слова в словаре нет");
                            chatList[chatid].wordtodell = String.Empty;
                            chatList[chatid].chatStatus = ChatStatus.Status.Dic;
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: $"Найденно слово {text} \n*Удалить?*", parseMode: ParseMode.Markdown);

                            chatList[chatid].chatStatus = ChatStatus.Status.DelConf;

                            ICommand confirm = new Commands.WordConfirm();
                            confirm.Execute(botClient, chatList[chatid]);

                        }

                        break;
                    }
                case ChatStatus.Status.DelConf:
                    {
                        var vocabulary = chatList[chatid].dictionary.Vocabulary;
                        if (mes == Commands.WordConfirm.Yes)
                        {
                            bool check = false;
                            Word del = new Word();

                            foreach (var word in chatList[chatid].dictionary.Vocabulary)
                                if (word.Russian == chatList[chatid].wordtodell)
                                {
                                    del = word;
                                    check = true;
                                }
                            if (check)
                            {
                                chatList[chatid].dictionary.Vocabulary.Remove(del);
                                await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово успешно удалено");
                            }

                        }

                        if (mes == Commands.WordConfirm.No)
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово не удалено");
                        }

                        chatList[chatid].wordtodell = String.Empty;
                        chatList[chatid].chatStatus = ChatStatus.Status.Dic;

                        ICommand mainmenu = new Commands.Mainmenu();
                        await WorkWithCommand(botClient, chatid, mainmenu);
                        ICommand dic = new Commands.Dic();
                        await WorkWithCommand(botClient, chatid, dic);




                        break;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(chatId: chatid, text: "А хз, что произошло");
                        chatList[chatid].chatStatus = ChatStatus.Status.Root;
                        break;
                    }
            }

            Console.WriteLine($"DellWordLogic: На выходе {chatList[chatid].chatStatus}");

        }
    }
}
