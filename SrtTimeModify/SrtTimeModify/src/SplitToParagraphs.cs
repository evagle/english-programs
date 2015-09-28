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
            //按照句号，感叹号拆开, 问号不拆开，因为会有回答
            for (int i = 0; i < tmp.Count; i++)
            {
                
                if (isTime(tmp[i]))
                {
                    if (i - 1 >= 0 && isSentEnd(tmp[i - 1]) )
                    {
                        this.result.Add("");
                        this.result.Add(tmp[i]);
                    }
                    else if (i - 2 >= 0 && !isTime(tmp[i - 1]) && isSentEnd(tmp[i - 2]))
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

            // 将所有的括号及括号里的内容删除
            string str = "";
            bool flag = true;
            for (int i = 0; i < line.Length; i++) {
                if (line[i] == '(' || line[i] == '（' || line[i] == '<') {
                    flag = false;
                }
                else if (line[i] == ')' || line[i] == '）' || line[i] == '>')
                {
                    flag = true;
                }
                else {
                    if (flag) {
                        str += line.Substring(i, 1);
                    }
                }
            }


            str = str.Replace('·', '.');
            str = str.Replace('。', '.');
            /*str = str.Replace('，', ',');
            str = str.Replace('。', '.');
            str = str.Replace('！', '!');
            str = str.Replace('〉','>');
            str = str.Replace('《','<');
            str = str.Replace('》','>');
            str = str.Replace('？','?');
            
            str = str.Replace('‘','\'');
            str = str.Replace('’','\'');
            str = str.Replace('“','"');
            str = str.Replace('”','"');
            str = str.Replace('（','(');
            str = str.Replace('）',')');*/

            int dot = -1; // 句中是否含有句号感叹号
            bool endWithOther = false; // 以其他标点结尾，例如逗号，问号，省略号

            for (int i = str.Length - 1; i >= 0; i--) {
                if (str[i] == '!' || str[i] == '！') {
                    dot = i;
                    break;
                }
                if (str[i] == '.' && (i - 1 < 0 || str[i - 1] != '.') && ( i + 1 >= str.Length || str[i + 1] != '.')) {
                    
                    dot = i;
                    break;        
                }
            }
            // 找到了句号或者感叹号，继续判定结尾符号
            if (dot >= 0) {
                for (int i = dot + 1; i < str.Length; i++) {
                    if (str[i] == ',' || str[i] == '?' || str[i] == '，' || str[i] == '？') {
                        return false;
                    }
                    if ((i + 2 < str.Length && str.Substring(i, 3) == "...")  
                        || (i + 1 < str.Length && str.Substring(i, 2) == "。。")) {
                        return false;
                    } 
                }
                return true;
            }   
            else { // 没有找到句号和感叹号，说明这句不是句子结尾
                return false;
            }

                // 对于加了语法的句子，需要先忽略语法，否则会导致结尾判断不正确
                /*line = line.TrimEnd();
                if (line.EndsWith(")")) {
                    int i = line.Length - 1;
                    for (; i >= 0; i--) {
                        if(line[i] == '(') {
                            break;
                        }
                    }
                    line = line.Substring(0,i);
                }
            

                if (line.EndsWith("..") || line.EndsWith("。。"))
                {
                    return false;
                }
                else if (line.EndsWith(".") || line.EndsWith("!") ||
                   line.EndsWith("！") || line.EndsWith("。"))
                {
                    return true;
                }*/
                return false;
        }
    }
}
