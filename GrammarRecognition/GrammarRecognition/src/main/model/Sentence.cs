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
        private String[] words;
        private String title;//first sentence in the paragraph

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        private int minSeq = Int32.MaxValue;

        public int MinSeq
        {
            get { return minSeq; }
            set { minSeq = value; }
        }

        public String[] Words
        {
            get { return words; }
            set { words = value; }
        }
        public Sentence(String text)
        {
            this.text = text;
            abbreviations = new List<string>();
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
            Regex r = new Regex("[^A-Za-z0-9 \r\n\t-'’,:：]");
            String rawtext=  r.Replace(text, "");
            r = new Regex("[ \t\r\n:：]+");
            rawtext = r.Replace(rawtext, " ");
            String[] tmp = rawtext.Split(new char[] { ' ', '\t', '\n' });
            List<String> list = new List<String>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!tmp[i].Trim().Equals(""))
                {
                    if(tmp[i].StartsWith(",")){
                        list.Add(",");
                        list.Add(tmp[i].Replace(",",""));
                    }else if(tmp[i].EndsWith(",")){
                        list.Add(tmp[i].Replace(",", ""));
                        list.Add(",");
                    }else
                        list.Add(tmp[i]);
                }
            }
            words = list.ToArray();
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
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
