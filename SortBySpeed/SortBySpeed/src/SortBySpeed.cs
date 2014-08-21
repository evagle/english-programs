using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace SrtTimeModify.src
{
    class SortBySpeed
    {
        private List<Block> allBlocks = new List<Block>();
        private Hashtable table = new Hashtable();
        private string foler;
        private string resultFolder;
        private bool sortAll;

        public void articleToParagraphBlocks(string file)
        {
            List<string> list = FileHandler.read(file);
            List<Block> blocks = new List<Block>();

            Block b = new Block();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Trim().Equals("") && b.startTime != null && b.endTime != null)
                {
                    blocks.Add(b);
                    b = new Block();
                }
                else
                {
                    b.addLine(list[i]);
                }
            }
            if (b.startTime != null && b.endTime != null)
                blocks.Add(b);
            table[file.Replace(this.foler, this.resultFolder)] = blocks;
            if(sortAll)
                allBlocks.AddRange(blocks);
        }
      

      
        public SortBySpeed(string folderPath, bool sortAll)
        {
            this.foler = folderPath;
            this.sortAll = sortAll;
            this.resultFolder = this.foler + "-排序";
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

        public void sort()
        {
            allBlocks.Sort();
            foreach (List<Block> list in table.Values) {
                list.Sort();
            }
            
        }

        public void printBlockList(string file, List<Block> blocks)
        {
            foreach (Block b in blocks) {
                FileHandler.write(file, b.lines, true);
            }
        }

        public void  printBySpeed()
        {
            if (sortAll)
                printBlockList(this.resultFolder + "\\所有文件一起排序.txt", allBlocks);
            foreach (string key in table.Keys) {
                printBlockList(key, (List<Block>)table[key]);
            }

        }
       
       
    }

}
