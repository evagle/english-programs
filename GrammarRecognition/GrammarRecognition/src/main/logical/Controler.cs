using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrammarRecognition.src.main.model;
using System.IO;
using System.Collections;
using System.Threading;

namespace GrammarRecognition.src.main.logical
{
    class Controler
    {
        private List<Paragraph> paragraphs;
        private List<Sentence> sentences;
      
        private WordMap wordmap;
        private List<Grammar> grammars;
        private List<Grammar> noReapeatGrammars;
        private int finishedThreadNum;
        private int threadNum;
        private Hashtable gramIndex;

        public Controler(String POSPath, String grammarPath,
            String paragraphPath,String outPath)
        {
            PrepareWordList prepareWordList = new PrepareWordList();
            wordmap = prepareWordList.getWordList(POSPath);

            paragraphs = new List<Paragraph>();
            sentences = new List<Sentence>();
            gramIndex = new Hashtable();
            new PrepareSentences(paragraphPath,paragraphs,sentences);

            PrepareGrammars prepareGrammar = new PrepareGrammars(grammarPath,wordmap);
            grammars = prepareGrammar.getGrammars();
            noReapeatGrammars = new List<Grammar>();
            HashSet<String> nameSet = new HashSet<String>();
            foreach (Grammar g in grammars)
            {
                if (!nameSet.Contains(g.Abbreviation))
                {
                    nameSet.Add(g.Abbreviation);
                    noReapeatGrammars.Add(g);
                }
            }
            
            threadNum = 8;
            finishedThreadNum = 0;
            for (int i = 0; i < threadNum; i++)   
            {
                ParameterizedThreadStart ParStart = new
                ParameterizedThreadStart(this.findPatternInEachSentence);
                Thread myThread = new
                Thread(ParStart);
                object o = i;
                myThread.Start(o);
            }
            while(finishedThreadNum < threadNum){
                Thread.Sleep(2000);
            }
           
            //findPatternInEachSentence();

            printParagraphHasGrammar(outPath+"\\出现过某语法的文章.txt");
            printSentencesHasGrammar(outPath + "\\出现过某语法的句子.txt", false);
            sortByGrammarSeq(outPath + "\\文章按照语法序号降序排序.txt");
            sortByGrammarSeqSentence(outPath + "\\句子按照语法序号降序排序.txt");
            printSentenceWithGrammar(outPath + "\\句子后面加上了它含有的语法.txt");
            grammarAppearFrequency(outPath );
        }
        
        public void grammarAppearFrequency(String outPath)
        {
            Hashtable map = new Hashtable();
            Hashtable grammarMap = new Hashtable();
            for (int i = 0; i < grammars.Count; i++)
            {
                grammarMap[grammars[i].Abbreviation] = grammars[i];
                if (map.ContainsKey(grammars[i].Abbreviation))
                {
                    map[grammars[i].Abbreviation] = ((int)map[grammars[i].Abbreviation]) + grammars[i].frequency;
                }
                else
                {
                    map[grammars[i].Abbreviation] = grammars[i].frequency;
                }
                
            }

            
            List<DictionaryEntry> l = map.Cast<DictionaryEntry>().OrderBy(entry => entry.Value).ToList();
            StreamWriter writer1 = new StreamWriter(outPath + "\\语法出现次数统计.txt", false, Encoding.GetEncoding("gbk"));
            StreamWriter writer2 = new StreamWriter(outPath + "\\词组出现次数统计.txt", false, Encoding.GetEncoding("gbk"));
            foreach (DictionaryEntry entry in l)
            {
                if (((Grammar)grammarMap[entry.Key]).Type == Grammar.T_GRAMMAR)
                {
                    writer1.Write("语法：" + entry.Key + ": 出现次数:  " + entry.Value + "\r\n");
                }else
                    writer2.Write("短语：" + entry.Key + ": 出现次数:  " + entry.Value + "\r\n");
            }
            writer1.Flush();
            writer1.Close();
            writer2.Flush();
            writer2.Close();

        }

