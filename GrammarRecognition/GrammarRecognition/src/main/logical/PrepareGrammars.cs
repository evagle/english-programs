using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrammarRecognition.src.main.model;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace GrammarRecognition.src.main.logical
{
    class PrepareGrammars
    {
        private String path;
        private List<Grammar> grammars;
        private WordMap wordmap;
        private Hashtable needExpandList;
        public PrepareGrammars(String path, WordMap wordmap)
        {
            this.path = path;
            grammars = new List<Grammar>();
            this.wordmap = wordmap;
        }
        public  List<Grammar> getGrammars()
        {
            HashSet<string> set = new HashSet<string>();
            try
            {
                StreamReader reader = new StreamReader(path, Encoding.Default);
                string line = reader.ReadLine();
                while (line != null)
                {

                    Grammar grammar = new Grammar();
                    String[] parts = line.Split(new char[] { '#'  });
                    if (set.Contains(parts[2]))
                    {
                        Console.WriteLine(parts[2]);

                    }
                    else
                        set.Add(parts[2]);
                    if (parts.Length == 5)
                    {
                        grammar = new Grammar();
                        grammar.Type = Grammar.T_GRAMMAR;
                        
                        grammar.Text = line;
                        grammar.Name = parts[1];
                        grammar.Abbreviation = parts[2];
                        grammar.Pattern = removeSpace(parts[3].Split(new char[] { '+', ' ' }));

                        grammar.Seq = Int32.Parse(parts[4]);
                        grammars.Add(grammar);
                        List<String> ignoreList = new List<String>();
                        ignoreList.Add("n");
                        ignoreList.Add("v");
                        ignoreList.Add("ved");
                        ignoreList.Add("ving");
                        expandGrammar(grammar, ignoreList);
                    }
                    else if (parts.Length == 4)
                    {
                        grammar = new Grammar();
                        grammar.Type = Grammar.T_PHRASE;
                        
                        grammar.Text = line;
                        grammar.Name = parts[1];
                        grammar.Abbreviation = parts[1];
                        grammar.Pattern = removeSpace(parts[2].Split(new char[] { '+', ' ' }));
                         
                        grammar.Seq = Int32.Parse(parts[3]);
                        grammars.Add(grammar);
                    }
                    else {
                        MessageBox.Show("语法格式不对,请先修改：" + line);
                    }
                    line = reader.ReadLine();
                }
            }
            catch (System.Exception ex)
            {
            	Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine("abbr num:" + set.Count);
            return grammars;
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
        public void expandGrammar(Grammar grammar ,List<String> ignoreList)
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
                        tmpIgnoreList.Add(p);
                        foreach (String s in set)
                        {
                            String[] ws = s.Split(new char[] { ' ' });
                            if (ws.Length >= 1 && !ws[0].Equals(p))
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
                                grammars.Add(newGrammar);
                                ignoreList.Add(p);
                                expandGrammar(newGrammar, tmpIgnoreList);
                            }

                        }
                         
                    }
                }
            }
                
        }

    }
}
