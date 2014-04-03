using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SrtTimeModify.src;
using System.Collections;
using System.IO;

namespace AssociateDict.src
{
    class Main
    {
        Hashtable associateDict;
        public void main(String associateFile,String abnormalFileDir,String outputDir)
        {
            associateDict = new Hashtable();
            readOldAssociateDict(associateFile);
            DirectoryInfo TheFolder = new DirectoryInfo(abnormalFileDir);
             
            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
                readAbnormalDict(NextFile.FullName);

            List<String> allDerivates = new List<String>();
            List<String> normalDerivates = new List<String>();
            List<String> otherDerivates = new List<String>();
            
            System.Collections.IDictionaryEnumerator enumerator = associateDict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                AssociateModel model = (AssociateModel)enumerator.Value;
                model.findNoramlAndMove();
                model.expandNormalDerivates();
                allDerivates.Add(model.getDerivates());
                normalDerivates.Add(model.getNormalDerivates());
                otherDerivates.Add(model.getOtherDerivates());
 
            }
            allDerivates.Sort();
            normalDerivates.Sort();
            otherDerivates.Sort();
            FileHandler.write(outputDir + "\\" + "关联词总表.txt", allDerivates);
            FileHandler.write(outputDir + "\\" + "关联词表1.txt", normalDerivates);
            FileHandler.write(outputDir + "\\" + "关联词表2.txt", otherDerivates);

        }
        /************************************************************************/
        /* 从文件中读如不规则词表
         * 第一行是类似这样的： ?-->??ing或者ie-->ying
         * 所以首先根据第一行来判断如何生成单词
         */
        /************************************************************************/
        public void readAbnormalDict(String path)
        {
            List<String> lines = FileHandler.read(path);
            if (lines.Count <= 1)
                return;
            String[] pattern = lines[0].ToLower().Trim().Replace("-", "").Split('>');
            lines.RemoveAt(0);
            if (pattern.Length != 2)
            {
                return;
            }
            if (pattern[0].Equals("?"))
            {
                doubleLastChar(pattern[1].Replace("?", ""), lines);
            }
            else
            {
                abnormalReform(pattern[1], pattern[0], lines);
            }
        }
        /************************************************************************/
        /* 处理形如    ?-->??ing的不规则形变                                                                 */
        /************************************************************************/
        public void doubleLastChar(String suffix,List<String> lines)
        {
            foreach (String l  in lines)
            {
                String line = l.Trim().ToLower();
                String w = line + line.Substring(line.Length - 1) + suffix;
                if (associateDict.ContainsKey(line))
                    ((AssociateModel)associateDict[line]).addDerivative(w);
                else
                {
                    associateDict.Add(line, new AssociateModel(line, new String[] { w }));
                }
            }
        }
        /************************************************************************/
        /* 处理形如 ie-->ying的形式                                                                     */
        /************************************************************************/
        public void abnormalReform(String suffix, String beReplace, List<String> lines)
        {
            foreach (String l in lines)
            {
                String line = l.Trim().ToLower();
                if(line.EndsWith(beReplace)){
                    String w = line.Substring(0,line.Length-beReplace.Length)+suffix;

                    if (associateDict.ContainsKey(line))
                    {
                        ((AssociateModel)associateDict[line]).addDerivative(w);
                    }else
                    {
                        associateDict.Add(line, new AssociateModel(line, new String[] { w }));
                    }
                }else{
                    //单词不符合要求，不处理
                    continue;
                }
              
            }   
        }
        //从文件中读入旧的关联词表
        public void readOldAssociateDict(String path)
        {
            List<String> lines = FileHandler.read(path);
            foreach (String line in lines)
            {
                String[] words = line.Trim().ToLower().Split(new char[]{' ','\t'},StringSplitOptions.RemoveEmptyEntries);
                String prototype = findPrototype(words);

                if (prototype != null)
                {
                    if (associateDict.ContainsKey(prototype))
                    {
                        ((AssociateModel)associateDict[prototype]).setDerivatives(words);
                    }
                    else
                    {
                        AssociateModel model = new AssociateModel(prototype, words);
                        associateDict.Add(prototype, model);
                    }
                }
            }
        }
        // 找出这一行单词的原形，先用最简单的方法，找最短的单词
        public String findPrototype(String[]  words)
        {
            int minLen = 1000;
            String ret = null;
            foreach (String w in words)
            {
                if (w.Length < minLen)
                {
                    minLen = w.Length;
                    ret = w;
                }
                 
            }
            return ret;
        }
    }
}
