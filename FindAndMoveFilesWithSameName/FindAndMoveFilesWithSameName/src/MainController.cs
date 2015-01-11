using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace FindAndMoveFilesWithSameName.src
{
    class MainController
    {
        private ClipIndices indices = new ClipIndices();
        private String[] targetFolders ;
        private String destFolder;
        private Hashtable matchResult;
       
        public MainController(String indexFilePath, String[] possibleTargetFolders, String destFolder)
        {
            initIndex(indexFilePath);
            this.targetFolders = possibleTargetFolders;
            this.destFolder = destFolder;
            matchResult = new Hashtable ();
            foreach (String str in possibleTargetFolders)
            {
                findFiles(str,0,15);
            }
            
        }
        public void moveFiles()
        {
            foreach (String key in matchResult.Keys)
            {
                ClipModel model = (ClipModel)matchResult[key];
                String destPath = destFolder+"\\" + model.Seq + "-" + key;
                if (Directory.Exists(destPath))
                {
                    Directory.Delete(destPath, true);
                }
                Directory.CreateDirectory(destPath);
                for  (int i=0;i<model.fileNames.Count;i++)
                {
                    String srcFile = model.filePaths[i];
                    if (srcFile.StartsWith(destFolder))
                    {
                        continue;
                    }
                    String destFile = destPath + "\\" + model.fileNames[i];
                    File.Copy(srcFile, destFile,true);
                }
            }
        }
        public void findFiles(String dirPath,int depth,int maxDepth)
        {
            if (dirPath.Equals(""))
                return;
            if (!Directory.Exists(dirPath))
                return;
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            FileInfo[] files = dirInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                int seq;
                String matchedName = mach(files[i].Name, out seq);
                if (matchedName != null)
                {
                    if (matchResult.ContainsKey(matchedName))
                    {
                        ClipModel model = (ClipModel)matchResult[matchedName];
                        model.addFile(files[i].FullName,files[i].Name);
                    }
                    else
                    {
                        ClipModel model = new ClipModel(matchedName); ;
                        model.addFile(files[i].FullName,files[i].Name);
                        model.Seq = seq;
                        matchResult.Add(matchedName, model);

                    }
                }
            }
            if (depth < maxDepth) { 
                DirectoryInfo[] dirs = dirInfo.GetDirectories();
                for (int i = 0; i < dirs.Length; i++)
                {
                    findFiles(dirs[i].FullName,depth+1,maxDepth);
                }
            }
        }
        //按照{-.}来切分，然后去掉后缀和-E、-C之后的进行匹配
               
        public String mach(String fileName, out int seq)
        {
            List<int> positions = indicesOf(fileName, "-", 0);
            positions.AddRange(indicesOf(fileName, ".", 0));
            for (int i = 0; i < positions.Count; i++)
            {
                String tmpName = fileName.Substring(0, positions[i]).TrimEnd();
                if (indices.hasIndex(tmpName))
                {
                    seq = (int)indices.clipNames[tmpName];
                    return tmpName;   
                }
            }
           
            Regex r = new Regex(".*[-\\.][0-9]+([-\\.][0-9]+)*");
            Match m = r.Match(fileName);
            string name = fileName;
            if (m.Success)
            {
                name = m.Value;
               
            }

            // 片名+序号的格式有：（1）片名-数字 （2）片名.数字  （3）片名-数字-数字 
            // 如果能够提取上述格式的文件名，那么就去匹配,前缀能够匹配上就算符合条件
            foreach (string key in indices.clipNames.Keys)
            {
                if (key.StartsWith(name) || name.StartsWith(key))
                {
                    seq = (int)indices.clipNames[key];
                    return key;
                }
            }
            
            seq = 0;
            return null;
        }

        public List<int> indicesOf(string s, string Search, int StartIndex)
        {
            List<int> indicePos = new List<int>();
            int lastIndex = 0;
            lastIndex = s.IndexOf(Search);
            while (lastIndex != -1)
            {
                indicePos.Add(lastIndex);
                lastIndex = s.IndexOf(Search, lastIndex+1); 
            }
            return indicePos;
        }



        private void initIndex(String indexFilePath)
        {
            try
            {
                StreamReader objReader = new StreamReader(indexFilePath, Encoding.Default);
                string sLine = "";
                bool isfirstLine = true;
                int seq = 0;
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine == null)
                        break;
                    if (sLine.Equals(""))
                    {
                        isfirstLine = true;
                    }
                    else
                    {
                        if (isfirstLine)
                        {

                            // 第一行的第一个空格前的部分作为视频的名字

                            Regex r = new Regex(".*[-\\.][0-9]+([-\\.][0-9]+)*");
                            Match m = r.Match(sLine);
                            string name = sLine;
                            if (m.Success)
                            {
                                name = m.Value;
                            } 
                           
                            if (!indices.hasIndex(name))
                            {
                                indices.addIndex(name, ++seq);   
                            }
                            isfirstLine = false;
                        }
                    }
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
