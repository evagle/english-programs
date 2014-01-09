using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace CutPaper.src.cutpaper
{
    class CutPapers
    {
        private String[] selections = new String[] {
            "A.","B.","C.","D.","A)","B)","C)","D)",
            "[A]","[B]","[C]","[D]","A．","B．","C．","D．"
        };
        private String[] keywords = new String[] { "^Part", "^Section", "^Directions", "^注意",
            "听第.*段对话", "完形填空", "^阅读下面短文", "阅读理解",
            "短文改错", "书面表达" ,"单项填空","听","题","答","英语",
            "画","一","二","三","划","四","六","^Questions [0-9]* to","^Passage",
        "^Text[ \t]*[0-9]","[ \t0-9]*Directions","Choose the best.*for each numbered blank"
        ,"Read the following.*text","choose the most suitable paragraphs from the list"
        ,"部分"};
        private String[] listeningpattern = new String[] {
            "Listening[ \t]*Comprehension","听力",
            "Directions.*hear.*conversations","对话","独白",
            "Directions.*hear.*passage"
        };
        private String[] readingpattern = new String[]{
            "Reading[ \t]*Comprehension","阅读理解",
            "choose the most suitable paragraphs from the list"
        };
        private String[] clozepattern = new String[]{
            "Cloze","完型","完形","Directions.*There are.*blanks",
            "Read .*the.*following.*text.*Choose.*the best.*word"
        };
        private String[] correctionpattern = new String[]{
            "改错"
        };
        private String[] translationpattern = new String[] {
            "Translation"
        };
        private String[] answerpattern = new String[]{
            "参考答案"
        };
        private String[] selectionpattern = new String[]{
            "单项填空","Vocabulary[ \t]*and[ \t]*Structure",
            "Directions.*There are.*incomplete sentences"
        };
        private String[] writingpattern = new String[] {
            "书面表达","Writing"
        };
        private String[] examplepattern = new String[]{
            "^例","^[Ee]xample"
        };


        public List<Block> process(String path, string outpath)
        {
            //path = "D:\\Download\\试卷\\cet4-1989.txt";
            List<String> paper = getPaper(path);
            List<Block> blocks = cutBySelection(paper);
            blocks = findKeyLineBlock(blocks, paper);
            setRightBlockTag(blocks);
            removeAttentionBlock(blocks);
            List<Block> finalBlocks = getFinalBlockList(blocks);
            mergeReading(finalBlocks);
            addSeqToBlock(finalBlocks);
            Hashtable table = getAnswer(finalBlocks);
            matchAnswer(finalBlocks, table);
            printBlocks(finalBlocks, outpath);
            return clean(finalBlocks);
        }
        private List<Block> clean(List<Block> finalBlocks)
        {
            List<Block> list =  new List<Block>();
            foreach (Block b in finalBlocks)
            {
                if (b.Type == Block.BLOCK_TYPE.answer)
                {
                    break;
                }
                else if (b.Type == Block.BLOCK_TYPE.keyline || 
                    b.Type == Block.BLOCK_TYPE.example)
                {
                    continue;
                }
                else
                {
                    list.Add(b);
                }
            }
            return list;
        }
        private void matchAnswer(List<Block> finalBlocks, Hashtable anstable)
        {
            foreach (Block b in finalBlocks)
            {
                foreach (int seq in b.Seqs)
                {
                    if (anstable.ContainsKey(seq))
                    {
                        b.Answers.Add((String)anstable[seq]);
                    }
                }
            }
        }
        public Hashtable getAnswer(List<Block> finalBlocks)
        {
            bool answerStarted = false ;
          
            Hashtable anstable = new Hashtable();
            foreach (Block b in finalBlocks)
            {
                if ((b.Type == Block.BLOCK_TYPE.answer || answerStarted )
                && b.Type != Block.BLOCK_TYPE.keyline)
                {
                    answerStarted = true;
                    foreach(String line in b.Lines){
                        Regex r = new Regex("[0-9]+[.．][^0-9]*");
                        MatchCollection matches = r.Matches(line);
                        foreach (Match match in matches)
                        {
                            foreach (Capture capture in match.Captures)
                            {
                                String value= capture.Value;
                                int cur = 0;
                                String ans = "";
                                getSeq(value, out cur, out ans);
                                if(!anstable.ContainsKey(cur))
                                    anstable.Add(cur, ans);
                            }
                        }
                    }
                }
            }
            return anstable;
        }
        private void getSeq(String line,out int seq,out String ans)
        {
            seq = -1;
            ans = "";
            if(line.Contains(".")||line.Contains("．")){
                if (line.ToCharArray()[1].Equals('.') || line.ToCharArray()[1].Equals('．'))
                {
                    seq = Int32.Parse(line.Substring(0, 1));
                    ans = line.Substring(2, line.Length-2) ;
                }
                else
                {
                    seq = Int32.Parse(line.Substring(0, 2));
                    ans = line.Substring(3, line.Length-3);
                }
            }
            
        }
        private void mergeReading(List<Block> finalBlocks)
        {
            for (int i = finalBlocks.Count - 1; i > 0; i--)
            {
                if (finalBlocks.ElementAt(i).Type == finalBlocks.ElementAt(i - 1).Type &&
                    finalBlocks.ElementAt(i).Type == Block.BLOCK_TYPE.reading)
                {
                    Block newblock = mergeBlock(finalBlocks.ElementAt(i-1), finalBlocks.ElementAt(i));
                    finalBlocks.RemoveAt(i );
                    finalBlocks.RemoveAt(i-1 );
                    finalBlocks.Insert(i-1, newblock);
                }
            }
        }
        private Block mergeBlock(Block b1, Block b2)
        {
            Block block = new Block();
            block.Type = b1.Type;
            block.Lines.AddRange(b1.Lines);
            block.Lines.AddRange(b2.Lines);
            block.Seqs.AddRange(b1.Seqs);
            block.Seqs.AddRange(b2.Seqs);
            return block;
        }
        private Block mergeBlock(List<Block> list, Block.BLOCK_TYPE type, int start, int end)
        {
            Block block = new Block();
            block.Type = type;
            for (int i = start; i < end; i++)
            {
                block.Lines.AddRange(list.ElementAt(i).Lines);
                block.Seqs.AddRange(list.ElementAt(i).Seqs);
            }
              
            return block;
        }
        public void removeAttentionBlock(List<Block> blocks)
        {
            Regex r = new Regex("^注意");
            for (int i = blocks.Count - 1; i >= 0;i-- )
            {
                if (r.IsMatch(blocks.ElementAt(i).getString()))
                    blocks.RemoveAt(i);
            }
        }
        private void addSeqToBlock(List<Block> finalBlocks)
        {
            int curseq = 1;
            foreach (Block block in finalBlocks)
            {
                if (block.Type == Block.BLOCK_TYPE.selection  ||
                    block.Type == Block.BLOCK_TYPE.selection_reading)
                {
                    while (!block.getString().Contains(curseq.ToString())) { curseq++; }
                    //if (block.getString().Contains(curseq.ToString()))
                    //{
                    if (block.Seqs.Count == 0)
                    {
                        block.Seqs.Add(curseq);
                        curseq++;
                    }
                    else
                        curseq = block.Seqs[0] + 1;
                    //}
                    
                }
                else if (block.Type == Block.BLOCK_TYPE.closen)
                {
                    while (block.getString().Contains(curseq.ToString()))
                    {
                       // block.Seqs.Add(curseq);
                        curseq++;
                    }
                }
                else if (block.Type == Block.BLOCK_TYPE.listening || block.Type == Block.BLOCK_TYPE.reading ||
                    block.Type == Block.BLOCK_TYPE.correction || block.Type == Block.BLOCK_TYPE.translation)
                {
                   // while (!block.getString().Contains(curseq.ToString())) { curseq++; }
                    while (block.getString().Contains(curseq.ToString()))
                    {
                        block.Seqs.Add(curseq);
                        curseq++;
                    }
                }
            }

        }
        private List<Block> getFinalBlockList(List<Block> blocks)
        {
            List<Block> finalList = new List<Block>();

            for (int i = 0; i < blocks.Count;  )
            {
                Block block = blocks.ElementAt(i);
                if (block.Type == Block.BLOCK_TYPE.closen)
                {
                    List<Block> tmpList = new List<Block>();
                    i = splitClozeBlock(blocks, i, tmpList);
                    finalList.AddRange(tmpList);
                } 
                else if (block.Type == Block.BLOCK_TYPE.reading)
                {
                    List<Block> tmpList = new List<Block>();
                    i = splitReadingComprehension(blocks, i, tmpList);
                    finalList.AddRange(tmpList);
                }
                else if ( block.Type == Block.BLOCK_TYPE.correction
                    ||block.Type == Block.BLOCK_TYPE.translation)
                {
                    List<Block> tmpList = new List<Block>();
                    i = splitOthers(blocks, i, tmpList);
                    finalList.AddRange(tmpList);
                }
                else
                {
                    finalList.Add(block);
                    i++;
                }
            }
            return finalList;

        }
        private int splitOthers(List<Block> blocks, int blockPos , List<Block> retlist)
        {
            Regex r = new Regex("[0-9]+");
            int ptr=0;
            Block block = blocks.ElementAt(blockPos);
            for (int i = 0; i < block.Lines.Count; i++)
            {
                String str = block.Lines.ElementAt(i);
                if (r.IsMatch(str))
                {
                    String tmp = "";
                    for(int k=ptr;k<=i;k++)
                        tmp += block.Lines.ElementAt(k);
                    retlist.Add(newBlock(block.Type, tmp));
                    ptr = i + 1;
              //      retlist.Add(newBlock(type,str))
                }
            }
            return blockPos + 1;
        }
        private int splitReadingComprehension(List<Block> blocks, int blockPos, List<Block> retlist)
        {
            int qStart = -1;
            int qEnd = -1;
            int k;
            if (blocks.ElementAt(blockPos + 1).Type != Block.BLOCK_TYPE.selection)
            {
                retlist.Add(blocks.ElementAt(blockPos));
                return blockPos + 1;
            }
            for (k = blockPos + 1; k < blocks.Count; k++)
            {
                if (blocks.ElementAt(k).Type == Block.BLOCK_TYPE.selection && qStart < 0)
                {
                    qStart = k;
                }
                if (blocks.ElementAt(k).Type != Block.BLOCK_TYPE.selection && qStart > 0)
                {
                    qEnd = k;
                    break;
                }
            }
            List<String> contents = splitPassage(blocks.ElementAt(blockPos), qEnd - qStart);
            for (int i = 0; i < contents.Count; i++)
            {
                retlist.Add(newBlock(Block.BLOCK_TYPE.selection_reading, contents.ElementAt(i), blocks.ElementAt(qStart + i)));
            }
            return qEnd;

        }
        private List<String> splitPassage(Block block, int qNum)
        {
            List<String> ret = new List<String>();
            List<String> list = splitToSentences(block.getString());
            int n = list.Count / qNum;
            n = n == 0 ? 1 : n;
            for (int i = 0; i < qNum; i++)
            {
                StringBuilder builder = new StringBuilder();
                for (int j = Math.Max(i*n - 1,0); j < Math.Min((i+1)*n+1,list.Count); j++)
                {
                    builder.Append(list.ElementAt(j));
                }
                ret.Add(builder.ToString());
            }
            return ret;
        }
        private int splitClozeBlock(List<Block> blocks, int blockPos, List<Block> clozeList)
        {
            int qStart = -1;
            int qEnd = -1;
            int k;
            for (k = blockPos + 1; k < blocks.Count; k++)
            {
                if (blocks.ElementAt(k).Type == Block.BLOCK_TYPE.selection && qStart < 0)
                {
                    qStart = k;
                }
                if (blocks.ElementAt(k).Type != Block.BLOCK_TYPE.selection && qStart > 0)
                {
                    qEnd = k;
                    break;
                }
            }
            if(qEnd == -1)
                qEnd = blockPos+1;
            String str = blocks.ElementAt(qStart).Lines.ElementAt(0);
            int seq=0;
            if(str.ToCharArray()[1]>='0'&&str.ToCharArray()[1]<='9'){
                seq = Int32.Parse(str.Substring(0,2));
            }else
                seq = Int32.Parse(str.Substring(0, 1));
            List<String> list = splitClozeToSentences(blocks.ElementAt(blockPos), qEnd - qStart, seq);
            for (int i = 0; i < list.Count; i++)
            {
                clozeList.Add(newBlock(Block.BLOCK_TYPE.closen, list.ElementAt(i), blocks.ElementAt(qStart + i)));
            }
            //clozeList.Add(mergeBlock(blocks,Block.BLOCK_TYPE.closen,blockPos,qEnd));
            return qEnd;
        }
        public  List<String> splitClozeToSentences(Block block,int qNum,int startSeq){
            List<String> res= new List<String>();
            List<String> list = splitToSentences(block.getString());
            int cur=0;
            for(int i=0;i<list.Count && qNum > 0;)
            {
                if(list.ElementAt(i).Contains(startSeq.ToString())){
                    String tmp="";
                    for(int k=Math.Min(cur,i);k<=i;k++)
                        tmp +=list.ElementAt(k);
                    res.Add(tmp);
                    cur = i+1;
                    startSeq++;
                    qNum--;
                }else
                    i++;
            }
            return res;

        }
        public List<String> splitToSentences(String text)
        {
            HashSet<String> map = new HashSet<String>();
            List<String> list = new List<String>();
            map.Add(".");
            map.Add("!");
            map.Add("?");
            int start = 0;
            for (int i = 0; i < (text.Length); i++)
            {
                //if (map.Contains(text.Substring(i, 1)) && isNextSentence(text.ToCharArray(), i + 1))
                if (map.Contains(text.Substring(i, 1))  )
                {
                    list.Add((text.Substring(start, i + 1 - start)));
                    //Sentence sentence = new Sentence(text.Substring(start, i + 1 - start));
                    //sentence.Text = text.Substring(start, i + 1 - start);
                   // sentences.Add(sentence);
                    start = i + 1;
                }
            }
            if (start < text.Length && !text.Substring(start, text.Length - start).Trim().Equals(""))
            {
                list.Add(text.Substring(start, text.Length - start));
                //Sentence sentence = new Sentence(text.Substring(start, text.Length - start));
               // sentences.Add(sentence);
            }
            return list;
        }
        private bool isNextSentence(char[] chs, int start)
        {

            for (int i = start; i < chs.Length; i++)
            {
                if ((chs[i] >= 'a' && chs[i] <= 'z') || (chs[i] >= 'A' && chs[i] <= 'Z'))
                {
                    if (Char.IsUpper(chs[i]))
                    {
                        if ((i + 1 < chs.Length && (chs[i + 1] == ' ' || (chs[i + 1] >= 'a' && chs[i + 1] <= 'z'))))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else if (chs[i] >= '0' && chs[i] <= '9')
                    return true;
            }
            return true;
        }
        private void setRightBlockTag(List<Block> blocks)
        {
            if (blocks == null || blocks.Count == 0)
                return;
            Block.BLOCK_TYPE type = Block.BLOCK_TYPE.unknown;
            
            for(int i=0;i<blocks.Count;i++)
            {
                Block block = blocks.ElementAt(i);
                if (block.Type == Block.BLOCK_TYPE.keyline && type != Block.BLOCK_TYPE.answer)
                {
                    //listening
                    if (isblockContainsPattern(block, listeningpattern))
                    {
                        type = Block.BLOCK_TYPE.listening;
                    }
                    else if (isblockContainsPattern(block, readingpattern))
                    {
                        type = Block.BLOCK_TYPE.reading;
                    }
                    else if (isblockContainsPattern(block, clozepattern))
                    {
                        type = Block.BLOCK_TYPE.closen;
                    }
                    else if (isblockContainsPattern(block, correctionpattern))
                    {
                        type = Block.BLOCK_TYPE.correction;
                    }
                    else if (isblockContainsPattern(block, translationpattern))
                    {
                        type = Block.BLOCK_TYPE.translation;
                    }
                    else if (isblockContainsPattern(block, answerpattern))
                    {
                        
                        if (i > 0 && blocks.ElementAt(i - 1).Type == Block.BLOCK_TYPE.example)
                        {
                            type = Block.BLOCK_TYPE.example;
                        }
                        else
                            type = Block.BLOCK_TYPE.answer;
                    }
                    else if (isblockContainsPattern(block, selectionpattern))
                    {
                        type = Block.BLOCK_TYPE.selection;
                    }
                    else if (isblockContainsPattern(block, writingpattern))
                    {
                        type = Block.BLOCK_TYPE.writing;
                    }

                }else if(block.Type == Block.BLOCK_TYPE.unknown){
                    if (isblockContainsPattern(block, examplepattern)){
                        block.Type = Block.BLOCK_TYPE.example;
                    }else
                        block.Type = type;
                }
            }
        }
        private bool isblockContainsPattern(Block block, String[] patterns)
        {
            foreach (String line in block.Lines)
            {
                for (int i = 0; i < patterns.Length; i++)
                {
                    Regex r = new Regex(patterns[i]);
                    if (r.IsMatch(line.Trim()))
                        return true;
                }
            }
            return false;
        }
        public List<Block> cutBySelection(List<String> paper)
        {
            List<Block> blocks = new List<Block>();
            int lastEnd = 0;
             Block block;
            for (int i = 0; i < paper.Count;  )
            {
                if (isSelectionStart(paper, i))
                {
                    if (lastEnd  < i)
                    {
                        block = new Block();
                        block.StartLineNum = lastEnd  ;
                        block.EndLineNum = i;
                        block.Type = Block.BLOCK_TYPE.unknown;
                        copyContent(paper, block);
                      
                        blocks.Add(block);
                    }
                    int newEnd = findSelectionEnd(paper, i+1);
                    block = new Block();
                    block.StartLineNum = i;
                    block.EndLineNum = newEnd;
                    block.Type = Block.BLOCK_TYPE.selection;
                    copyContent(paper, block);
                    getSelectionSeq(block);
                    blocks.Add(block);
                    lastEnd = newEnd;
                    i = lastEnd;
                }else
                    i++;
            }
            block = new Block();
            block.StartLineNum = lastEnd;
            block.EndLineNum = paper.Count;
            block.Type = Block.BLOCK_TYPE.unknown;
            copyContent(paper, block);
            
            blocks.Add(block);
            return blocks;
        }
        private int getSelectionSeq(Block block)
        {
            int len = 0;
            if (block.Lines[0].Substring(1, 1).Equals(".") || block.Lines[0].Substring(1, 1).Equals("．"))
                len = 1;
            else
                len = 2;
            try
            {
                int seq = Int32.Parse(block.Lines[0].Substring(0, len));
                block.Seqs.Add(seq);
                return seq;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        private List<Block> findKeyLineBlock(List<Block> blocks, List<String> paper)
        {
            List<Block> smallBlock = new List<Block>();
            foreach (Block b in blocks)
            {
                if (b.Type == Block.BLOCK_TYPE.unknown)
                {
                    Block block;
                    int lastEnd = 0;
                    for (int i = 0; i < b.Lines.Count; i++)
                    {
                        if (isStartWithKeywords(b.Lines.ElementAt(i)))
                        {

                            if (lastEnd < i)
                            {
                                block = newBlock(b.StartLineNum + lastEnd, b.StartLineNum + i, Block.BLOCK_TYPE.unknown, paper);
                                smallBlock.Add(block);
                            }
                            lastEnd = i;
                         //    Regex r = new Regex("^注意");
                          //  if (r.IsMatch(b.Lines.ElementAt(i).Trim()))
                          //  {
                             block = newBlock(b.StartLineNum + i, b.StartLineNum + i + 1, Block.BLOCK_TYPE.keyline, paper);
                             smallBlock.Add(block);
                             lastEnd = i+1;
                          //  }
                            
                        }

                    }
                    if (lastEnd < b.Lines.Count  )
                    {
                        block = newBlock(b.StartLineNum + lastEnd, b.StartLineNum+b.Lines.Count, Block.BLOCK_TYPE.unknown, paper);
                        smallBlock.Add(block);
                    }
                }
                else
                {
                    smallBlock.Add(b);
                }
            }
            return smallBlock;
        }
        private Block newBlock(int start, int end,  Block.BLOCK_TYPE type, List<String> paper)
        {
            if (type == Block.BLOCK_TYPE.unknown && isStartWithKeywords(paper.ElementAt(start)))
                type = Block.BLOCK_TYPE.keyline;

            Block block = new Block();
            block.StartLineNum = start;
            block.EndLineNum = end;
            block.Type = type;
            copyContent(paper, block);
            return block;
        }
        private Block newBlock(  Block.BLOCK_TYPE type,  String  content,Block questionblock)
        {
            Block block = new Block();
            block.StartLineNum = -1;
            block.EndLineNum = -1;
            block.Type = type;
            block.Lines.Add (content );
            block.Lines.AddRange(questionblock.Lines);
            block.Seqs = new List<int>(questionblock.Seqs);
            return block;
        }
        private Block newBlock(Block.BLOCK_TYPE type, String content   )
        {
            Block block = new Block();
            block.StartLineNum = -1;
            block.EndLineNum = -1;
            block.Type = type;
            block.Lines.Add(content);
            return block;
        }
        private bool isStartWithKeywords(String line)
        {
            for (int i = 0; i < keywords.Length; i++)
            {
                Regex r = new Regex(keywords[i]);
                if (r.IsMatch(line.Trim()))
                    return true;
            }
            return false; 
        } 
        private void printBlocks(List<Block> blocks,String outPath)
        {
            StreamWriter writer = new StreamWriter(outPath, false);
            Block.BLOCK_TYPE type = Block.BLOCK_TYPE.unknown;
            Boolean stop = false;
            foreach (Block b in blocks)
            {

                if (b.Type == Block.BLOCK_TYPE.keyline)
                {
                    //listening
                    if (isblockContainsPattern(b, listeningpattern))
                    {
                        if (type != Block.BLOCK_TYPE.listening)
                            writer.Write("听力：\n");
                        type = Block.BLOCK_TYPE.listening;
                    }
                    else if (isblockContainsPattern(b, readingpattern))
                    {
                        if (type != Block.BLOCK_TYPE.reading)
                            writer.Write("阅读理解：\n");
                        type = Block.BLOCK_TYPE.reading;
                    }
                    else if (isblockContainsPattern(b, clozepattern))
                    {
                        if (type != Block.BLOCK_TYPE.closen)
                            writer.Write("完型填空：\n");
                        type = Block.BLOCK_TYPE.closen;
                    }
                    else if (isblockContainsPattern(b, correctionpattern))
                    {
                        if (type != Block.BLOCK_TYPE.correction)
                            writer.Write("短文改错：\n");
                        type = Block.BLOCK_TYPE.correction;
                    }
                    else if (isblockContainsPattern(b, translationpattern))
                    {
                        if (type != Block.BLOCK_TYPE.translation)
                            writer.Write("翻译：\n");
                        type = Block.BLOCK_TYPE.translation;
                    }

                    else if (isblockContainsPattern(b, selectionpattern))
                    {
                        if (type != Block.BLOCK_TYPE.selection)
                            writer.Write("单选题：\n");
                        type = Block.BLOCK_TYPE.selection;
                    }
                    else if (isblockContainsPattern(b, writingpattern))
                    {
                        if (type != Block.BLOCK_TYPE.writing)
                            writer.Write("写作：\n");
                        type = Block.BLOCK_TYPE.writing;
                    }
                    else if (isblockContainsPattern(b, answerpattern))
                    {
                        break;
                    }
                }
                else if (b.Type == Block.BLOCK_TYPE.example)
                {
                    continue;
                }else
                {
                    String tmp = "";
                    /*foreach (int i in b.Seqs)
                    {
                        tmp += i.ToString() + ",";
                    }
                    writer.Write(tmp + "\t" + b.Type.ToString() + "\n");*/
                    foreach (String s in b.Lines)
                    {
                        writer.Write(s + "\n");
                    }
                    if (b.Answers.Count > 0)
                    {
                        tmp = "";
                        writer.Write("答案：\t");
                        for (int i = 0; i < b.Answers.Count; i++)
                        {
                            writer.Write(b.Seqs[i] + "." + b.Answers[i] + "\t");
                        }
                    }
                    writer.Write("\n\n");
                }
            }
            writer.Flush();
            writer.Close();
        }
        private int findSelectionEnd(List<String> paper, int pos)
        {
             Regex r = new Regex("^[0-9]+");
            for (int i = pos; i < paper.Count; i++)
            {
                bool isEnd = true;
                for(int k=0;k<selections.Length;k++)
                    if(paper.ElementAt(i).Trim().StartsWith(selections[k])){
                        isEnd = false;
                        break;
                    }
              //  if(strContainsPattern(paper.ElementAt(i).Trim(),selections,true))
                //    isEnd = false;
                if (i == pos && !r.IsMatch(paper.ElementAt(i)) &&
                    !strContainsPattern(paper.ElementAt(pos-1),selections,false))
                {
                    isEnd = false;
                }
                if (isEnd)
                    return i;
            }
            return paper.Count;
        }
        private bool strContainsPattern(String line, String[] pattern, bool startwith)
        {
            foreach (String p in pattern)
            {
                String tmp=p;
                if (p.Contains(")"))
                    tmp = p.Replace(")", "\\)");
                if (p.Contains("."))
                    tmp = p.Replace(".", "\\.");
                if (p.Contains("["))
                    tmp = p.Replace("[", "\\[");
                if (tmp.Contains("]"))
                    tmp = tmp.Replace("]", "\\]");
                Regex r;
                if (startwith)
                    r = new Regex("^" + tmp);
                else
                    r = new Regex(tmp);
                if (r.IsMatch(line))
                    return true;
            }
            return false;
        }
        private Boolean isSelectionStart(List<String> paper, int pos)
        {
            int count = 0;
            Regex r = new Regex("^[0-9]+[\\.．]");
            if(r.IsMatch(paper.ElementAt(pos))){
                for (int i = 0; i < selections.Length; i++)
                    if (paper.ElementAt(pos).Contains(selections[i]))
                        count++;
                if (count > 1)
                    return true;
                if (paper.Count > pos + 1)
                {
                    for (int i = 0; i < selections.Length; i++)
                        if (paper.ElementAt(pos + 1).Trim().StartsWith(selections[i]))
                            return true;
                }
                if (paper.Count > pos + 2)
                {
                    for (int i = 0; i < selections.Length; i++)
                        if (paper.ElementAt(pos + 2).Trim().StartsWith(selections[i]))
                            return true;
                }
            }
            
            return false;
            
        }
        private void copyContent(List<String> paper, Block block)
        {
            for (int i = block.StartLineNum; i < block.EndLineNum; i++)
            {
                block.Lines.Add(paper.ElementAt(i));
            }
        }
        public List<String> getPaper(String path)
        {
            List<String> list = new  List<String>();
            try
            {
                StreamReader reader = new StreamReader(path, Encoding.Default);
                string line = "";
                line = reader.ReadLine();
                while (line != null)
                {
                    line = line.Trim();
                    if(!line.Equals(""))
                        list.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return list;
        }
    }
}
