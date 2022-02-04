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
        public int score { get; set; }
        public Test()
            {
                MaxNofQuestions = 3;
            direction = Direction.NotDef;

              }
    }
}
 
    


