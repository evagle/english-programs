using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrammarRecognition.src.main.model;
using System.IO;
using System.Collections;

namespace GrammarRecognition.src.main.logical
{
    class PrepareSentences
    {
        private List<Paragraph> paragraphs;
        private List<Sentence> sentences;
        private String paragraphDirPath;
        public PrepareSentences(String paragraphDirPath,List<Paragraph> paragraphs,List<Sentence> sentences)
        {
            this.paragraphs = paragraphs;
            this.sentences = sentences;
            this.paragraphDirPath = paragraphDirPath;
            listFiles(paragraphDirPath);
        }
        public void listFiles(String dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            FileInfo[] files = dirInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                handleFile(files[i].FullName);
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                listFiles(dirs[i].FullName);
            }
        }
        private void handleFile(String filepath)
        {
            try
            {
                StreamReader objReader = new StreamReader(filepath, Encoding.Default);
                string sLine = "";
                string paragraphText = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine == null)
                        break;
                    if (sLine == "")
                    {
                        if (!paragraphText.Equals(""))
                        {
                            Paragraph model = new Paragraph(paragraphText);
                            paragraphs.Add(model);
                            paragraphText = "";
                        }
                    }
                    else
                    {
                        paragraphText += (sLine + "\r\n");
                    }

                }
                if (paragraphText != null && !paragraphText.Equals(""))
                {
                    Paragraph model = new Paragraph(paragraphText);
                    paragraphs.Add(model);
                }
                objReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
