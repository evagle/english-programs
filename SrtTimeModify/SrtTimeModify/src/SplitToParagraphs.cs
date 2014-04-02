using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SrtTimeModify.src
{
    class SplitToParagraphs
    {
        public List<string> result = new List<string>();
        
        public List<string> split(List<string> srt)
        {
            int start = 0;
            List<string> tmp = new List<string>();
            //将序号和空行合并
            for (int i = 0; i < srt.Count; i++)
            {
                if(isTime(srt[i])){
                    tmp.Add(srt[i]);
                }else if(!this.isSeq(srt[i])&&!srt[i].Trim().Equals("")){
                    tmp.Add(srt[i]);
                }
            }
            //按照句号，问号，感叹号拆开
            for (int i = 0; i < tmp.Count; i++)
            {
                if (isTime(tmp[i]))
                {
                    if (i - 1 >= 0 && isSentEnd(tmp[i - 1]))
                    {
                        this.result.Add("");
                        this.result.Add(tmp[i]);
                    }
                    else
                        this.result.Add(tmp[i]);
                }
                else
                {
                    this.result.Add(tmp[i]);
                }
            }
            return this.result;

        }
        private bool isSeq(String line)
        {
            Match match = Regex.Match(line, @"^[0-9]*$");
            return match.Success;
        }
        private bool isTime(string line)
        {
            Match match = Regex.Match(line, @"[0-9]{2}:[0-9]{2}:[0-9]{2},[0-9]{3}");
            return match.Success;
        }
        private bool isSentEnd(string line)
        {
            if (line.EndsWith(".") || line.EndsWith("?") || line.EndsWith("!") ||
                line.EndsWith("？" )|| line.EndsWith("！") || line.EndsWith("。"))
            {
                return true;
            }
            return false;
        }
    }
}
