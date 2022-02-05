using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    /// <summary>
    /// Статический класс для работы с клавиатурой
    /// </summary>
    public static class KeyboardHelper
    {
        /// <summary>
        /// Помогает сформировать клавиатуру из списка слов
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static ReplyKeyboardMarkup GetKeyboard(List<string> keys)
        {
            var rkm = new ReplyKeyboardMarkup(new KeyboardButton(String.Empty));
            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            foreach (var t in keys)
            {
                cols.Add(new KeyboardButton(t));
                rows.Add(cols.ToArray());
                cols = new List<KeyboardButton>();
            }
            rkm.Keyboard = rows.ToArray();
            return rkm;
        }

        /// <summary>
        /// Убрать текущую клавиатуру
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="chatid"></param>
        public static async void RemoveKeyboard(ITelegramBotClient botClient, long chatid)
        {
            
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatid,
                text: "",
                replyMarkup: new ReplyKeyboardRemove());
        }

    }


}
