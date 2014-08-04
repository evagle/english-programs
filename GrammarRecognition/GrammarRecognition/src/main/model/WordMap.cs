using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GrammarRecognition.src.main.model
{
    class WordMap
    {
        private Hashtable table = new Hashtable ();
        
        public void addWordList(String name,HashSet<String> wordList){
            table.Add(name, wordList);
        }
        public bool hasWordList(String name)
        {
            return table.ContainsKey(name);
        }
        public HashSet<String> getWordList(String name)
        {
            return (HashSet<String>)table[name];
        }
        public Boolean isInWordList(String listName, String word)
        {
            //先处理过去时与进行时
            if (listName.Equals("ved") || listName.EndsWith("v-ed"))
            {
                listName = "v";
                if (!word.EndsWith("ed"))
                {
                    return false;
                }
            }
            else if (listName.Equals("ving") || listName.EndsWith("v-ing"))
            {
                listName = "v";
                if (!word.EndsWith("ing"))
                {
                    return false;
                }
            }
            HashSet<String> wordList = null;
            //只有动词和名词需要扩展
            if (listName.Equals("v") || listName.Equals("n"))
            {
                wordList = (HashSet<String>)table[listName];
            }
            if (wordList == null)
                return false;
            //ing 形式的转换为普通的形式
            int index = word.IndexOf("'");
            if (index >= 0)
            {
                return wordList.Contains(word.Substring(index));
            }
            int wlen = word.Length;
            if (word.EndsWith("ing"))
            {
                if (word.Length >= 5 && word[wlen - 5]== word[wlen - 4])
                {
                    word = word.Substring(0, wlen - 4);
                }else
                    word = word.Substring(0, wlen - 3);
                return wordList.Contains(word);
            }
            //ed的
            else if (word.EndsWith("ed"))
            {
                if (word.Length >= 4 && word[wlen - 4] == word[wlen - 3])
                {
                    word = word.Substring(0, wlen - 3);
                }
                else
                    word = word.Substring(0, wlen - 2);
                 return wordList.Contains(word);
            }
            //加s或者es，ies的
            else if (word.EndsWith("ies"))
            {
                return wordList.Contains(word.Substring(0, wlen - 3)) || wordList.Contains(word);
            }
            else if (word.EndsWith("es"))
            {
                return wordList.Contains(word.Substring(0, word.Length - 2)) || wordList.Contains(word);
            }
            else if (word.EndsWith("s"))
            {
                return wordList.Contains(word.Substring(0, word.Length - 1)) || wordList.Contains(word);
            }

            return wordList.Contains(word.ToLower()); 
        }
    }
}
