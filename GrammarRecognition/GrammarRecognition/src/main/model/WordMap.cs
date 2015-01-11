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
        public Hashtable reservedDictName = new Hashtable();

        public WordMap()
        {    
        }

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
            
            HashSet<String> wordList = null;
            //只有动词和名词需要扩展
            
            //if (reservedDictName.ContainsKey(listName))
            //{
                wordList = (HashSet<String>)table[listName];
            //}

            if (wordList == null)
                return false;
            //ing 形式的转换为普通的形式
            int index = word.IndexOf("'");
            if (index >= 0)
            {
                return wordList.Contains(word) || wordList.Contains(word.Substring(index)) ;
            }
            int wlen = word.Length;

            if (listName == "n" || listName == "v")
            {
                if (word.EndsWith("ies"))
                {
                    return wordList.Contains(word.Substring(0, wlen - 3) + "y") || wordList.Contains(word);
                }
                else if (word.EndsWith("es")) 
                {
                    return wordList.Contains(word.Substring(0, word.Length - 2)) ||
                        wordList.Contains(word.Substring(0, word.Length - 1)) || 
                        wordList.Contains(word);
                }
                else if (word.EndsWith("s"))
                {
                    return wordList.Contains(word.Substring(0, word.Length - 1)) || 
                        wordList.Contains(word);
                }
            }
            if (listName[listName.Length - 1] - '0' >= 0 && listName[listName.Length - 1] - '0' <= 9)
                return wordList.Contains(word);
            else
                return wordList.Contains(word) || wordList.Contains(word.ToLower()); 
        }
    }
}
