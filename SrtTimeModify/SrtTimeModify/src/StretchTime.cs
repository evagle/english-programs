using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrtTimeModify.src
{
    class StretchTime
    {
        private List<Block> blocks = new List<Block>();
        private bool isFillLongTimeSpan;
        public List<Block> getBlockList()
        {
            return this.blocks;
        }
        public void articleToParagraphBlocks( )
        {
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
        }
        public void mergeBlockByTimeSpanBetweenParagraphs(int timeBetweenParagraphs) {
            if (blocks == null || blocks.Count <= 1 || timeBetweenParagraphs==0)
                return;
            Block pre = blocks[0];
             
            List<Block> tmpList = new List<Block>();
            for (int i = 1; i < blocks.Count; i++)
            {
                Block post = blocks[i];
                 
                if (post.startTime.sub(pre.endTime) < timeBetweenParagraphs)
                {
                    pre = pre.mergeBlock(post);
                }
                else {
                    tmpList.Add(pre);
                    pre = post;
                }
            }
            tmpList.Add(pre);
            blocks = tmpList;
        }
        public List<String> getResultArticle(){
            List<string> result = new List<string>();
            for (int i = 0; i < blocks.Count; i++)
            {
                result.AddRange(blocks[i].getLines());
                result.Add("");
            }

            return result;
        }
        private List<string> list;
        private int timeSpan;
        public List<string> startTimeEndTime;

        public StretchTime(List<string> list, int t, bool isFillLongTimeSpan)
        {
            this.list = list;
            this.timeSpan = t;
            this.isFillLongTimeSpan = isFillLongTimeSpan;
            
        }
        public List<string> stretch(int timeBetweenParagraphs)
        {

            articleToParagraphBlocks();
            mergeBlockByTimeSpanBetweenParagraphs(timeBetweenParagraphs);
            addTime(this.timeSpan);
            

            //如果上一段的结束时间的毫秒数比下一段的开始时间毫秒数还要大，那么交换
            for (int i = 0; i < blocks.Count-1; i++) {
                Block pre = blocks[i];
                Block post = blocks[i+1];
                if (pre.endTime.intTime == post.startTime.intTime&&pre.endTime.millSecond.CompareTo(post.startTime.millSecond) > 0)
                {
                    String tmpMilli = post.startTime.millSecond;
                    post.startTime.setMilliSec(pre.endTime.millSecond);
                    pre.endTime.setMilliSec(tmpMilli);
                }
            }

            this.startTimeEndTime = new List<String>();
            for (int i = 0; i < blocks.Count; i++)
            {
                this.startTimeEndTime.Add(blocks[i].genStartEndTime());
            }

            return getResultArticle();
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
                    if (timeSpan - 2 * t >= t && isFillLongTimeSpan)
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
