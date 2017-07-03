using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using PaperReorganization.src.main.logical;
using System.IO;

namespace PaperReorganization.src.main.model
{
    class NewWord
    {
        private static Hashtable table = new Hashtable (); 

        public NewWord()
        {    
        }

        public static void initNewWords(string newWordPath)
        {
            try
            {
                StreamReader objReader = new StreamReader(newWordPath, Encoding.GetEncoding("gbk"));
                string sLine = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine == null)
                        break;
                    if (sLine.Trim() == "") {
                        continue;
                    }
                    string[] tmp = sLine.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 2) {
                        CLog.debug("生词表格式错误（前面单词，后面词频，空格隔开）："+sLine);
                        continue;
                    }
                    int frequency = Convert.ToInt32(tmp[1]);
                    if (frequency <= 0) {
                        CLog.debug("生词表词频错误：" + sLine);
                        continue;
                    }
                    if (!table.ContainsKey(tmp[0]))
                    {
                        table.Add(tmp[0], frequency);
                    }
                    string associate = AssociateWords.getOriginWord(tmp[0]);
                    if (associate != null && !table.ContainsKey(associate)) {
                        table.Add(associate, frequency);
                    }
                }
            }
            catch (Exception e)
            {
                CLog.error("生词表初始化错误：：error:" + e.Message + "  trace:" + e.StackTrace);
                Console.WriteLine(e.StackTrace);
            }        
        }

        public static int find(String word)
        {
            if (table.ContainsKey(word)) {
                CLog.debug("生词直接匹配："+word); 
                return (int)table[word];
            }
            string originWord = AssociateWords.getOriginWord(word);
            if (originWord != null && table.ContainsKey(originWord))
            {
                CLog.debug("生词关联词匹配：" + word + "\t" + originWord); 
                return (int)table[originWord];
            }
            return 0;
        }
  
    }
}
