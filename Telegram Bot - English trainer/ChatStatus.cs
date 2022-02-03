using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{


    public class ChatStatus
    {

        public enum Status
        {
            Root = 0,
            Dic = 1,
            AddWord = 10,
            AddedRus =101,
            AddedEng =102,
            AddedTopic = 103,
            DelWord =20,
            DelConf=200,
            Test = 2,
            TestInProcess = 22,
            Any = 999
        }
                
    }
}
