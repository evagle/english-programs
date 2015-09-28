using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GrammarRecognition.src.main.model
{
    class Sentence : IComparable
    {
        private string text;
        private List<string> abbreviations;
        private List<string> words;
        private String title;//first sentence in the paragraph
        private int minSeq = Int32.MaxValue;

        public Sentence()
        {
        }
        public Sentence(String text)
        {
            this.text = text;
            abbreviations = new List<string>();
            words = new List<string>();
            splitToWords();
        }
        public void addGrammar(Grammar grammar)
        {
            if (!abbreviations.Contains(grammar.Abbreviation))
            {
                if (grammar.Seq < minSeq)
                    minSeq = grammar.Seq;
                abbreviations.Add(grammar.Abbreviation);
            }
        }
        public String abbrsToString()
        {
            if (abbreviations.Count == 0)
                return "";
            else
            {
                String ret = " [ " + abbreviations[0];
                for (int i = 1; i < abbreviations.Count; i++)
                    ret += "," + abbreviations[i];
                ret += " ] ";
                return ret;
            }
        }
        public String ToStringWithAbbr()
        {
            if (abbreviations.Count > 0)
            {
                String str = "(";
                foreach (String s in abbreviations)
                {
                    str += s + ",";
                }
                str += ")";
                return text + str;
            }
            else
                return text;
        }
        public void splitToWords()
        {
            foreach (string line in text.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                Regex r = new Regex("[a-zA-Z]+");
                if (r.IsMatch(line))
                {
                    string[] tmp = this.lineToWords(line);
                    words.AddRange(tmp);
                }
            }
        }

        public string[] lineToWords(string str)
        {
            Regex r = new Regex("[^A-Za-z0-9 \r\n\t-'’,?:：.!]");
            String rawtext = r.Replace(str, "");
            r = new Regex("[ \t\r\n:：]+");
            rawtext = r.Replace(rawtext, " ");
            rawtext = rawtext.Replace(",", " , ");
            rawtext = rawtext.Replace("?", " ? ");
            rawtext = rawtext.Replace(".", " . ");
            rawtext = rawtext.Replace("!", " ! ");

            return rawtext.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public Sentence subSentence(int start)
        {
            Sentence sub = new Sentence();
            sub.words = new List<string>();
            for (int i = start; i < this.words.Count; i++ )
            {
                sub.Words.Add(this.words[i]);
            }
            return sub;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public String Title
        {
            get { return title; }
            set { title = value; }
        }



        public int MinSeq
        {
            get { return minSeq; }
            set { minSeq = value; }
        }

        public List<String> Words
        {
            get { return words; }
            set { words = value; }
        }

        public List<string> Abbreviations
        {
            get { return abbreviations; }
            set { abbreviations = value; }
        }
        #region IComparable 成员

        public int CompareTo(object obj)
        {
            return -this.minSeq + ((Sentence)obj).MinSeq;

        }

        #endregion
    }
}
