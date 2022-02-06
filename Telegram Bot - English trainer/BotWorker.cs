

namespace Telegram_Bot___English_trainer
{
    /// <summary>
    /// Создает и запускает бота
    /// </summary>
    internal class BotWorker
    {
        static ITelegramBotClient botClient;
        static CancellationTokenSource cts;
        static BotLogic botLogic;
        public void Initialize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            botLogic = new BotLogic(botClient);
        }

        public async void Work()
        {
            
            
            using var cts = new CancellationTokenSource();
            ReceiverOptions receiverOptions = new() { AllowedUpdates = { } };


            botClient.StartReceiving(botLogic.HandleUpdateAsync,
                               botLogic.HandleErrorAsync,
                               receiverOptions,
                               cts.Token);

            var me = await botClient.GetMeAsync();
            Console.Title = me.Username ?? "My awesome Bot";

            Console.WriteLine($"{DateTime.Now}: Start listening for @{me.Username}");
            Console.ReadLine();
        
            cts.Cancel();
        }
    }
}