        public List<String> getTextByHighestGrammar(String abbr, int type)
        {
            List<String> list = new List<String>();
            foreach (Paragraph p in paragraphs)
            {
                if (type == 1)
                {
                    foreach (Sentence s in p.Sentences)
                    {
                        if (isGrammarTheHighest(s.Abbreviations,abbr))
                            list.Add(s.Text);
                    }
                }
                else
                {
                    if (isGrammarTheHighest(p.Abbreviations,abbr))
                        list.Add(p.Text);
                }
            }
            return list;
        }
        public List<String> getTextHasGrammars(String[] abbr, int type)
        {
            List<String> list = new List<String>();
            foreach (Paragraph p in paragraphs)
            {
                if (type == 1)
                {
                    foreach (Sentence s in p.Sentences)
                    {
                        if (isContainsGrammars(s.Abbreviations, abbr))
                            list.Add(s.Text);
                    }
                }
                else
                {
                    if (isContainsGrammars(p.Abbreviations, abbr))
                        list.Add(p.Text);
                }
            }
            return list;
        }
        private Boolean isContainsGrammars(List<String> abbrList,String[] queryAbbrs)
        {
            List<String> queryList = new List<String>();
            foreach (String s in queryAbbrs)
            {
                if (!s.Trim().Equals(""))
                {
                    queryList.Add(s.Trim());
                }
            }
            if (queryList.Count == 0)
                return false;
            foreach (String s in queryList)
            {
                if(!s.Trim().Equals("") ){
                    if(!abbrList.Contains(s.Trim()))
                        return false;
                }
            }
            return true;
        }
        private Boolean isGrammarTheHighest(List<String> abbrList,String selectAbbr)
        {
            if (!abbrList.Contains(selectAbbr))
                return false;
            int max = -1;
            foreach (String abbr in abbrList)
            {
                int seq = findSeq(abbr);
                max = max < seq ? seq : max;
            }
            if (findSeq(selectAbbr) >= max)
                return true;
            return false;
        }
        private int findSeq(String abbr)
        {
            foreach (Grammar g in grammars)
            {
                if (g.Abbreviation.Equals(abbr) )
                {
                    return g.Seq;
                }
            }
            return -1;
        }
        public void printSentenceWithGrammar(String outPath)
        {
            StreamWriter writer = new StreamWriter(outPath, false ,Encoding.GetEncoding("gbk"));
            foreach (Paragraph p in paragraphs)
            {
                foreach (Sentence s in p.Sentences)
                {
                    if (s.Abbreviations.Count > 0)
                    {
                        String str = "(";
                        foreach (String abbr in s.Abbreviations)
                        {
                            str += abbr+",";
                        }
                        str = str.Substring(0, str.Length - 1);
                        str += ")";
                        writer.Write(s.Text + str);
                    }
                    else
                        writer.Write(s.Text);
                    //writer.Write("\r\n");
                }
                writer.Write("\r\n\r\n\r\n");
            }
            writer.Flush();
            writer.Close();
        }
        public void sortByGrammarSeq(String outPath)
        {
            List<Paragraph> tmp = new List<Paragraph>(paragraphs);
            tmp.Sort();
            int i = 1;
            StreamWriter writer = new StreamWriter(outPath, false, Encoding.GetEncoding("gbk"));
            foreach (Paragraph p in tmp)
            {
                writer.Write("第"+i.ToString() + "篇：\r\n");
                writer.Write(p.ToStringWithAbbrAfterSentence() +"\r\n\r\n");
                i++;
            }
            writer.Flush();
            writer.Close();
        }
        public void sortByGrammarSeqSentence(String outPath)
        {
            List<Sentence> slist = new List<Sentence>();
            foreach (Paragraph p in paragraphs)
            {
                slist.AddRange(p.Sentences);
            }
            slist.Sort();
            int i = 1;
            StreamWriter writer = new StreamWriter(outPath, false, Encoding.GetEncoding("gbk"));
            foreach (Sentence s in slist)
            {
                writer.Write(i.ToString() + ": " + s.Text + s.abbrsToString() + " (" + s.Title + ")" + "\r\n\r\n");
                i++;
            }
            writer.Flush();
            writer.Close();
        }
        public void printParagraphHasGrammar(String outPath)
        {
            //FileStream fs = new FileStream(outPath, FileMode.Create);
            StreamWriter writer = new StreamWriter(outPath, false, Encoding.GetEncoding("gbk"));
            foreach (Grammar grammar in noReapeatGrammars)
            {
                writer.Write("含有语法：名称：[" + grammar.Name+","+grammar.Abbreviation+ "]  的文章有：\r\n");
                foreach (Paragraph paragraph in paragraphs)
                {
                    if (paragraph.Abbreviations.Contains(grammar.Abbreviation))
                    {
                        writer.Write(paragraph.ToStringWithAbbrAfterSentence() + "\r\n\r\n");
                    }
                }
            }
            writer.Flush();
            writer.Close();
        }
        public void printSentencesHasGrammar(String outPath,bool append)
        {
            StreamWriter writer = new StreamWriter(outPath, append, Encoding.GetEncoding("gbk"));
            foreach (Grammar grammar in noReapeatGrammars)
            {
                
                List<Sentence> tmp = new List<Sentence>();
                foreach (Paragraph paragraph in paragraphs)
                {
                    List<Sentence> list = paragraph.Sentences;
                    foreach (Sentence sentence in list)
                    {
                        if (sentence.Abbreviations.Contains(grammar.Abbreviation))
                        {
                            tmp.Add(sentence);
                           
                        }
                    }
                }
                if (tmp.Count > 0)
                {
                    writer.Write("含有语法：[" + grammar.Name + "," + grammar.Abbreviation + "]  的句子有：\r\n");
                    foreach (Sentence s in tmp)
                    {
                        writer.Write(s.ToStringWithAbbr().TrimStart() + "\r\n\r\n");
                    }
                }
            }
            writer.Flush();
            writer.Close();
        }
         
