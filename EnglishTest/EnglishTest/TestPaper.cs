using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace EnglishTest
{
    class TestPaper
    {
        private string article;
        private List<SingleWordModel> words;
        public List<int> questionIndexes;
        public List<string> QuestionList;
        public Hashtable examWordList;
        public List<string> associatedList;
        public Hashtable associateHashtable;
        public HashSet<string> PunctuationTable;
        public TestPaper(string article)
        {
            this.article = article;
            words = new List<SingleWordModel>();
            questionIndexes = new List<int>();
            QuestionList = new List<string>();
            examWordList = new Hashtable();
            associatedList = new List<string>();
            PunctuationTable = new HashSet<string>();
            PunctuationTable.Add(".");
            PunctuationTable.Add("!");
            PunctuationTable.Add(":");
            PunctuationTable.Add(",");
            PunctuationTable.Add("~");
          



            SplittoWords();
            if (GlobalData.examType == GlobalData.Exam_Type.JUMP_WORD)
                SelectWordsByJump();
            else
            {
                SelectWordsByWordList();
            }

        }
        public string getPaper()
        {
            int count = 0;
            StringBuilder paper = new StringBuilder();
            QuestionList = new List<string>();
            questionIndexes = new List<int>();
            for (int i = 0; i < words.Count; i++)
            {
                if (words[i].Type != SingleWordModel.WordType.SELECTED)
                {
                    paper.Append(words[i].Word);
                    if (!words[i].Word.Equals("\n"))
                    {
                        paper.Append(" ");
                    }
    
                }
                else
                {
                    QuestionList.Add(words[i].Word);
                    questionIndexes.Add(paper.Length+3+((int)Math.Log10(count+2)));
                    paper.Append(" "+(count+1) + ".        ");
                    count++;
                }
            }
            return paper.ToString();
        }
        public void selectedPhrase(string[] phrase)
        {
            int i=0;
            int j = 0;
            List<int> pos = new List<int>();
            while ( i < words.Count)
            {
                if (words[i].Type == SingleWordModel.WordType.PUNCTUATION &&(words[i].Word.Trim().Equals(".") || 
                    words[i].Word.Trim().Equals("!") ||words[i].Word.Trim().Equals("！") ||
                    words[i].Word.Trim().Equals("?") ||words[i].Word.Trim().Equals("？")||
                    words[i].Word.Trim().Equals(":")||words[i].Word.Trim().Equals("：")))
                {
                    j = 0;
                    i++;
                    pos.Clear();
                }
                else if (words[i].Word.Equals(phrase[j]))
                {
                    pos.Add(i);
                    i++;
                    j++;                   
                }
                else
                {
                    i++;
                }
                if (j == phrase.Length)
                {
                    for (int k = 0; k < pos.Count; k++)
                    {
                        if (words[pos[k]].Type!=SingleWordModel.WordType.PUNCTUATION)
                            words[pos[k]].Type = SingleWordModel.WordType.SELECTED;
                    }
                    pos.Clear();
                    j = 0;
                }
            }        
        }
        public void SelectWordsByWordList(){
          
            string path = GlobalData.selectedWordList.Path;
            
            loadAssociatedWordList();
            initHashtable();
            StreamReader reader = new StreamReader(path);
            String str;
            while ((str = reader.ReadLine()) != null)
            {
                str = str.Trim().ToLower();
                string[] phrase = str.Split(new char[] { ' ' });
                if (phrase.Length > 1)
                {
                    selectedPhrase(phrase);
                }
                else
                {
                    if (PunctuationTable.Contains(str.Trim()))
                        continue;
                    if (associateHashtable.Contains(str))
                    {
                        int lineNum = (int)associateHashtable[str];
                        string[] ws = associatedList[lineNum].Split(new char[] { ' ', '\t' });
                        for (int j = 0; j < ws.Length; j++)
                            if (!examWordList.ContainsKey(ws[j]) && !PunctuationTable.Contains(ws[j].Trim()))
                                examWordList.Add(ws[j], 1);
                    }
                    else
                    {
                        if (!examWordList.ContainsKey(str) && !PunctuationTable.Contains(str.Trim()))
                            examWordList.Add(str, 1);
                    }
                }
            }
            reader.Close();
             
            for (int i = 0; i < words.Count; i++)
            {
                if (examWordList.ContainsKey(words[i].Word.ToLower()))
                {
                    words[i].Type = SingleWordModel.WordType.SELECTED;
                }
            }
            



        }
 
        public string[] findAssociateWords(string w)
        {
            string[] res = null;
            for (int i = 0; i < associatedList.Count; i++)
            {
                if (associatedList[i].Contains(w)) {
                    res = associatedList[i].Split(new char[] { ' ', '\t' });
                    for (int j = 0; j < res.Length; j++)
                        if (res[j].Trim().Equals(w))
                            return res;
                         
                }
            }
            return null;
        }
        public void initHashtable()
        {
            associateHashtable = new Hashtable();
            for (int i = 0; i < associatedList.Count; i++)
            {
                string[] ws = associatedList[i].Split(new char[] { ' ', '\t' });
                for (int j = 0; j < ws.Length; j++)
                    if (!associateHashtable.ContainsKey(ws[j]))
                        associateHashtable.Add(ws[j], i);
            }
        }
      
        public void loadAssociatedWordList()
        {

            string path = GlobalData.ASSOCIATE_LIST_PATH;
            StreamReader reader = new StreamReader(path);
            String str;
            while ((str = reader.ReadLine()) != null)
            {
                str = str.Trim().ToLower();
                if(!PunctuationTable.Contains(str))
                    associatedList.Add(str);
            }
            reader.Close();
        }
        public void SelectWordsByJump()
        {
            int jumped = 0;
            int selected = 0;
            for (int i = 0; i < words.Count; i++)
            {
                if (jumped == GlobalData.jumpNum)
                {
                    if (words[i].Type != SingleWordModel.WordType.PUNCTUATION)
                    {
                        words[i].Type = SingleWordModel.WordType.SELECTED;
                        
                        selected++;
                    }
                    if (selected == GlobalData.selectNum)
                    {
                        jumped = 0;
                        selected = 0;
                    }
                    continue;
                }

                if (words[i].Type != SingleWordModel.WordType.PUNCTUATION)
                {
                    jumped++;
                }

            }
        }
        private void SplittoWords(){
            string[] tmp = article.Split(new char[] { ' ' });       
            //[a-zA-Z-'0-9]
            for (int k = 0; k < tmp.Length; k++)
            {
                string w = tmp[k].Trim();
                int start = 0;
                SingleWordModel model;
                for (int i = 0; i < w.Length;)
                {
                    if ((w[i] >= 'a' && w[i] <= 'z') || (w[i] >= 'A' && w[i] <= 'Z') ||
                        w[i] == '-' || w[i] == '\'' || w[i] == '’' || (w[i] >= '0' && w[i]<='9'))
                        i++;
                    else
                    {
                        if (i > start)
                        {
                            model = new SingleWordModel();
                            model.Word = w.Substring(start, i - start);
                            model.Type = SingleWordModel.WordType.NONSELECTED;
                            words.Add(model);
                            
                        }
                        model = new SingleWordModel();
                        model.Word = w.Substring(i, 1);
                        model.Type = SingleWordModel.WordType.PUNCTUATION;
                        words.Add(model);
                        start = ++i ;
                    }
                   
                }
                if (start < w.Length)
                {
                    model = new SingleWordModel();
                    model.Word = w.Substring(start, w.Length - start);
                    model.Type = SingleWordModel.WordType.NONSELECTED;
                    words.Add(model);

                }
            }
        }

        
        
    }
}
