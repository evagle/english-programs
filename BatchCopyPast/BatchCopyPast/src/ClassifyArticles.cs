using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace BatchCopyPast.src
{
    class ClassifyArticles
    {
        private Hashtable fouts = new Hashtable();
        public void classify(string filePath, string outDir){
            StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("gbk"));
            String str;
            List<String> list = new List<String>();
            while ((str = reader.ReadLine()) != null)
            {
                str = str.Trim();
                if (str == "")
                {
                    writeOut(list,outDir);
                    list = new List<String>();
                }
                else
                {
                    list.Add(str);
                }
            }
            reader.Close();
            close();

        }
        public void writeOut(List<String> content, String outDir)
        {
            if (content.Count <= 1)
                return;
            else
            {
                String title = content[0];
                StreamWriter writer = null;
                if (fouts.ContainsKey(title))
                {
                    writer = (StreamWriter)fouts[title];
                }
                else
                {
                    writer = new StreamWriter(outDir+"\\"+title, false, Encoding.GetEncoding("gbk"));
                    fouts[title] = writer;
                }
                for (int i = 1; i < content.Count; i++)
                {
                    writer.WriteLine(content[i]);
                }
            }
        }
        public void close()
        {
            foreach (String key in fouts.Keys)
            {
                StreamWriter writer = (StreamWriter)fouts[key];
                writer.Flush();
                writer.Close();
            }
        }
    }
}
