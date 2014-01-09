using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnglishTest
{
    class GlobalData
    {
        public enum Exam_Type { WORD_LIST, JUMP_WORD };
        public static List<WordListModel> WordList = new List<WordListModel>();
        public static string original_article;
        public static Exam_Type examType;
        public static WordListModel selectedWordList;
        public static int jumpNum;
        public static int selectNum;
        public static string WORD_LIST_CONFIG_PATH = "data\\wordlist";
        public static string ASSOCIATE_LIST_PATH = "data\\associate.txt";
    }
}
