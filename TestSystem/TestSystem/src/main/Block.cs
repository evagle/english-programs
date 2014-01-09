using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CutPaper.src.cutpaper
{
    class Block
    {
        public enum BLOCK_TYPE  { selection , fill_blank,unknown,keyline,
        listening,writing,closen,reading,selection_reading,translation,correction,answer,example};
        private List<String> lines;
        private int startLineNum;
        private int endLineNum;
        private BLOCK_TYPE type;
        private List<int> seqs;
        private List<String> answers;

      
        

        public Block(){
            lines = new List<string>();
            seqs = new List<int>();
            answers = new List<string>();
        }
        public String getString()
        {
            StringBuilder builer = new StringBuilder();
            foreach (String s in lines)
            {
                builer.Append(s + "\n");
            }
            return builer.ToString();
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
        
    }
}
