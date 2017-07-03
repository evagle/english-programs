using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaperReorganization.src.main.model;
using System.IO;
using System.Collections;

namespace PaperReorganization.src.main.logical
{
    class PrepareSentences
    {
        private List<Paragraph> paragraphs;
        private List<Sentence> sentences;
        private String paragraphDirPath;
        private int paperType; // 题型
        public PrepareSentences(String paragraphDirPath,List<Paragraph> paragraphs,List<Sentence> sentences)
        {
            this.paragraphs = paragraphs;
            this.sentences = sentences;
            this.paragraphDirPath = paragraphDirPath;
            listFiles(paragraphDirPath);
        }
        public PrepareSentences(String paragraphFile, List<Paragraph> paragraphs, List<Sentence> sentences, int paperType)
        {
            this.paragraphs = paragraphs;
            this.sentences = sentences;
            //this.paragraphDirPath = paragraphDirPath;
            //listFiles(paragraphDirPath);
            handleFile(paragraphFile);
            this.paperType = paperType;
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
                StreamReader objReader = new StreamReader(filepath, Encoding.GetEncoding("gbk"));
                string sLine = "";
                string paragraphText = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine == null)
                        break;
                    if (sLine.Trim() == "")
                    {
                        if (!paragraphText.Equals(""))
                        {
                            paragraphText = paragraphText.Replace('，', ',');
                            paragraphText = paragraphText.Replace('？', '?');
                            paragraphText = paragraphText.Replace('！', '!');
                            paragraphText = paragraphText.Replace('。', '.');
                            paragraphText = paragraphText.Replace('．', '.');
                            
                            Paragraph model = new Paragraph(paragraphText);
                            model.PaperType = this.paperType;
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
                    model.PaperType = this.paperType;
                    paragraphs.Add(model);
                }
                objReader.Close();
            }
            catch (Exception e)
            {
                CLog.error("读取题型数据出错：error:" + e.Message + "  trace:" + e.StackTrace);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
