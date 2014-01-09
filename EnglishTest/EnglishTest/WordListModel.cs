using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnglishTest
{
    class WordListModel
    {
        //词表的默认中文名称
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string path;
        //词表的相对路径
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string enName;
        //词表的英文名称
        public string EnName
        {
            get { return enName; }
            set { enName = value; }
        }
        override
        public string ToString()
        {
            return name + "#" + path + "#" + enName;
        }

    }
}
