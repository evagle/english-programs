using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaperReorganization.src.main.model;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace PaperReorganization.src.main.logical
{
    class PrepareGrammars
    {
      
        private static List<Grammar> grammars;
        public static Hashtable grammarTable;
        private WordMap wordmap;
        private Hashtable needExpandList;

   
        public void initGrammars(string path, int showError)
        {
            if (grammarTable == null)
            {
                grammarTable = new Hashtable();
            }
            HashSet<string> set = new HashSet<string>();
            List<String> ignoreList = new List<String>();
           // foreach (string s in wordmap.reservedDictName.Keys)
           // {
           //     ignoreList.Add(s);
           // }
            StringBuilder duplicateGrammars = new StringBuilder();
            try
            {
                StreamReader reader = new StreamReader(path, Encoding.GetEncoding("gbk"));
                string line = reader.ReadLine();
                
                while (line != null)
                {

                    Grammar grammar = new Grammar();
                    String[] parts = line.Split(new char[] { '#'  });
                    if (set.Contains(parts[2]))
                    {
                        Console.WriteLine(parts[2]);
                        duplicateGrammars.Append(line + "\n");
                    }
                    else
                        set.Add(parts[2]);
                    if (parts.Length == 5 && parts[3].Trim() != "")
                    {
                        grammar = new Grammar();
                        grammar.Type = Grammar.T_GRAMMAR;
                        
                        grammar.Text = line;
                        grammar.Name = parts[1];
                        grammar.Abbreviation = parts[2];
                        grammar.Pattern = removeSpace(parts[3].Split(new char[] { '+', ' ' }));
                        if (grammar.Pattern.Length > 0)
                        {
                            grammar.PatternLowerCase = this.toLower(grammar.Pattern);
                            grammar.Seq = Int32.Parse(parts[4]);
                            //grammars.Add(grammar);
                            grammarTable[grammar.Abbreviation] = grammar;

                            //expandGrammar(grammar, ignoreList);
                        }
                    }
                    else if (parts.Length == 4 && parts[2].Trim() != "")
                    {
                        grammar = new Grammar();
                        grammar.Type = Grammar.T_PHRASE;
                        
                        grammar.Text = line;
                        grammar.Name = parts[1];
                        grammar.Abbreviation = parts[1];
                        grammar.Pattern = removeSpace(parts[2].Split(new char[] { '+', ' ' }));
                        grammar.PatternLowerCase = this.toLower(grammar.Pattern);

                        grammar.Seq = Int32.Parse(parts[3]);
                        //grammars.Add(grammar);
                        grammarTable[grammar.Abbreviation] = grammar;
                        //expandGrammar(grammar, ignoreList);

                    }
                    else {
                        if(showError > 0)
                            MessageBox.Show("语法格式不对,请先修改：" + line);
                    }
                    line = reader.ReadLine();
                }
            }
            catch (System.Exception ex)
            {
                CLog.error("解析语法出错：error:" + ex.Message + "  trace:" + ex.StackTrace);
            	Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("abbr num:" + set.Count);
            if (duplicateGrammars.ToString() != "")
            {
                if (showError > 0)
                    MessageBox.Show("以下语法有重复，请及时修改" + duplicateGrammars.ToString());
            }
            //return grammars;
        }
        private String[] removeSpace(String[] strArr)
        {
            List<String> tmp = new List<String>();
            foreach (String s in strArr)
            {
                if (!s.Trim().Equals(""))
                    tmp.Add(s.Trim());
            }
            return tmp.ToArray();
        }

        private String[] toLower(String[] strArr)
        {
            List<String> tmp = new List<String>();
            foreach (String s in strArr)
            {
              
                tmp.Add(s.ToLower());
            }
            return tmp.ToArray();
        }

        public void expandGrammar(Grammar grammar, List<String> ignoreList)
        {
            
            bool isExpanded = false;
            List<String> tmpIgnoreList = new List<String>(ignoreList);
            for (int i = 0; i < grammar.Pattern.Length;i++ )
            {
                String p = grammar.Pattern[i];
                if (isExpanded)
                {
                    break;
                }
                else
                {
                    HashSet<String> set = wordmap.getWordList(p);
                    if (set != null && !tmpIgnoreList.Contains(p))
                    {
                        //判断后面是否还有相同的缩写，没有就加的ignore里，有就不行
                        bool haveSamePatternBeind = false;
                        for (int m = i + 1; m < grammar.Pattern.Length; m ++ )
                        {
                            if (grammar.Pattern[m] == p)
                            {
                                haveSamePatternBeind = true;
                                break;
                            }
                        }
                        if (!haveSamePatternBeind)
                            tmpIgnoreList.Add(p);

                        foreach (String s in set)
                        {
                            String[] ws = s.Split(new char[] { ' ' });
                            if (ws.Length > 1 && !ws[0].Equals(p))
                            {
                                isExpanded = true;
                                Grammar newGrammar = new Grammar(grammar);
                                List<String> tmp = new List<String>();
                                for (int k = 0; k < i; k++)
                                {
                                    tmp.Add(grammar.Pattern[k]);
                                }
                                tmp.AddRange(ws);
                                for (int k = i + 1; k < grammar.Pattern.Length; k++)
                                {
                                    tmp.Add(grammar.Pattern[k]);
                                }
                                newGrammar.Pattern = tmp.ToArray();
                                newGrammar.PatternLowerCase = this.toLower(newGrammar.Pattern);
                                grammars.Add(newGrammar);
                                
                                tmpIgnoreList.AddRange(ws);
                                expandGrammar(newGrammar, tmpIgnoreList);
                            }

                        }
                         
                    }
                }
            }
                
        }

    }
}
