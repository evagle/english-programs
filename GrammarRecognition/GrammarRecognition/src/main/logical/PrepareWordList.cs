using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrammarRecognition.src.main.model;
using System.IO;

namespace GrammarRecognition.src.main.logical
{
    class PrepareWordList
    {
        public PrepareWordList( )
        {

        }
        public WordMap getWordList(String path)
        {
            WordMap map = new WordMap();
            listFiles(path, map);
            return map;
        }
        public void listFiles(String dirPath,WordMap map)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            FileInfo[] files = dirInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                handleFile(files[i].FullName,files[i].Name,map);
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                listFiles(dirs[i].FullName,map);
            }
        }
        private void handleFile(String filepath, String name, WordMap map)
        {
            try
            {
                HashSet<String> set = new HashSet<String>();
                StreamReader reader = new StreamReader(filepath, Encoding.Default);
                string line = "";
                line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Trim().Equals(""))
                        set.Add(line.Trim().ToLower());
                    line = reader.ReadLine();
                }
                reader.Close();
                map.addWordList(name.Replace(".txt", ""), set);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            } 
        }
    }
}
