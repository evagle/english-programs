using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FindAndMoveFilesWithSameName.src
{
    class ClipIndices
    {
        public Hashtable clipNames= new Hashtable();
        public void addIndex(String name, int seq){
            clipNames.Add(name.Trim(), seq);
        }

        public bool hasIndex(String name){
            return this.clipNames.ContainsKey(name);
        }

        
    }
}
