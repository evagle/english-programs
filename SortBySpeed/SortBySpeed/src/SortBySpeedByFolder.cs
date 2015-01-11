using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace SrtTimeModify.src
{
    // 按照文件夹来排序，第二种方式
    class SortBySpeedByFolder
    {
        private List<Block> allBlocks = new List<Block>();
        private Hashtable table = new Hashtable();
        private string foler;
        private string resultFolder;
        private bool sortAll;
        private List<string> fileList = new List<string>();
        private Hashtable handledFiles = new Hashtable();

        private List<FileBlock> fileBlockList = new List<FileBlock>();
        private bool outputSpeed;
        public SortBySpeedByFolder(string folderPath, bool outputSpeed)
        {
            this.foler = folderPath;
            this.outputSpeed = outputSpeed;
            this.resultFolder = this.foler + "-排序";
            if (Directory.Exists(this.resultFolder))
            {
                Directory.Delete(this.resultFolder, true);
            }
            Directory.CreateDirectory(this.resultFolder);
            this.fileList = FileHandler.listFiles(folderPath, "");

            this.listFolders(this.foler);
            this.sort();
            this.printBySpeed();
        }

        public void listFolders(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {

                string srtfile = findSrtFile(dirs[i].FullName);
                if (srtfile != "")
                    fileBlockList.Add(new FileBlock(srtfile));; 
            }
        }

        public string findSrtFile(string folder) {
            DirectoryInfo dirInfo = new DirectoryInfo(folder);
            FileInfo[] files = dirInfo.GetFiles();
            
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].FullName.EndsWith("srt"))
                {
                    return files[i].FullName;
                    
                }
            }
            return "";
        }

  

        public void sort()
        {
            fileBlockList.Sort();
        }

        
        public string addSeqToFileName(string fileName, int seq)
        {
            int pos = fileName.LastIndexOf('\\');
            return fileName.Substring(0, pos + 1) + seq + "-" + fileName.Substring(pos + 1, fileName.Length - pos - 1);
        }

        public void moveFolder(string ofolder, int seq)
        {
            string folderName = ofolder.Substring(ofolder.LastIndexOf("\\"));
            folderName = seq + folderName.Substring(folderName.IndexOf("-") + 1);
            string destFolder = this.resultFolder +"\\"+ folderName;

            this.copyDirectory(ofolder, destFolder);
        }
        public void printBlockList(string ofolder, string filename, int seq, List<Block> blocks)
        {
            string folderName = ofolder.Substring(ofolder.LastIndexOf("\\"));
            folderName = seq + folderName.Substring(folderName.IndexOf("-") + 1);
            string destFolder = this.resultFolder + "\\" + folderName;

            string destFile = destFolder + filename;
            //destFile = addSeqToFileName(destFile, seq);
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }
            foreach (Block b in blocks)
            {
                FileHandler.write(destFile, b.linesWithSpeed, true);
            }
        }


        public void  printBySpeed()
        {
            for (int i = 0; i < fileBlockList.Count; i++) {
                this.moveFolder(fileBlockList[i].folderName, i + 1);
                if (this.outputSpeed) {
                    this.printBlockList(fileBlockList[i].folderName, fileBlockList[i].name, i + 1, fileBlockList[i].blocks);
                }
            }
        }

        private void copyDirectory(string strSource, string strDestination)
        {
            if (!Directory.Exists(strDestination))
            {
                Directory.CreateDirectory(strDestination);
            }
            DirectoryInfo dirInfo = new DirectoryInfo(strSource);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo tempfile in files)
            {
                tempfile.CopyTo(Path.Combine(strDestination, tempfile.Name));
            }
            DirectoryInfo[] dirctororys = dirInfo.GetDirectories();
            foreach (DirectoryInfo tempdir in dirctororys)
            {
                copyDirectory(Path.Combine(strSource, tempdir.Name), Path.Combine(strDestination, tempdir.Name));
            }

        }
       
       
    }

}

