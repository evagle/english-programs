using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace ArticleRecognize.src.main
{
    class Judge
    {
        public double THRESHOLD = 0.9;
        public List<String> removeSame(List<String> list1, List<String> list2)
        {
            List<String> duplicatedList = new List<String>();
            List<String> remained = new List<String>();
            foreach (String at1 in list1)
            {
                bool isDuplicated = false;
                foreach (String at2 in list2)
                {
                    if (isSimilar(at1, at2))
                    {
                        isDuplicated = true;
                        break;
                    }
                }
                if (isDuplicated)
                {
                    duplicatedList.Add(at1);
                }
                else
                {
                    remained.Add(at1);
                }
            }
            return remained;

        }
        private List<String> cleanSelf(List<String> list)
        {
            list.Sort();
            List<String> remained = new List<String>();
            List<bool> duplicate = new List<bool>();
            for (int i = 0; i < list.Count; i++) {
                duplicate.Insert(i,false);
            }
          
            for (int i = list.Count - 1; i >= 0; i--)
            {
                //  之前没有与i重复的段落
                if (!duplicate[i])
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        // 如果ｊ与ｉ重复，则保留ｉ，ｊ标记为已重复
                        if (isSimilar(list[i], list[j]))
                        {
                            duplicate[j] = true;
                        }
                    }
                    remained.Add(list[i]);
                }
            }
            return remained;
        }
        public List<String> cleanSelf(String file)
        {
            List<String> list  = getarticles(file );
            return cleanSelf(list);
        }
        public List<String> removeSame(String file1, String file2)
        {
            List<String>  list1 =  getarticles(file1) ;
            List<String> list2 = getarticles(file2);
            return removeSame(list1, list2);
        }
        private List<String> getarticles(String path)
        {
            List<String> list = new List<String>();
            try
            {
                StreamReader reader = new StreamReader(path, Encoding.Default);
                StringBuilder  article = new StringBuilder();
                String line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Trim().Equals(""))
                    {
                        list.Add(article.ToString());
                        article = new StringBuilder();
                    }
                    else
                        article.Append(line+"\r\n");
                    line = reader.ReadLine();
                }
                string last = article.ToString();
                if (last != "") {
                    list.Add(last);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return list;
        }
        private  Boolean isSimilar(String article1,String article2)
        {
            double cosVaule = getCos(generateVector(splitToWords(article1)
                , splitToWords(article2)));
            if (cosVaule > THRESHOLD)
                return true;
            else
                return false;
        }
        private double getCos(Hashtable table)
        {
            double denominator = 0;
            double v1 = 0;
            double v2 = 0;
            foreach (String key in table.Keys)
            {
                int[] rec = (int[])table[key];
                denominator += rec[0] * rec[1];
                v1 += rec[0] * rec[0];
                v2 += rec[1] * rec[1];
            }
            if (v1 * v2 > 0)
                return denominator / (Math.Sqrt(v1) * Math.Sqrt(v2));
            else
                return -1;
        }
        private Hashtable generateVector(List<String> list1, List<String> list2)
        {
            Hashtable table = new Hashtable();
            foreach (String str in list1)
            {
                if (table.ContainsKey(str))
                {
                    int[] rec = (int[])table[str];
                    rec[0] += 1;
                    rec[1] = 0;
                }
                else
                {
                    int[] rec = new int[2];
                    rec[0] = 1;
                    rec[1] = 0;
                    table[str] = rec;
                }
            }
            foreach (String str in list2)
            {
                if (table.ContainsKey(str))
                {
                    int[] rec = (int[])table[str];
                    
                    rec[1] += 1;
                }
                else
                {
                    int[] rec = new int[2];
                    rec[0] = 0;
                    rec[1] = 1;
                    table[str] = rec;
                }
            }
            return table;
        }
        private List<String> splitToWords(String article)
        {
            List<String> list = new List<String>();
            String[] words = Regex.Split(article, @"[^a-zA-Z0-9]");
            foreach (String s in words)
            {
                if(!s.Trim().Equals(""))
                    list.Add(s);
            }
            return list;
        }
    }
}
