﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrammarRecognition.src.main.model;
using System.IO;

namespace GrammarRecognition.src.main.logical
{
    class Controler
    {
        private List<Paragraph> paragraphs;
        private List<Sentence> sentences;
        private WordMap wordmap;
        private List<Grammar> grammars;
        private List<Grammar> noReapeatGrammars;
        public Controler(String POSPath, String grammarPath,
            String paragraphPath,String outPath)
        {
            PrepareWordList prepareWordList = new PrepareWordList();
            wordmap = prepareWordList.getWordList(POSPath);

            paragraphs = new List<Paragraph>();
            sentences = new List<Sentence>();
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
            

            findPatternInEachSentence();

            printParagraphHasGrammar(outPath+"\\出现过某语法的文章.txt");
            printSentencesHasGrammar(outPath + "\\出现过某语法的句子.txt", false);
            sortByGrammarSeq(outPath + "\\文章按照语法序号降序排序.txt");
            sortByGrammarSeqSentence(outPath + "\\句子按照语法序号降序排序.txt");
            printSentenceWithGrammar(outPath + "\\句子后面加上了它含有的语法.txt");
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
                    writer.Write("\r\n");
                }
                writer.Write("\r\n\r\n");
            }
            writer.Flush();
            writer.Close();
        }
        public void sortByGrammarSeq(String outPath)
        {
            paragraphs.Sort();
            int i = 1;
            StreamWriter writer = new StreamWriter(outPath, false, Encoding.GetEncoding("gbk"));
            foreach (Paragraph p in paragraphs)
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
        public void findPatternInEachSentence()
        {
            foreach (Grammar grammar in grammars)
            {
                foreach (Paragraph paragraph in paragraphs)
                {
                    sentences = paragraph.Sentences;
                    foreach (Sentence sentence in sentences)
                    {
                        
                        if (isSencenceContainsPattern(sentence, grammar))
                        {
                            sentence.addGrammar(grammar);
                            paragraph.addGrammar(grammar);
                        }
                    }
                }
            }
        }
        private Boolean isSencenceContainsPattern(Sentence sentence, Grammar grammar)
        {

            for (int i = 0; i <= sentence.Words.Length - grammar.Pattern.Length; i++)
            {
                Boolean contains = true;
                for (int j = 0; j < grammar.Pattern.Length; j++)
                {
                    if (grammar.Pattern[j].Equals("*"))
                        continue;
                    if (!wordmap.isInWordList(grammar.Pattern[j], sentence.Words[i + j]) && 
                        !sentence.Words[i + j].ToLower().Equals(grammar.Pattern[j].Trim().ToLower())&&
                        !sentence.Words[i + j].ToLower().EndsWith(grammar.Pattern[j].ToLower()))
                    {
                        contains = false;
                        break;
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