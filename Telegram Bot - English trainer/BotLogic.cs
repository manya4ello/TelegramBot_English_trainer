using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_Bot___English_trainer
{
    /// <summary>
    /// Класс работы с логикой робота
    /// </summary>
    internal class BotLogic
    {
        private ITelegramBotClient botClient;

        private Dictionary<long, Conversation> chatList;
       
                

        public BotLogic(ITelegramBotClient botClientRes)
        {

            chatList = new Dictionary<long, Conversation>();
            
            botClient = botClientRes;
                       
        }

        
        /// <summary>
        /// Запускается если произошло исключение
        /// </summary>
        /// <param name="botClient">Текущий бот</param>
        /// <param name="exception">Произошедшее исключение</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>        
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

        /// <summary>
        /// Запускается если получено изменение непонятного типа
        /// </summary>
        /// <param name="botClient">Текущий бот</param>
        /// <param name="exception">Произошедшее исключение</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>    
        public async Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {

            Console.WriteLine("Я ничего не понял. Давайте по сценарию, как договаривались");

        }

        /// <summary>
        /// Проверяет является ли текстовое сообщение командой
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="message">Полученное сообщение</param>
        /// <returns></returns>
        public async Task CheckIFTXTCommand(ITelegramBotClient botClient, Message message)
        {
            var chatID = message.Chat.Id;            

            //если чата нет в списке - добавляем
            if (!chatList.ContainsKey(chatID))
            {
                var newchat = new Conversation(message.Chat);                             

                chatList.Add(chatID, newchat);

                Console.WriteLine($"{DateTime.Now}: создан чат {chatID}");

                //добавляем кнопки в клавиатуру для удобства
                ICommand mainmenu = new Commands.Mainmenu();
                mainmenu.Execute(botClient,chatList[chatID]);
                               

            }

            Console.WriteLine($"{DateTime.Now}: чат {chatID}: текст: {message.Text}");

            var chat = chatList[chatID];
            var chatstatus = chat.chatStatus;
            
            chat.AddMessage(message);

           
            ICommand curCommand;

            ///если команда, выполняем
            if (chatList[chatID].IfCommand(message.Text, out curCommand))
                await WorkWithCommand(botClient, chatID, curCommand);
            ///если не команда - скорее всего это ответ на какой-то вопрос
            else
            {
                if (chatstatus == ChatStatus.Status.AddWord || chatstatus == ChatStatus.Status.AddedRus || chatstatus == ChatStatus.Status.AddedEng
               || chatstatus == ChatStatus.Status.AddedTopic)
                {
                    await AddWordLogic(botClient, chatID);
                    return;
                }
                if (chatstatus == ChatStatus.Status.DelWord || chatstatus == ChatStatus.Status.DelConf)
                {
                    await DelWordLogic(botClient, chatID);
                    return;
                }
                if (chatstatus == ChatStatus.Status.Test || chatstatus == ChatStatus.Status.TestInProcess)
                {
                    await TestLogic(botClient, chatID);
                    return; 
                }
                //если сообщение не распознанно - выводим инструкции
                Console.WriteLine($"{DateTime.Now}: чат:{chatID}: Принято неопознанное сообщение - {message.Text}");
                var command = new Commands.Instructions();
                await WorkWithCommand(botClient, chatID, command);
            }

        }
        /// <summary>
        /// Обрабатывает ответ кнопкой
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="query">Ответ</param>
        /// <returns></returns>
        public async Task CheckCallBackQuerry(ITelegramBotClient botClient, CallbackQuery query)
        {
            var chatID = query.Message.Chat.Id;

            if (!chatList.ContainsKey(chatID))
            {
                var newchat = new Conversation(query.Message.Chat);
                Console.WriteLine($"{DateTime.Now}: создан чат {chatID}");
                chatList.Add(chatID, newchat);

                Commands.Show show = new Commands.Show();
                show.Execute(botClient, newchat);

            }

            var chat = chatList[chatID];
            chat.AddMessage(query.Message);

            Console.WriteLine($"{DateTime.Now}: чат {chatID}: CallBackQuerry : {query.Data}");

            ICommand curCommand;


            if (chatList[chatID].IfCommand(query.Data, out curCommand))
            {
                
                await WorkWithCommand(botClient, chatID, curCommand);
                await botClient.AnswerCallbackQueryAsync(query.Id);
            }

        }

        /// <summary>
        /// Задача по обработке любого входящего обновления
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="update">Полученное обновление</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                //Проверяем - если это первый запуск, выводим инструкцию
                long? newchatid = null;
                if (update.Type == UpdateType.Message)
                    newchatid = update.Message.Chat.Id;
                if (update.Type == UpdateType.CallbackQuery)
                    newchatid = update.CallbackQuery.Message.Chat.Id;
                if (newchatid != null)
                {
                    if (chatList[(long)newchatid].firstimerun)
                    {
                        
                        var help = new Commands.Help();
                        string text = help.Content;
                        await botClient.SendTextMessageAsync(chatId: newchatid, text: text,parseMode: ParseMode.Markdown  );
                        chatList[(long)newchatid].firstimerun = false;
                    }
                   
                }
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }

        }

        /// <summary>
        /// Задача по обработке команды
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="chatid">Номер чата</param>
        /// <param name="command">Комманда</param>
        /// <returns></returns>
        private async Task WorkWithCommand(ITelegramBotClient botClient, long chatid, ICommand command)
        {
            Console.WriteLine($"{DateTime.Now}: чат {chatid}: WorkWithCommand {command.CommandName}, статус чата: {chatList[chatid].chatStatus}");

            var chat = chatList[chatid];

            chat.actualCommands = chat.Commands.GetChildren(chatList[chatid].chatStatus);

            chatList[chatid].chatStatus = await command.Execute(botClient, chat);
                        
            chat.actualCommands = chat.Commands.GetChildren(chatList[chatid].chatStatus);

            ICommand check = new Commands.Mainmenu();
            if (command.Id == check.Id)
                return;
             check = new Commands.Show();
            if (command.Id != check.Id)                        
               chatList[chatid].chatStatus = await check.Execute(botClient, chat);
            
        }

        /// <summary>
        /// Задача по добавлению слова в словарь
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="chatid">номер чата</param>
        /// <returns></returns>
        private async Task AddWordLogic(ITelegramBotClient botClient, long chatid)
        {
            string mes = chatList[chatid].GetLastMessage();
            var chatstatus = chatList[chatid].chatStatus;
            var wordtoad = chatList[chatid].wordtoadd;

            Console.WriteLine($"{DateTime.Now}: чат:{chatid}: AddWordLogic: команда:{ mes}, статус: {chatstatus}");

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
                                    chatList[chatid].chatStatus = ChatStatus.Status.Dic;
                                }
                            }

                            if (!dubl)
                            {
                                chatList[chatid].dictionary.Vocabulary.Add(chatList[chatid].wordtoadd);
                                await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово успешно добавлено");
                                chatList[chatid].chatStatus = ChatStatus.Status.Dic;
                            }
                        }

                        if (mes == Commands.WordConfirm.No)
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: "Слово не добавлено");
                            chatList[chatid].chatStatus = ChatStatus.Status.Dic;
                        }

                        chatList[chatid].wordtoadd = new Word();
                        //chatList[chatid].chatStatus = ChatStatus.Status.Dic;

                        ICommand mainmenu = new Commands.Mainmenu();
                        await WorkWithCommand(botClient, chatid, mainmenu);

                        ICommand dic = new Commands.Dic();
                        await WorkWithCommand(botClient, chatid, dic);
                        Console.WriteLine($"{DateTime.Now}: чат:{chatid}: AddWordLogic завершение работы, статус: {chatList[chatid].chatStatus}");
                        


                        break;
                    }
                default:
                    {
                        await botClient.SendTextMessageAsync(chatId: chatid, text: "А хз, что произошло");
                        chatstatus = ChatStatus.Status.Root;
                        break;
                    }
            }
                        
            chatList[chatid].chatStatus = chatstatus;
        }

        /// <summary>
        /// Задача по проведению теста
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="chatid">номер чата</param>
        /// <returns></returns>
        private async Task TestLogic(ITelegramBotClient botClient, long chatid)
        {
           Random random = new Random();    
            string mes = chatList[chatid].GetLastMessage();
            
            var wordtoad = chatList[chatid].wordtoadd;

            Console.WriteLine($"{DateTime.Now}: чат:{chatid}: TestLogic: команда:{ mes}, статус: {chatList[chatid].chatStatus}");

            switch (chatList[chatid].chatStatus)
            {
                //Начало теста
                case ChatStatus.Status.Test:
                    {
                        chatList[chatid].test.score = 0;
                        chatList[chatid].test.CurQuest = 0;

                        if (mes == Commands.TestDir.RusEng)
                        {
                            chatList[chatid].test.direction = Test.Direction.RusEng;
                            chatList[chatid].test.CurQuestRusEng = true;
                        }

                        if (mes == Commands.TestDir.EngRus)
                        {
                            chatList[chatid].test.direction = Test.Direction.EngRus;
                            chatList[chatid].test.CurQuestRusEng = false;
                        }

                        if (mes == Commands.TestDir.Rand)
                        {
                            chatList[chatid].test.direction = Test.Direction.Rand;
                            var ruseng = false;
                            if (random.Next(1) ==0)
                                ruseng = true;
                            chatList[chatid].test.CurQuestRusEng = ruseng;
                        }

                        await botClient.SendTextMessageAsync(chatId: chatid, text: $"Приступаем к тесту. Вам предстоит ответить на {chatList[chatid].test.MaxNofQuestions} вопросов");
                        chatList[chatid].chatStatus = ChatStatus.Status.TestInProcess;
                                                
                        //определяем первый вопрос, показываем его и печатаем ответы
                        ShowCurrentQuestion(SetQuestion());
                        var stop = new Commands.TestStop();
                        chatList[chatid].actualCommands.Add(stop.Id, stop);

                        break;
                    }                
                case ChatStatus.Status.TestInProcess:
                    {
                        

                        //Оценка результата
                        
                        if (chatList[chatid].test.CheckAnswer(mes, chatList[chatid].test.CurQuestRusEng))
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: "Правильный ответ");
                            chatList[chatid].test.score += 1;
                            
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(chatId: chatid, text: $"К сожалению, вы ошиблись. " +
                                $"\nПравильный ответ: *{chatList[chatid].test.AskedWord.Russian} - {chatList[chatid].test.AskedWord.English}*",parseMode: ParseMode.Markdown);
                        }

                        //Если это был последний вопрос
                        if (chatList[chatid].test.CurQuest == chatList[chatid].test.MaxNofQuestions)
                        {
                            string text = $"Ваш результат - {chatList[chatid].test.score} правильных ответов из {chatList[chatid].test.MaxNofQuestions}";
                            Console.WriteLine($"{DateTime.Now}: чат:{chatid}: TestLogic: окончание теста. Правильных ответов из {chatList[chatid].test.MaxNofQuestions}");
                            await botClient.SendTextMessageAsync(chatId: chatid, text: text);
                            chatList[chatid].test.score = 0;
                            chatList[chatid].test.CurQuest = 1;
                            chatList[chatid].test.direction = Test.Direction.NotDef;
                            chatList[chatid].test.AskedWord = new Word();
                            chatList[chatid].chatStatus = ChatStatus.Status.Root;
                            WorkWithCommand(botClient, chatid, new Commands.Mainmenu());
                            break;
                        }

                        //Задаем новый вопрос и ответы

                        
                        if (chatList[chatid].test.direction == Test.Direction.Rand)
                        {
                            bool qdir = false;
                            var randq = random.Next(100);
                            
                            if (randq < 51)
                                qdir = true;
                            chatList[chatid].test.CurQuestRusEng = qdir;
                        }

                        ShowCurrentQuestion(SetQuestion());

                                             


                        break;
                    }
                default:
                    {
                        //не должно происходить, но если вдруг - меняет статус
                        await botClient.SendTextMessageAsync(chatId: chatid, text: "А хз, что произошло");
                        chatList[chatid].chatStatus = ChatStatus.Status.Root;
                        break;
                    }
            }
                      

            //Выбирает следующий ответ и добавляет к списку неправильных ответов 
            List<string> SetQuestion ()
            {
                var wrong = new List<string>();
                int rnd = random.Next(chatList[chatid].dictionary.Vocabulary.Count);
                chatList[chatid].test.AskedWord = chatList[chatid].dictionary.Vocabulary[rnd];



                if (chatList[chatid].test.CurQuestRusEng)
                {
                    wrong = chatList[chatid].dictionary.GetRandQuestion(true, chatList[chatid].dictionary.Vocabulary[rnd], chatList[chatid].test.NumberOfWrongQuest);
                    wrong.Add(chatList[chatid].dictionary.Vocabulary[rnd].English);

                }
                else
                {
                    wrong = chatList[chatid].dictionary.GetRandQuestion(false, chatList[chatid].dictionary.Vocabulary[rnd], chatList[chatid].test.NumberOfWrongQuest);
                    wrong.Add(chatList[chatid].dictionary.Vocabulary[rnd].Russian);

                }
                                
                wrong.Sort();

                chatList[chatid].test.CurQuest += 1;

                return wrong;
            }

            //выводит вопрос в чат, ответы на клавиатуру
            async void  ShowCurrentQuestion(List<string>answers)
            {
                string lan = "Русском";
                string question = chatList[chatid].test.AskedWord.English;
                if (chatList[chatid].test.CurQuestRusEng)
                {
                    lan = "Английском";
                    question = chatList[chatid].test.AskedWord.Russian;
                }
                
                string text = $"Тема вопроса - {chatList[chatid].test.AskedWord.Topic} " +
                    $"\n как будет на {lan} - {question}?";


                var rkm = new ReplyKeyboardMarkup(new KeyboardButton(String.Empty));
                var rows = new List<KeyboardButton[]>();
                var cols = new List<KeyboardButton>();
                
                for (var Index = 0; Index < answers.Count; Index++)
                {
                    
                    cols.Add(new KeyboardButton(answers[Index]));
                    if (Index % 2 != 0) continue;
                    rows.Add(cols.ToArray());
                    cols = new List<KeyboardButton>();
                    
                }
                rkm.Keyboard = rows.ToArray();
                
               
                await botClient.SendTextMessageAsync(
                    chatid, text, replyMarkup: rkm);

            }
        }
        /// <summary>
        /// Задача по удалению слова из словаря
        /// </summary>
        /// <param name="botClient">Бот</param>
        /// <param name="chatid">Номер чата</param>
        /// <returns></returns>
        private async Task DelWordLogic(ITelegramBotClient botClient, long chatid)
        {
            string mes = chatList[chatid].GetLastMessage();

            var wordtoad = chatList[chatid].wordtoadd;

            Console.WriteLine($"{DateTime.Now}: чат:{chatid}: DelWordLogic: команда:{ mes}, статус: {chatList[chatid].chatStatus}");

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
                                Console.WriteLine($"{DateTime.Now}: чат:{chatid}: DelWordLogic: Успешно удалено слово:{del.Russian}-{del.English}, статус: {chatList[chatid].chatStatus}");
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
                        //не должно произойти
                        await botClient.SendTextMessageAsync(chatId: chatid, text: "А хз, что произошло");
                        chatList[chatid].chatStatus = ChatStatus.Status.Root;
                        break;
                    }
            }
                       

        }
    }
}
