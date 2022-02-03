using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    public class Word
    {
        public string Russian { get; set; }    
        public string English { get; set; }    
        public string Topic { get; set; }

        public Word()
        {
            
        }
        public Word (string rus, string eng, string topic)
        {
            Russian = rus;
            English = eng;
            Topic = topic;
        }
    }
}
