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
        public double aveSpeed = 0;
        public int wordCount = 0;
        public double totalTime = 0;
        public double maxSpeed = 0;
        public static int cc =0;
        public double tmpt = 0;
        public int tmpw = 0;

        public List<string> lines = new List<string>();
        public List<string> linesWithSpeed = new List<string>();

        public Block mergeBlock(Block other) {
            this.endTime = other.endTime;
            this.lines.AddRange(other.getLines());
            return this;
        }

        public void addLine(string line, bool isTitle)
        {
            lines.Add(line);
            if (!doNotCount(line) && !isTitle)
            {
                double t = initTime(line);
                tmpt += t;
                int countThisLine= this.countWords(line) ;
                wordCount += countThisLine;
                tmpw += countThisLine;
                if (startTime != null && endTime != null)
                {
                    totalTime += t;
                   
                    if (totalTime > 0)
                        aveSpeed = wordCount / totalTime;
                }
                if (t > 0 && tmpt > t) { //这一行是时间，所以上一个句子已经完结
                    tmpt -= t;
                    double tmpSpeed = tmpw / tmpt;
                    if (tmpSpeed > maxSpeed)
                    {
                        maxSpeed = tmpSpeed;
                    }
                   
                    if (tmpSpeed > 0)
                    {
                        linesWithSpeed.Add("******本行语速****** " + String.Format("{0:0.##}", (tmpSpeed * 1000)) + " 词每秒" + " time:" + tmpt + "  words:" + tmpw);
             
                    }
                    tmpw = 0;
                    tmpt = t;

                }
            }
            if (line == "" && tmpt > 0) {
                double tmpSpeed = tmpw / tmpt;
                if (tmpSpeed > maxSpeed)
                {
                    maxSpeed = tmpSpeed;
                }
               
                if (tmpSpeed > 0)
                {
                    linesWithSpeed.Add("******本行语速****** "+String.Format("{0:0.##}", (tmpSpeed * 1000)) + " 词每秒" + " time:"+tmpt +"  words:"+tmpw);
                }
                tmpw = 0;
                tmpt = 0;
            }
            linesWithSpeed.Add(line);
            
        }

        public void addBlockSpeed() { 
            linesWithSpeed.Insert(0, "##### 本段最快语速 ： "+String.Format("{0:0.##}", (maxSpeed * 1000)) + " 词每秒");         
        }
        
        public bool doNotCount(string str)
        {
            if (str.StartsWith("最低词频率") || str =="")
                return true;
            Regex r = new Regex("^[a-zA-Z]+,[0-9]+$");
            if (r.IsMatch(str))
                return true;
            if (isTitle(str))
                return true;
            return false;
        }

        public int countWords(string str)
        {
           
            Regex grammar = new Regex("\\([a-zA-Z,]+\\)");
            str = grammar.Replace(str,"");
            Regex r1 = new Regex("[a-zA-Z]+");
            if (r1.IsMatch(str))
            {
                Regex r = new Regex("[^A-Za-z0-9 \r\n\t-'’]");
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
        public bool isTitle(string str) 
        {
             
            return  str.IndexOf("eng.srt") >= 0 || str.IndexOf("chs.srt") >= 0 ||
                str.IndexOf("chs&eng.srt") >= 0 || str.IndexOf("eng&chs.srt") >= 0;
             
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
            Match match = Regex.Match(line, @"[0-9]{2}:[0-9]{2}:[0-9]{2}[,\.][0-9]{1,3}");
            return match.Success;
        }

        public double initTime(string line)
        {
            MatchCollection matches = Regex.Matches(line, @"[0-9]{2}:[0-9]{2}:[0-9]{2}[,\.][0-9]{1,3}");
            int i = 0;
            ITime s = null, e = null;
            foreach (Match match in matches)
            {
                foreach (Capture capture in match.Captures)
                {
                    if (i == 0 )
                    {
                        s =  new ITime(capture.Value);
                        if (startTime == null)
                            startTime = s;

                    }
                    if (i == 1)
                    {
                       endTime = new ITime(capture.Value);
                       e = endTime;
                    }
                    i++;
                }
            }
            if (s != null && e != null)
                return e.sub(s);
            else
                return 0;
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
             
            if (this.maxSpeed > ((Block)other).maxSpeed)
                return 1;
            else
                return -1;
           
        }

        #endregion
    }
}
