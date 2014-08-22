using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FindAndMoveFilesWithSameName.src
{
    //电影片段，包括3个srt字幕文件和3个视频片段
    class ClipModel
    {
        public List<String> filePaths = new List<String>();
        public List<String> fileNames = new List<String>();
        public String indexName;
        private HashSet<String> fileSet = new HashSet<String>();
        private int seq = 0;

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }
        public void addFile(String path, String name)
        {
            if (!fileSet.Contains(path))
            {
                filePaths.Add(path);
                fileNames.Add(name);
                fileSet.Add(path);
            }
        }
        public ClipModel(String indexName)
        {
            this.indexName = indexName;
        }

    }
}