        public void findPatternInEachSentence(object paramObj)
        {
            int param = (int)paramObj;


            for (int i = param; i < paragraphs.Count; i += threadNum)
            {
                Paragraph paragraph = paragraphs[i];
                sentences = paragraph.Sentences;
                foreach (Sentence sentence in sentences)
                {
                    List<Grammar> gramlist = findPossibleGrammar(sentence);
                    foreach (Grammar g in gramlist) {
                        if (isSencenceContainsPattern(sentence, g))
                        {
                            g.frequency++;
                            sentence.addGrammar(g);
                            paragraph.addGrammar(g);
                        }
                    }
                    
                }
            }
/*
            foreach (Grammar grammar in grammars)
            {
                for (int i = param; i < paragraphs.Count; i += threadNum)
                {
                    Paragraph paragraph = paragraphs[i];
                    sentences = paragraph.Sentences;
                    foreach (Sentence sentence in sentences)
                    {                    
                        if (isSencenceContainsPattern(sentence, grammar))
                        {
                            grammar.frequency++;
                            sentence.addGrammar(grammar);
                            paragraph.addGrammar(grammar);
                        }
                    }
                }
            }
*/
            finishedThreadNum ++;
        }

        private List<Grammar> findPossibleGrammar(Sentence sentence)
        {
            List<Grammar> list = new List<Grammar>(); ;
            foreach (string word in sentence.Words) {
                list.AddRange(getGrammarIndex(word));
            }
            return list;
        }

        private List<Grammar> getGrammarIndex(String word)
        {
            List<Grammar> list;
            if (!gramIndex.ContainsKey(word)){
                list = new List<Grammar>();
            } else {
                return (List<Grammar>)gramIndex[word];
            }
            foreach (Grammar grammar in grammars)
            {
                if (grammar.Pattern[0] == "can") {
                    Console.Write("ca");
                }
                if (word.ToLower().Equals(grammar.Pattern[0])||        
                    wordmap.isInWordList(grammar.Pattern[0], word)){
                    list.Add(grammar);
                }
            }
            gramIndex[word] = list;
            return list;
        }
        
        private Boolean isSencenceContainsPattern(Sentence sentence, Grammar grammar)
        {
           
            for (int i = 0; i <= sentence.Words.Count - grammar.Pattern.Length; i++)
            {
                Boolean contains = true;
                
                for (int j = 0; j < grammar.Pattern.Length; j++)
                {
                    if (grammar.Pattern[j].Equals("*") && j < grammar.Pattern.Length - 1)
                    {
                        /*
                         * 遇到*之后
                         * 从这个位置开始可以跳n个词，
                         * 如果遇到下一个匹配，就递归
                         */
                        bool matched = false;
                        for (int k = j ; k  < sentence.Words.Count - i; k++)
                        {
                            String word = sentence.Words[i + k];
                            if (word.Equals(grammar.Pattern[j+1]))
                            {
                                Sentence subSent = sentence.subSentence(i + k + 1);
                                Grammar subGram = grammar.subGrammar(j + 2);
                                matched |= this.isSencenceContainsPattern(subSent, subGram);
                            }
                        }
                        if (matched)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                         
                    if (grammar.Pattern[j][0] < 'a')//语法中大写的只能匹配大写
                    {
                        if ( !sentence.Words[i + j].Equals(grammar.Pattern[j])&&
                            !wordmap.isInWordList(grammar.Pattern[j], sentence.Words[i + j])     
                            )
                        {
                            contains = false;
                            break;
                        }
                    }
                    else
                    {
                        if (!sentence.Words[i + j].ToLower().Equals(grammar.Pattern[j])&&
                            !wordmap.isInWordList(grammar.Pattern[j], sentence.Words[i + j]) 
                            )
                        {
                            contains = false;
                            break;
                        }
                    }
                }
                if (contains)
                    return true;
            }
            return false;
        }
        public List<Grammar> getGrammarList()
        {
            return noReapeatGrammars;
        }
    }
}
