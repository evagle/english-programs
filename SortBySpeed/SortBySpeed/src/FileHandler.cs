using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SrtTimeModify.src
{
    class FileHandler
    {
        public static List<string> read(String path)
        {
            StreamReader r = new StreamReader(path, Encoding.GetEncoding("gbk"));
            String str;
            List<String> list = new List<String>();
            while ((str = r.ReadLine()) != null)
            {
                str = str.Trim();
                list.Add(str);
            }
            r.Close();
            return list;
        }
        public static void write(String path, List<string>  content, bool append)
        {
            StreamWriter writer = new StreamWriter(path, append, Encoding.GetEncoding("gbk"));
            foreach (string str in content)
            {
                writer.WriteLine(str);
            }
            writer.Close();
        }
        public static void writeStartTimeEndTime(String path, List<string> content)
        {
            if (content.Count == 0)
                return;
            StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("gbk"));
            for (int i = 0; i < content.Count-1;i++ )
            {
                writer.Write(content[i]);
            }
            writer.Write(content[content.Count - 1]);
            writer.Close();
        }

        public static List<string> listFiles(String dirPath, string suffix)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            FileInfo[] files = dirInfo.GetFiles();
             List<string> fileList = new List<String>();
            for (int i = 0; i < files.Length; i++)
            {
                if (suffix == "" || files[i].FullName.EndsWith(suffix))
                    fileList.Add(files[i].FullName);
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                fileList.AddRange(listFiles(dirs[i].FullName, suffix));
            }
            return fileList;
        }
    }
}
