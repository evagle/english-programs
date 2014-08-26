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
        private List<string> fileList = new List<string>();
        private Hashtable handledFiles = new Hashtable();
        private List<FileBlock> fileBlockList = new List<FileBlock>();
      
        public SortBySpeed(string folderPath)
        {
            this.foler = folderPath;
            //this.sortAll = sortAll;
            this.resultFolder = this.foler + "-排序";
            Directory.CreateDirectory(this.resultFolder);
            this.fileList = FileHandler.listFiles(folderPath, "");

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
                string baseFileName = files[i].FullName.Replace(".chs&eng.srt","").Replace(".eng&chs.srt","").Replace(".eng.srt", "").Replace(".chs.srt", "");
                   
                if (files[i].FullName.EndsWith("srt") && !handledFiles.ContainsKey(baseFileName))
                {    
                    fileBlockList.Add( new FileBlock(files[i].FullName));
                    handledFiles[baseFileName] = true;
                }
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                listFiles(dirs[i].FullName);
            }
        }

        public bool isFileHandled(string fileName)
        {
            return handledFiles.ContainsKey(fileName);
        }

        public void sort()
        {
            this.fileBlockList.Sort();
            
        }

        public string addSeqToFileName(string fileName, int seq)
        {
            int pos = fileName.LastIndexOf('\\');
            return fileName.Substring(0, pos + 1) + seq  + fileName.Substring(pos + 1, fileName.Length - pos - 1);
        }

        public void moveRelativeFile(string file, int seq)
        {
            string baseFileName = file.Replace(".chs&eng.srt", "").Replace(".eng&chs.srt", "").Replace(".eng.srt", "").Replace(".chs.srt", "");
               
            foreach (string fileName in fileList) { 
                if (fileName.StartsWith(baseFileName) && !fileName.Equals(file) && File.Exists(fileName))
                {
                    string destFile = this.addSeqToFileName(fileName.Replace(this.foler, this.resultFolder), seq);
                    File.Copy(fileName, destFile);
                }
            }
        }

        public void  printBySpeed()
        {
            //if (sortAll)
            //    printBlockList(this.resultFolder + "\\所有文件一起排序.txt", allBlocks);
            /*for (int i = 0; i < table.Keys.Count; i++) {
                string key = table.Keys[i];
                printBlockList(key, i, (List<Block>)table[key]);
            }*/
            for (int i = 0; i < fileBlockList.Count; i++) {
                this.printBlockList(fileBlockList[i].fileName, i+1, fileBlockList[i].blocks);
            }
               

        }

        public void printBlockList(string file, int seq, List<Block> blocks)
        {
            string destFile = file.Replace(this.foler, this.resultFolder);
            destFile = addSeqToFileName(destFile, seq);
            foreach (Block b in blocks)
            {
                FileHandler.write(destFile, b.lines, true);
            }
            this.moveRelativeFile(file, seq);
        }

    }

}

