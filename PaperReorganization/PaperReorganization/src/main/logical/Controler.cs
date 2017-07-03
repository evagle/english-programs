using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaperReorganization.src.main.model;
using System.IO;
using System.Collections;
using System.Threading;

namespace PaperReorganization.src.main.logical
{
    class Controler
    {
        //private List<Paragraph> paragraphs;
        //private List<Sentence> sentences;
        private Hashtable paragraphTable;
        private Hashtable paperTypeAverageWordsCount;
        private WordMap wordmap;  

        public Controler()
        {
            paragraphTable = new Hashtable();
            paperTypeAverageWordsCount = new Hashtable();
        }

        public void initAssosiateWords(String associateFilePath)
        {
            AssociateWords.prepare(associateFilePath, 0);
        }

        public void initWordList(String wordListPath)
        {
            PrepareWordList prepareWordList = new PrepareWordList();
            wordmap = prepareWordList.getWordList(wordListPath);
        }

        public void initZhenTi(String zhenTiPath)
        {
            
            DirectoryInfo dirInfo = new DirectoryInfo(zhenTiPath);
            FileInfo[] files = dirInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                //handleFile(files[i].FullName);
                string fileName = files[i].FullName;
                int pos = fileName.LastIndexOf(".");
                string suffix = fileName.Substring(pos, fileName.Length - pos);
                string prefix = files[i].Name.Substring(0,2);
                int pos1 = files[i].Name.LastIndexOf(".");
                if (prefix == "题型") { 
                    List<Paragraph> paragraphs = new List<Paragraph>();
                    List<Sentence> sentences = new List<Sentence>();
                    int type = Convert.ToInt32(files[i].Name.Substring(2,pos1-2));
                    new PrepareSentences(fileName, paragraphs, sentences, type);

                    paragraphs.Sort(delegate(Paragraph p1, Paragraph p2) { return p1.AveSentenceLen.CompareTo(p2.AveSentenceLen); });
                    for (int k = 0; k < paragraphs.Count; k++) {
                        paragraphs[k].aveSentenceLenRank = k + 1;
                    }
                    paragraphs.Sort(delegate(Paragraph p1, Paragraph p2) { return p2.AveNewWordsFriquence.CompareTo(p1.AveNewWordsFriquence); });
                    for (int k = 0; k < paragraphs.Count; k++)
                    {
                        paragraphs[k].aveNewWordsFriquenceRank = k + 1;
                    }
                    paragraphs.Sort(delegate(Paragraph p1, Paragraph p2) { return p1.NewWordsRate.CompareTo(p2.NewWordsRate); });
                    for (int k = 0; k < paragraphs.Count; k++)
                    {
                        paragraphs[k].newWordsRateRank = k + 1;
                    }
                    paragraphs.Sort(delegate(Paragraph p1, Paragraph p2) { return p1.GrammarScore.CompareTo(p2.GrammarScore); });
                    for (int k = 0; k < paragraphs.Count; k++)
                    {
                        paragraphs[k].grammarScoreRank = k + 1;
                    }

                    foreach (Paragraph p in paragraphs) {
                        p.generateFinalScore();
                    }
                    paragraphs.Sort(delegate(Paragraph p1, Paragraph p2) { return p1.FinalScore.CompareTo(p2.FinalScore); });
                    int count = 0;
                    foreach (Paragraph p in paragraphs) {
                        count += p.totalWordsCount;
                    }
                    if (paragraphs.Count > 0) {
                        this.paperTypeAverageWordsCount.Add(type, count / paragraphs.Count);
                    }
                   
                    paragraphTable.Add(type, paragraphs);
                }
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                //listFiles(dirs[i].FullName);
            }

        }

        public void initShengCi(string shengCiPath)
        {
            NewWord.initNewWords(shengCiPath);
        }

        public void initGrammar(string grammarPath)
        {
            PrepareGrammars p = new PrepareGrammars();
            p.initGrammars(grammarPath, 1);
        }

        public void generateNewPaper()
        {
            int f = Config.tiXingBiLi[0];
            foreach (int v in Config.tiXingBiLi) {
                if (f > v)
                {
                    f = gcd(f, v);
                }
                else {
                    f = gcd(v, f);
                }
            }
            // 统计按照一份比例来计算有多少词数
            int count = 0;
            for (int i = 0; i < Config.tiXingBiLi.Count; i++) {
                Config.tiXingBiLi[i] /= f; 
                count += ((int)this.paperTypeAverageWordsCount[i+1]) * Config.tiXingBiLi[i];
            }
            int num = Config.totalWordsCount / count;
            for (int i = 0; i < Config.tiXingBiLi.Count; i++)
            {
                Config.tiXingBiLi[i] *= num;
            }
            int seq = 1;
            while (true) {

                string fileName = Config.outPath + "新题第" + seq.ToString() + "套.txt";
                StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding("gbk"));
                for (int type = 0; type < Config.tiXingBiLi.Count; type++)
                {
                    List<Paragraph> result = new List<Paragraph>();
                    List<Paragraph> list = (List < Paragraph >) paragraphTable[type+1];
                    for (int j = 0; j < Config.tiXingBiLi[type]; j++) {
                        if (list.Count > 0)
                        {
                            //result.Add(list[0]);
                            writer.Write(list[0].Text+"\r\n");
                            list.RemoveAt(0);
                        }
                        else {
                            writer.Close();
                            File.Delete(fileName);
                            CLog.error("题型"+type.ToString()+"已用完");
                            return;
                        }
                    } 
                }
                writer.Close();
                CLog.debug("成功生词第" + seq.ToString() + "套试题");
                seq++;
            }

        }



        public int gcd(int a, int b) {
            if (b == 0)
                return a;
            return gcd(b, a % b);
        }
         
         
    }
}
