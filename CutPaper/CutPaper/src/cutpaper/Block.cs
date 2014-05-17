using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace CutPaper.src.cutpaper
{
    class Block:ICloneable
    {
        public enum BLOCK_TYPE  { selection , fill_blank,unknown,keyline,
        listening,writing,closen,reading,selection_reading,translation,correction,answer,example
        , selection_listening, selection_cloze,seq_range,complete_sentence,word_spell};
        private List<String> lines;
        private int startLineNum;
        private int endLineNum;
        private BLOCK_TYPE type;
        private List<int> seqs;
        private List<String> answers;
        private String textFilledWithAnswer;
        private Hashtable selectionAnswerTable;
        private String questionStringFilledWithAnswer;
        public List<string> question;
        public List<string> article;
        public Hashtable options;
        public Hashtable answerOptions;
        public int guessMinSeq = 0;
        public int guessMaxSeq = 0;
       
        

        public Block(){
            lines = new List<string>();
            seqs = new List<int>();
            answers = new List<string>();
            selectionAnswerTable = new Hashtable();
            question = new List<string>();
            article = new List<string>();
            options = new Hashtable();

            answerOptions = new Hashtable();
        }
       
        public String getString()
        {
            StringBuilder builer = new StringBuilder();
            foreach (String s in lines)
            {
                builer.Append(s + "\r\n");
            }
            return builer.ToString();
        }
        
        public String questionLinesToString()
        {
            StringBuilder builder = new StringBuilder("");

            foreach (String line in question)
            {
                builder.Append(line + "\r\n");
            }
            return builder.ToString();
        }

        public String getQuestionString()
        {
            //这个才是题目描述，原来是下面的函数，现在改名
            StringBuilder builer = new StringBuilder();
            foreach (String s in lines)
            {
                if (!Regex.IsMatch(s.Trim(), @"^A\."))
                {
                    builer.Append(s + "\n");
                }
                else
                    return builer.ToString();
            }
            return builer.ToString();
        }
        public String getOptionString()
        {
            StringBuilder builer = new StringBuilder();
            bool start = false;
            foreach (String s in lines)
            {
                if (Regex.IsMatch(s.Trim(), @"^A\.") || start)
                {
                    builer.Append(s + "\n");
                    start = true;
                }
                //else
               //     return builer.ToString();
            }
            return builer.ToString();
        }
        public void AddSeqsRange(List<int> seqList)
        {
            foreach (int i in seqList)
            {
                if (!this.Seqs.Contains(i))
                    Seqs.Add(i);
            }
        }
        public void AddSeq(int seq)
        {
             if (!this.Seqs.Contains(seq))
                    Seqs.Add(seq);
            
        }
        public List<String> Answers
        {
            get { return answers; }
            set { answers = value; }
        }
        public List<int> Seqs
        {
            get { return seqs; }
            set { seqs = value; }
        }
         public List<String> Lines
        {
            get { return lines; }
            set { lines = value; }
        }
        public int StartLineNum
        {
            get { return startLineNum; }
            set { startLineNum = value; }
        }
        public int EndLineNum
        {
            get { return endLineNum; }
            set { endLineNum = value; }
        }
        public BLOCK_TYPE Type
        {
            get { return type; }
            set { type = value; }
        }
        public String TextFilledWithAnswer
        {
            get { return textFilledWithAnswer; }
            set { textFilledWithAnswer = value; }
        }
        public Hashtable SelectionAnswerTable
        {
            get { return selectionAnswerTable; }
            set { selectionAnswerTable = value; }
        }
        public String QuestionStringFilledWithAnswer
        {
            get { return questionStringFilledWithAnswer; }
            set { questionStringFilledWithAnswer = value; }
        }


        #region ICloneable 成员

        public object Clone()
        {
            return new Block() as object;
        }

        #endregion
    }
}
