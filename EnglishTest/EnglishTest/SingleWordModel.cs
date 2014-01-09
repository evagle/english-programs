using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnglishTest
{
    class SingleWordModel
    {
        public enum WordType { NONSELECTED, SELECTED,  PUNCTUATION};
        private string word;

        public string Word
        {
            get { return word; }
            set { word = value; }
        }
        public WordType type;

        public WordType Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
