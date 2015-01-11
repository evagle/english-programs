using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace SrtTimeModify.src
{
    class SortBySpeedInFile
    {
        private List<Block> allBlocks = new List<Block>();
        private Hashtable table = new Hashtable();
        private string foler;
        private string resultFolder;
        private bool sortAll = false;

        private bool outputSpeed;


        public SortBySpeedInFile(string folderPath, bool outputSpeed)
        {
            this.foler = folderPath;
            this.outputSpeed = outputSpeed;
            //this.sortAll = sortAll;
            this.resultFolder = this.foler + "-排序";
            if (Directory.Exists(this.resultFolder))
            {
                Directory.Delete(this.resultFolder, true);
            }
            Directory.CreateDirectory(this.resultFolder);

            this.listFiles(this.foler);
            this.sort();
            this.printBySpeed();
        }

        public void listFiles(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                this.articleToParagraphBlocks(files[i].FullName);

            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                listFiles(dirs[i].FullName);
            }
        }
        public bool isTitle(string str)
        {
            
            bool title =  str.IndexOf("eng.srt") >= 0 || str.IndexOf("chs.srt") >= 0 ||
                str.IndexOf("chs&eng.srt") >= 0 || str.IndexOf("eng&chs.srt") >= 0 ;
            Regex r = new Regex("\\.[0-9]+$");
            if (r.IsMatch(str)) {
                title = true;
            }
            return title;
        }
        public void articleToParagraphBlocks(string file)
        {
            List<string> list = FileHandler.read(file);
            List<Block> blocks = new List<Block>();

            Block b = new Block();
            for (int i = 0; i < list.Count; i++)
            {
                if (isTitle(list[i].Trim()))
                {
                    if (b.startTime != null && b.endTime != null)
                    {
                        b.addBlockSpeed();
                        blocks.Add(b);
                        b = new Block();
                        b.addLine(list[i], true);
                    }
                    else { //第一行的title
                        b.addLine(list[i], true);
                    }
                }
                else
                {
                    b.addLine(list[i], false);
                }
            }
            if (b.startTime != null && b.endTime != null)
            {
                b.addBlockSpeed();
                blocks.Add(b);
            }
            table[file.Replace(this.foler, this.resultFolder)] = blocks;
            if (sortAll)
                allBlocks.AddRange(blocks);
        }


        public void sort()
        {
           
            foreach (List<Block> list in table.Values)
            {
                list.Sort();
            }

        }

        public void printBlockList(string file, List<Block> blocks)
        {
            foreach (Block b in blocks)
            {
                FileHandler.write(file, b.lines, true);
            }
        }
        public void printBlockListWithSpeed(string file, List<Block> blocks)
        {
            foreach (Block b in blocks)
            {
                FileHandler.write(file, b.linesWithSpeed, true);
            }
        }

        public void printBySpeed()
        {
            
            foreach (string key in table.Keys)
            {
                if (this.outputSpeed)
                    printBlockListWithSpeed(key, (List<Block>)table[key]);
                else
                    printBlockList(key, (List<Block>)table[key]);
            }

        }


    }

}
