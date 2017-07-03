using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace PaperReorganization.src.main.logical
{
    class WordsTreeNode
    {
        public string origin = null;
        public WordsTreeNode[] childs; // a-z
    }

    class WordsTree
    {
        private static WordsTreeNode root;
       

        public static void addWord(string word, string origin)
        {
            word = word.ToLower();
            if (root == null) {
                root = new WordsTreeNode();
            }
            WordsTreeNode node = root;
            for (int i = 0; i < word.Length; i++) {
                int p = word[i] - 'a';
                if (p >= 26 || p < 0) {
                    continue;
                }
                if (node.childs == null)
                {
                    node.childs = new WordsTreeNode[26];
                }
              
                if (node.childs[p] == null)
                {
                    node.childs[p] = new WordsTreeNode();
                }
                node = node.childs[p];
            }
            node.origin = origin;
        }

        public static string getOriginWord(string word) 
        {
            WordsTreeNode node = root;
            word = word.ToLower();
            for (int i = 0; i < word.Length; i++)
            {
                int p = word[i] - 'a';
                if (p < 0 || p >= 26) {
                    continue;
                }
                node = node.childs[p];
                if (node == null || node.childs == null)
                {
                    return null;
                }
            }
            return node.origin;
        }
    }

    class AssociateWords
    {
        private static Hashtable associates = new Hashtable();
        private static WordsTree wordTree;
        public static void prepare(string path, int showError) 
        {
            try
            {
                if (!File.Exists(path)) {
                    return;
                }
                StreamReader objReader = new StreamReader(path, Encoding.GetEncoding("gbk"));
                string sLine = "";
                string paragraphText = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine == null)
                        break;
                    string[] words = sLine.Split(new char[] { ' ','\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length > 0) {
                        HashSet<String> set = new HashSet<string>();
                        foreach (String w in words) {
                            set.Add(w);
                            WordsTree.addWord(w, words[0]);
                        }
                        associates[words[0]] = set;
                    }
                }
              
                objReader.Close();
            }
            catch (Exception e)
            {
                CLog.error("初始化关联词表出错：error:" + e.Message + "  trace:" + e.StackTrace);
                if (showError > 0)
                    MessageBox.Show("初始化关联词表出错:" + path + ", error:" + e.Message);
            }
        }

        public static bool isAssociateWord(String word, String other)
        {
            return associates.ContainsKey(word) && ((HashSet<String>)associates[word]).Contains(other);
        }

        public static string getOriginWord(string word)
        {
            return WordsTree.getOriginWord(word);
        }

    }
}
