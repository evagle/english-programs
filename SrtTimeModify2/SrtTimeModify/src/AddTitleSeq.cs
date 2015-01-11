using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SrtTimeModify.src
{
    class AddTitleSeq
    {
        public void listFiles(string path, List<String> statList)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles();
            
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.StartsWith("改好时间-"))
                {
                    statList.AddRange(addTitleSeq(files[i].Directory.FullName, files[i].Name));
                }
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                listFiles(dirs[i].FullName, statList);
            }
        }

        public List<String> addTitleSeq(string path, string oname)
        {
            string name = oname.Replace("改好时间-", "");
            name = name.Replace(".srt-", "").Replace(".txt-", "").Replace(".txt", "").Replace(".srt", "");
            name = name.Replace(".eng&chs", "");
            name = name.Replace(".chs&eng", "");
            name = name.Replace(".eng&ch", "");
            name = name.Replace(".eng", "").Replace(".chs", "");
            


            bool start = true;
            int i = 1;
            List<String> lines = FileHandler.read(path + "\\" + oname);
         
            List<String> outLines = new List<String>();
            foreach (string line in lines)
            {
                if (line == "")
                {
                    start = true;
                    outLines.Add("\n");
                }
                else
                {
                    if (start)
                    {
                        outLines.Add(name +"."+ i+"");
                        i++;
                    }
                    outLines.Add(line);
                    start = false;
                }
            }
            path = path + "-加标题序号\\";
            Directory.CreateDirectory(path);
            FileHandler.write(path + "加标题序号-" + oname, outLines);

            StretchTime st = new StretchTime(lines);
            st.articleToParagraphBlocks();
            List<string> startEndTimeList = st.getStartTimeEndTime();
            FileHandler.writeStartTimeEndTime(path + "开始时间-结束时间-" + oname.Replace("改好时间-", ""), startEndTimeList);
         

            List<String> statList = new List<String>();
            statList.Add(oname);
            statList.Add((i-1)+"");
            statList.Add("无备注");
            statList.Add("\n");
            return statList;
        }
    }
}
