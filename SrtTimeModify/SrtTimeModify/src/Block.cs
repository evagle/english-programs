using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SrtTimeModify.src
{
    class Block
    {
        public ITime startTime=null;
        public ITime endTime=null;
        
        private List<string> lines = new List<string>();

        public Block mergeBlock(Block other) {
            this.endTime = other.endTime;
            this.lines.AddRange(other.getLines());
            return this;
        }
        public void addLine(string line)
        {
            lines.Add(line);
            initTime(line);
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
            Match match = Regex.Match(line, @"[0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3}");
            return match.Success;
        }
        public void initTime(string line)
        {
            MatchCollection matches = Regex.Matches(line, @"[0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3}");
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
        

    }
}
