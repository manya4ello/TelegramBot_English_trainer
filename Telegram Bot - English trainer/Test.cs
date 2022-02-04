using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    public class Test
    {
        public enum Direction
        {
            NotDef =0,
            RusEng = 1,
            EngRus,
            Rand
        }
       
        public int MaxNofQuestions { get; set; }
        public Direction direction {get;set;}
        public int CurQuest { get; set; }
        public bool CurQuestRusEng { get; set; }    
        public int score { get; set; }
        public Word AskedWord { get; set; }

        public int NumberOfWrongQuest { get; set; } 
        public Test()
            {
                MaxNofQuestions = 5;
            direction = Direction.NotDef;
            NumberOfWrongQuest = 4;
              }

        public bool CheckAnswer (string answer, bool RusEng)
        {
            bool result = false;

            if (RusEng)
                if (AskedWord.English == answer)
                    result = true;
            if (!RusEng)
                if (AskedWord.Russian == answer)
                    result = true;

            return result;
        }
    }

}
 
    


