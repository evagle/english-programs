using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PaperReorganization.src.main.logical
{
    class CLog
    { 
        private static StreamWriter writer = new StreamWriter("log.txt", false, Encoding.GetEncoding("gbk"));

        private static void write(string data, string type)
        {
            data = DateTime.Now.ToString("yyyyMMdd HH:mm:ss\t["+type+"]\t") + data + "\r\n";
            writer.Write(data);
            writer.Flush();
       
        }

        public static void debug(string data)
        {
            write(data, "DEBUG");
        }

        public static void error(string data)
        {
             write(data, "ERROR");
        }

        public static void close()
        {
            writer.Close();
        }
    }
}
