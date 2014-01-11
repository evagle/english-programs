using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

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
                String destPath = destFolder+"\\"+key;
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

                String matchedName = mach(files[i].Name);
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
               
        public String mach(String fileName)
        {
            List<int> positions = indicesOf(fileName, "-", 0);
            positions.AddRange(indicesOf(fileName, ".", 0));
            for (int i = 0; i < positions.Count; i++)
            {
                String tmpName = fileName.Substring(0, positions[i]).TrimEnd();
                if (indices.hasIndex(tmpName))
                {
                    return tmpName;   
                }
            }
            return null;
        }
        public List<int> indicesOf(string s, string Search, int StartIndex)
        {
            List<int> indices = new List<int>();
            int lastIndex = 0;
            lastIndex = s.IndexOf(Search);
            while (lastIndex != -1)
            {
                indices.Add(lastIndex);
                lastIndex = s.IndexOf(Search, lastIndex+1); 
            }
            return indices ;
        }



        private void initIndex(String indexFilePath)
        {
            try
            {
                StreamReader objReader = new StreamReader(indexFilePath, Encoding.Default);
                string sLine = "";
                bool isfirstLine = true;
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
                            indices.addIndex(sLine);
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
