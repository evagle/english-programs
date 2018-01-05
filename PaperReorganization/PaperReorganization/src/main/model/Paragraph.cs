using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using PaperReorganization.src.main.logical;

namespace PaperReorganization.src.main.model
{
    class Paragraph:IComparable
    {
        private String text;
        private int paperType;
        private List<Grammar> grammars;
        private double aveSentenceLen;
        private double grammarScore;
        private double newWordsRate;
        private double aveNewWordsFriquence;
        public int aveSentenceLenRank;
        public int grammarScoreRank;
        public int newWordsRateRank;
        public int aveNewWordsFriquenceRank;
        private double finalScore;
        public int totalWordsCount;

        private List<Sentence> sentences;
        private List<String> abbreviations;

        private int minSeq=Int32.MaxValue;
        public Paragraph(String text)
        {
            //text = text.Replace("\n", "");
            this.text = text;
            sentences = new List<Sentence>();
            abbreviations = new List<string>();
            this.grammars = new List<Grammar>();
            splitToSentences();
            initAttributes();
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
            string[] qa = text.Split(new string[]{"答案："}, StringSplitOptions.RemoveEmptyEntries);
            string question = qa[0];
            Regex rx = new Regex("\\(.*\\)");
            MatchCollection mc = rx.Matches(question);
            Hashtable grammarTable = PrepareGrammars.grammarTable;
            foreach (Match m in mc) {
                string gstr = m.ToString().Substring(1, m.Length - 1);
                string[] grammarsStr = gstr.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in grammarsStr) {
                    if (grammarTable.ContainsKey(s) && !this.containsGrammar(s)) {
                        this.grammars.Add((Grammar)grammarTable[s]);
                    }
                }
            }
            this.grammars.Sort();

            string[] lines = question.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
            String str = "";
            Regex r = new Regex("[a-zA-Z,.?!]+");
            foreach (string line in lines)
            {
                if (r.IsMatch(line))
                {
                    int start = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (i + 2 < line.Length && line.Substring(i, 3) == "...") {
                            i = i + 2;
                            continue;
                        } 
                        if (map.Contains(line[i]))
                        {
                            Sentence sentence = new Sentence(str + line.Substring(start, i + 1 - start));
                            str = "";
                            if (!sentence.Text.Trim().Equals("") && sentence.WordsCount > Config.minSentenceLen)
                            {
                                sentences.Add(sentence);
                            }
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
                if (!sentence.Text.Trim().Equals("") && sentence.WordsCount > Config.minSentenceLen)
                {
                    sentences.Add(sentence);
                }
            }


        }

        private bool containsGrammar(string grammarAbbr)
        {
            foreach (Grammar g in grammars) {
                if (g.Abbreviation == grammarAbbr) {
                    return true;
                }
            }
            return false;
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

        public void initAttributes()
        {
            // 计算平均句长
            int wordsCount = 0;
            int newWordsCount = 0;
            int newWordFrequency = 0;
            foreach (Sentence s in sentences)
            {
                wordsCount += s.WordsCount;
                newWordsCount += s.NewWordCount;
                newWordFrequency += s.NewWordFrequency;
                totalWordsCount += s.WordsCount;
            }
            
            if (wordsCount > 0)
            {
                this.aveSentenceLen = wordsCount*1.0 / sentences.Count;
                // 计算净生词率
                this.newWordsRate = newWordsCount * 1.0 / wordsCount;  
            }
            if (newWordsCount > 0) {
                this.aveNewWordsFriquence = newWordFrequency * 1.0 / newWordsCount;
            }

            // 计算语法得分
            int n = (int)Math.Ceiling(wordsCount * 1.0 / Config.grammarDensity);
            
            for (int i = 0; i < Math.Min(n, grammars.Count); i++) {  
                this.grammarScore += grammars[i].Pattern.Length;
            }
            CLog.debug(this.text.Substring(0, Math.Min(30, this.text.Length)) + "\r\n" + wordsCount.ToString() + "\t" + this.aveSentenceLen.ToString() + "\t" + this.newWordsRate.ToString() + "\t" + this.aveNewWordsFriquence.ToString()); 

        }

        public void generateFinalScore()
        {
            int total = Config.grammarQuanZhi + Config.juChangQuanZhi + Config.shengCiCiPingQuanZhi + Config.shengCiLvQuanZhi;
            this.finalScore = (Config.grammarQuanZhi * grammarScoreRank + Config.juChangQuanZhi * aveSentenceLenRank +
                Config.shengCiCiPingQuanZhi * aveNewWordsFriquenceRank + Config.shengCiLvQuanZhi * newWordsRateRank) / (total*1.0);
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

        public double FinalScore
        {
            get { return finalScore; }
            set { finalScore = value; }
        }

        public double GrammarScore
        {
            get { return grammarScore; }
            set { grammarScore = value; }
        }

        public double AveSentenceLen
        {
            get { return aveSentenceLen; }
            set { aveSentenceLen = value; }
        }

        public double NewWordsRate
        {
            get { return newWordsRate; }
            set { newWordsRate = value; }
        }
        public double AveNewWordsFriquence
        {
            get { return aveNewWordsFriquence; }
            set { aveNewWordsFriquence = value; }
        }

        public int PaperType
        {
            get { return paperType; }
            set { paperType = value; }
        }

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            return -this.minSeq + ((Paragraph)obj).MinSeq;
           
        }

        #endregion
    }
}
