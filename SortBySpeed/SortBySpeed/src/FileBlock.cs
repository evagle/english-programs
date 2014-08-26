using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrtTimeModify.src
{
    class FileBlock : IComparable
    {
       
        public double speed = 0;
        public int wordCount = 0;
        public double totalTime = 0;
        public string fileName; //文件名带路径
        public string folderName; //路径
        public string name; //文件名不带路径
        public List<Block> blocks = new List<Block>();

        public FileBlock(string file)
        {
            this.folderName = file.Substring(0, file.LastIndexOf("\\"));
            this.name = file.Substring(file.LastIndexOf("\\"));
            this.fileName = file;
            List<string> list = FileHandler.read(file);
           
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

            foreach (Block block in blocks) {
                wordCount += block.wordCount;
                totalTime += block.totalTime;
            }
            if(totalTime > 0)
                speed = wordCount / totalTime;
        }


        #region IComparable 成员

        public int CompareTo(object obj)
        {
            if (this.speed > ((FileBlock)obj).speed)
                return 1;
            else
                return -1;
        }

        #endregion
    }
}
