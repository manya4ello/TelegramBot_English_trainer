using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    public static class KeyboardHelper
    {
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
    }
}
