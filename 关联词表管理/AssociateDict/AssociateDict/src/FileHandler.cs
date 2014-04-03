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
        public static void write(String path, List<string>  content)
        {
            StreamWriter writer = new StreamWriter(path , false, Encoding.GetEncoding("gbk"));
            foreach (string str in content)
            {
                writer.WriteLine(str);
            }
            writer.Close();
        }
        
        
    }
}
