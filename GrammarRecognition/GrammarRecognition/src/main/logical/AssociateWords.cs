using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace GrammarRecognition.src.main.logical
{
    class AssociateWords
    {
        private Hashtable associates = new Hashtable();
        
        public void prepare(string path, int showError) 
        {
            try
            {
                if (!File.Exists(path)) {
                    return;
                }
                StreamReader objReader = new StreamReader(path, Encoding.Default);
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
                        }
                        associates[words[0]] = set;
                    }
                }
              
                objReader.Close();
            }
            catch (Exception e)
            {
                if (showError > 0)
                    MessageBox.Show("读取文件发生错误:" + path+", error:"+e.Message);
            }
        }
        public bool isAssociateWord(String word, String other)
        {
            return associates.ContainsKey(word) && ((HashSet<String>)associates[word]).Contains(other);
            /*bool flag = false;
            if (associates.ContainsKey(word))
                flag |= ((HashSet<String>)associates[word]).Contains(other);
            if (associates.ContainsKey(other))
                flag |= ((HashSet<String>)associates[other]).Contains(word);
            return flag;*/

        }
    }
}
