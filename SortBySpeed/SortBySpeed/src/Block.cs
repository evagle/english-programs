using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SrtTimeModify.src
{
    class Block :IComparable
    {
        public ITime startTime=null;
        public ITime endTime=null;
        public double speed = 0;
        public int wordCount = 0;
        public double totalTime = 0;
        public static int cc =0;

        public List<string> lines = new List<string>();

        public Block mergeBlock(Block other) {
            this.endTime = other.endTime;
            this.lines.AddRange(other.getLines());
            return this;
        }
        public void addLine(string line)
        {
            lines.Add(line);
            initTime(line);
            wordCount += this.countWords(line);
            if (startTime != null && endTime != null)
            {
                totalTime = endTime.sub(startTime);
                if (totalTime > 0)
                    speed = wordCount / totalTime;
            }
            
        }
        public int countWords(string str)
        {
            Regex r1 = new Regex("[a-zA-Z]+");
            if (r1.IsMatch(str))
            {
                Regex r = new Regex("[^A-Za-z0-9 \r\n\t-'’,?:：]");
                String rawtext = r.Replace(str, "");
                r = new Regex("[ \t\r\n:：]+");
                rawtext = r.Replace(rawtext, " ");
                rawtext = rawtext.Replace(",", " , ");
                rawtext = rawtext.Replace("?", " ? ");
                string[] words = rawtext.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return words.Length;
            }
            return 0;
           

           
        }

        public List<string> getLines()
        {
            int start = -1;
            int end = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                if (isTime(lines[i]))
                {
                    if(start==-1)
                        start = i;
                    end = i;
                }
            }
            lines[start] = startTime.strTime + lines[start].Substring(12);
            lines[end] = lines[end].Substring(0,17)+ endTime.strTime;

            return lines;
        }
        private bool isTime(string line)
        {
            Match match = Regex.Match(line, @"[0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{1,3}");
            return match.Success;
        }
        public void initTime(string line)
        {
            MatchCollection matches = Regex.Matches(line, @"[0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{1,3}");
            int i = 0;
            foreach (Match match in matches)
            {
                foreach (Capture capture in match.Captures)
                {
                    if (i == 0 && startTime == null)
                    {
                        startTime = new ITime(capture.Value);
                    }
                    if (i == 1)
                    {
                       endTime = new ITime(capture.Value);
                    }
                    i++;
                }
            }
            
        }
        
        public void amendStartTime(int t){
            startTime.amend(t);
        }
        public void amendEndTime(int t){
            endTime.amend(t);
        }
        public void genTimeString()
        {
            this.lines.Add(startTime.strTime + " --> " + endTime.strTime);
        }
        public string genStartEndTime()
        {
            string str = startTime.strTime + "\r\n" + endTime.strTime + "\r\n\r\n";
            str =  str.Replace(',', '.');
            return str;
        }



        #region IComparable<Block> 成员

        int IComparable.CompareTo(Object other)
        {
             
            if (this.speed > ((Block)other).speed)
                return 1;
            else
                return -1;
           
        }

        #endregion
    }
}
