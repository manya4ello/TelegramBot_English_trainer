using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot___English_trainer
{
    /// <summary>
    /// Определяет тест и все, что с ним связано
    /// </summary>
    public class Test
    {
        public enum Direction
        {
            NotDef =0,
            RusEng = 1,
            EngRus,
            Rand
        }
       
        /// <summary>
        /// Кол-во вопросов в тесте
        /// </summary>
        public int MaxNofQuestions { get; set; }
        /// <summary>
        /// Направление тестирования
        /// </summary>
        public Direction direction {get;set;}
        /// <summary>
        /// Номер текущего вопроса
        /// </summary>
        public int CurQuest { get; set; }
        /// <summary>
        /// Направление тестирования текущего вопроса
        /// </summary>
        public bool CurQuestRusEng { get; set; }   
        /// <summary>
        /// Оценка: кол-во правильно отвеченных вопросов
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// Текущее заданное слово
        /// </summary>
        public Word AskedWord { get; set; }
        /// <summary>
        /// Кол-во альтернативных, неправильных ответов, которые добавятся к правильному
        /// </summary>
        public int NumberOfWrongQuest { get; set; } 
        public Test()
            {
                MaxNofQuestions = 5;
            direction = Direction.NotDef;
            NumberOfWrongQuest = 4;
              }
        /// <summary>
        /// Проверка правильности ответа
        /// </summary>
        /// <param name="answer">Полученный ответ</param>
        /// <param name="RusEng">Направление теста с русского на английский?</param>
        /// <returns></returns>
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
 
    


