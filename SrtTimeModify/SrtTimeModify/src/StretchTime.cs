using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrtTimeModify.src
{
    class StretchTime
    {
        private List<Block> blocks = new List<Block>();
        public List<string> stretch(List<string> list, int t,out List<string> startTimeEndTime)
        {
            Block b = new Block();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Trim().Equals("") && b.startTime != null&&b.endTime!=null)
                { 
                    blocks.Add(b);
                    b = new Block();
                }
                else
                {
                    b.addLine(list[i]);
                }
            }
            if (b.startTime != null&&b.endTime!=null)
                blocks.Add(b);
            addTime(t);
            List<string> result = new List<string>();
            for (int i = 0; i < blocks.Count; i++)
            {
                result.AddRange(blocks[i].getLines());
                result.Add("");
            }
            startTimeEndTime = new List<String>();
            for (int i = 0; i < blocks.Count; i++)
            {
                startTimeEndTime.Add(blocks[i].genStartEndTime());
            }
            return result;
        }
        private void addTime(int t)
        {
            List<Block> tmp = new List<Block>();
            Block preBlock = null;
            Block postBlock = null;
            for(int i=1;i<blocks.Count;i++){
                 preBlock = blocks[i-1];
                 postBlock = blocks[i];
                int timeSpan = postBlock.startTime.intTime - preBlock.endTime.intTime;
                if (timeSpan >= 2*t)
                {
                    postBlock.amendStartTime(-t);
                    preBlock.amendEndTime(t);
                    tmp.Add(preBlock);
                    if (timeSpan - 2 * t >= t)
                    {
                        ITime newTimeStart = preBlock.endTime.clone();
                        Block b ;
                        while (newTimeStart.intTime + t < postBlock.startTime.intTime)
                        {
                            b = new Block();
                            b.startTime = newTimeStart.clone();
                            b.endTime= newTimeStart.clone().amend(t);
                            b.genTimeString();
                            b.addLine("");
                            tmp.Add( b);
                            newTimeStart.amend(t);
                        }
                         b = new Block();
                        b.startTime = newTimeStart.clone();
                        b.endTime = postBlock.startTime.clone();
                        b.genTimeString();
                        b.addLine("");
                        tmp.Add(b);
                         
                    }
                }
                else
                {
                    postBlock.startTime.amend(-timeSpan / 2);
                    preBlock.endTime.amend(timeSpan / 2);
                    tmp.Add(preBlock);
                }
                
            }
            if (postBlock != null)
            {
                postBlock.amendEndTime(t);
                tmp.Add(postBlock);
            }
            
            blocks = tmp;
        }
    }

}
