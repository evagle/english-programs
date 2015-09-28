using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace GrammarRecognition.src.main.model
{
    class Paragraph:IComparable
    {
        private String text;

        private List<Sentence> sentences;
        private List<String> abbreviations;

        private int minSeq=Int32.MaxValue;
        public Paragraph(String text)
        {
            //text = text.Replace("\n", "");
            this.text = text;
            sentences = new List<Sentence>();
            abbreviations = new List<string>();
            splitToSentences();
            setSentenceTitle();
        }
        public void setSentenceTitle()
        {
            if(sentences.Count==0)
                return;
            String title=sentences.ElementAt(0).Text ;
            if (title.IndexOf('\r') > 0)
                title = title.Substring(0,title.IndexOf('\r'));
            foreach (Sentence s in sentences)
                s.Title = title;
        }
        public String ToStringWithAbbrAfterSentence()
        {
            String str = "";
            foreach(Sentence s in sentences){
                str += s.ToStringWithAbbr();
            }
            return str;
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
        
        public void splitToSentences()
        {
            HashSet<char> map = new HashSet<char>();
            map.Add('.');
            map.Add('!');
            map.Add('?');
            
            string[] lines = text.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
            String str = "";
            Regex r = new Regex("[a-zA-Z,.?!]+");
            foreach (string line in lines)
            {
                if (r.IsMatch(line))
                {
                    int start = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (i + 2 < line.Length && line.Substring(i, 3) == "...")
                        {
                            i = i + 2;
                            continue;
                        } 
                        if (map.Contains(line[i]))
                        {
                            Sentence sentence = new Sentence(str + line.Substring(start, i + 1 - start));
                            str = "";
                            if (!sentence.Text.Trim().Equals(""))
                                sentences.Add(sentence);
                            start = i + 1;
                        }
                    }
                    if (start < line.Length)
                    {
                        str += line.Substring(start, line.Length - start) + "\n";
                    }
                    else
                    {
                        str += "\r\n";
                    }
                    
                }
                else
                {
                    str += line + "\n";
                }
            }
            if (str != null)
            {
                Sentence sentence = new Sentence(str);
                if (!sentence.Text.Trim().Equals(""))
                    sentences.Add(sentence);
            }
        }
        private bool isNextSentence(char[] chs, int start)
        {

            for (int i = start; i < chs.Length; i++)
            {
                if ((chs[i] >= 'a' && chs[i] <= 'z') || (chs[i] >= 'A' && chs[i] <= 'Z'))
                {
                    if (Char.IsUpper(chs[i])  )
                    {
                        if ((i + 1 < chs.Length && (chs[i + 1] == '\''||chs[i + 1] == ' ' || (chs[i+1] >= 'a' && chs[i+1] <= 'z'))))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }else if(chs[i] >= '0' && chs[i] <= '9')
                    return true;
            }
            return true;
        }

    
        public String Text
        {
            get { return text; }
            set { text = value; }
        }
        public int MinSeq
        {
            get { return minSeq; }
            set { minSeq = value; }
        }
        public List<Sentence> Sentences
        {
            get { return sentences; }
            set { sentences = value; }
        }
        public List<String> Abbreviations
        {
            get { return abbreviations; }
            set { abbreviations = value; }
        }

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            return -this.minSeq + ((Paragraph)obj).MinSeq;
           
        }

        #endregion
    }
}
