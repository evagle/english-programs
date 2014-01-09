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
        private String filename;
        private String[] selections = new String[] {
            "A.","B.","C.","D.","A)","B)","C)","D)",
            "[A]","[B]","[C]","[D]","A．","B．","C．","D．",
            "A:","B:","C:","D:", "A：","B：","C：","D：",
            "A,","B,","C,","D,",
        };
        private String[] keywords = new String[] { "^Part", "^Section", "^Directions", "^注意",
            "听第.*段对话", "完形填空", "^阅读下面短文", "阅读理解",
            "短文改错", "书面表达" ,"单项填空","听","答","英语",
            "画","第一","第二","第三","划","第四","第六","^Questions [0-9]* to","^Passage",
        "^Text[ \t]*[0-9]","[ \t0-9]*Directions","Choose the best.*for each numbered blank"
        ,"Read the following.*text","choose the most suitable paragraphs from the list"
        ,"部分","are based on the following","Cloze","完型","完形","Directions.*There are.*blanks",
            "Read .*the.*following.*text.*Choose.*the best.*word",
         "Listening[ \t]*Comprehension", 
            "Directions.*hear.*conversations","对话","独白",
            "Directions.*hear.*passage",
          "Reading[ \t]*Comprehension", 
            "choose the most suitable paragraphs from the list",
            "改错","Translation","翻译","参考答案",
        "单项填空","Vocabulary[ \t]*and[ \t]*Structure",
            "Directions.*There are.*incomplete sentences",
         "书面表达","Writing", "^例","^[Ee]xample","best fits the context",
            };
        private String[] listeningpattern = new String[] {
            "Listening[ \t]*Comprehension","听力",
            "Directions.*hear.*conversations","对话","独白",
            "Directions.*hear.*passage"
        };
        private String[] readingpattern = new String[]{
            "Reading[ \t]*Comprehension","阅读理解","语篇理解","理解",
            "choose the most suitable paragraphs from the list",
        };
        private String[] clozepattern = new String[]{
            "Cloze","完型","完形","Directions.*There are.*blanks",
            "Read .*the.*following.*text.*Choose.*the best.*word",
            "综合填空","Fill in each blank.*word or phrase.*fits the context"
        };
        private String[] correctionpattern = new String[]{
            "改错"
        };
        private String[] translationpattern = new String[] {
            "Translation","翻译"
        };
        private String[] answerpattern = new String[]{
            "参考答案" ,"试卷答案","英语答案","标准答案"
        };
        private String[] selectionpattern = new String[]{
            "单项","Vocabulary[ \t]*and[ \t]*Structure",
            "Directions.*There are.*incomplete sentences","Grammar"
            ,"Vocabulary","completes.*sentence","Language Knowledge"
        };
        private String[] writingpattern = new String[] {
            "书面表达","Writing"
        };
        private String[] examplepattern = new String[]{
            "^例","^[Ee]xample"
        };
       
        
        public void process(String path,string outpath)
        {
            String[] tmp  = outpath.Split(new char[]{'\\'});
            filename = tmp[tmp.Length-1];
            //path = "D:\\Download\\试卷\\cet4-1989.txt";
            List<String> paper = getPaper(path);
            List<Block> blocks = cutBySelection(paper);
            List<Block> finalBlocks=null;
            Hashtable optionTable=null;
            Hashtable answertable = null;
            try
            {
                blocks = findKeyLineBlock(blocks, paper);
            }
            catch (Exception e) { Console.WriteLine("findKeyLineBlock:" + e.Message); }
            try
            {
                setRightBlockTag(blocks);
            }
            catch (Exception e) { Console.WriteLine("setRightBlockTag:"+e.Message); }
            try
            {
                removeAttentionBlock(blocks);
            }
            catch (Exception e) { Console.WriteLine("removeAttentionBlock:" + e.Message); }
            try
            {
                finalBlocks = getFinalBlockList(blocks);
            }
            catch (Exception e) { Console.WriteLine("getFinalBlockList:" + e.Message); }
            try
            {
                mergeReading(finalBlocks);
                finalBlocks=mergeReadingAndCloze(finalBlocks);
            }
            catch (Exception e) { Console.WriteLine("mergeReading:"+e.Message); }
            try
            {
                addSeqToBlock(finalBlocks, true);
            }
            catch (Exception e) { Console.WriteLine("addSeqToBlock:" + e.Message); }
            try
            {
                answertable = getAnswer(finalBlocks);
            }
            catch (Exception e) { Console.WriteLine("getAnswer:"+e.Message); }
            try
            {
                matchAnswer(finalBlocks, answertable);
            }
            catch (Exception e) { Console.WriteLine("matchAnswer:"+e.Message); }
            try
            {
                optionTable = getOptionTable(finalBlocks, answertable);
            }
            catch (Exception e) { Console.WriteLine("getOptionTable:"+e.Message); }
            try
            {
                //addSeqToBlock(blocks, true);
            }
            catch (Exception e) { Console.WriteLine("addSeqToBlock:"+e.Message); }
            try
            {
                //fillWithAnswer(blocks, optionTable, answertable);
                fillWithAnswer(finalBlocks, optionTable, answertable);
            }
            catch (Exception e) { Console.WriteLine("fillWithAnswer:"+e.Message); }
            try
            {
                printPaperFilledWithAnswer(finalBlocks, outpath + "_填入答案后的试卷.txt");
                printBlocks(finalBlocks, outpath + "_split.txt",false);
                printBlocks(finalBlocks, outpath + "_切分并填答案.txt", true);
            }
            catch (Exception e) { Console.WriteLine("printPaperFilledWithAnswer,printBlocks:" + e.Message); }

        }
          
        private List<int> getSeqList(Block b)
        {
            if (b.Seqs.Count == 0)
                return new List<int>();
            List<int> seqlist = new List<int>();
            seqlist.AddRange(b.Seqs);
            String text = b.getString();
            for (int i = b.Seqs[0] - 5; i < b.Seqs[0] + 5; i++)
            {
                if (text.Contains(i.ToString()) && !seqlist.Contains(i))
                {
                    seqlist.Add(i);
                }
            }
            seqlist.Sort();
            return seqlist;
        } 
        private void fillWithAnswer(List<Block> blocks, Hashtable optionTable,Hashtable answertable)
        {
            
            foreach (Block b in blocks)
            {
                
                if ((b.Type == Block.BLOCK_TYPE.selection || b.Type == Block.BLOCK_TYPE.selection_listening) 
                     && b.Answers.Count > 0 && optionTable.ContainsKey(b.Seqs[0]))
                {
                    Regex r = new Regex(@"[ _]{4,}");
                    b.TextFilledWithAnswer = r.Replace(b.getQuestionString(), " " + (String)((Hashtable)optionTable[b.Seqs[0]])[b.Answers[0].Trim()] + " ")+b.getOptionString();
                    b.TextFilledWithAnswer = r.Replace(b.TextFilledWithAnswer, " ");
                    //b.QuestionStringFilledWithAnswer = r.Replace(b.getOptionString(), " " + (String)((Hashtable)optionTable[b.Seqs[0]])[b.Answers[0].Trim()] + " ");
                }
                else if (b.Type == Block.BLOCK_TYPE.closen || b.Type == Block.BLOCK_TYPE.selection_cloze )
                {
                    List<int> seqlist = getSeqList(b);

                    foreach (int seq in seqlist)
                    {
                        if (answertable.ContainsKey(seq) &&  optionTable.ContainsKey(seq) )
                        {

                            if (b.TextFilledWithAnswer == null)
                            {
                                b.TextFilledWithAnswer = myReplace(b.getString(), seq.ToString(), " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                                b.QuestionStringFilledWithAnswer = myReplace(b.getQuestionString(), seq.ToString(), " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                            }
                            else
                            {
                                b.TextFilledWithAnswer = myReplace(b.TextFilledWithAnswer, seq.ToString(), " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                                b.QuestionStringFilledWithAnswer = myReplace(b.QuestionStringFilledWithAnswer, seq.ToString(), " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                            }
                            
                        }
                    }
                    Regex rx = new Regex("[_()（）]+");
                    if (b.TextFilledWithAnswer!=null)
                        b.TextFilledWithAnswer = rx.Replace(b.TextFilledWithAnswer, " ");
                    if (b.QuestionStringFilledWithAnswer!=null)
                        b.QuestionStringFilledWithAnswer = rx.Replace(b.QuestionStringFilledWithAnswer, " ");  
                }else if( b.Type == Block.BLOCK_TYPE.reading){
                    List<int> seqlist = b.Seqs;
                    seqlist.Sort();
                    for (int i = 0; i < seqlist.Count;i++ )
                    {
                        int seq = seqlist[i];
                        if (answertable.ContainsKey(seq) && optionTable.ContainsKey(seq))
                        {

                            if (b.TextFilledWithAnswer == null)
                            {
                                b.TextFilledWithAnswer = readingReplace(seq,b.getString(), " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                                //b.QuestionStringFilledWithAnswer = readingReplace(b.getQuestionString(), " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                            }
                            else
                            {
                                b.TextFilledWithAnswer = readingReplace(seq, b.TextFilledWithAnswer, " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                                //b.QuestionStringFilledWithAnswer = readingReplace(b.QuestionStringFilledWithAnswer, " " + (String)((Hashtable)optionTable[seq])[((String)answertable[seq]).Trim()] + " ");
                            }

                        }
                    }
                    Regex rx = new Regex("[_()（）]+");
                    if (b.TextFilledWithAnswer!=null)
                        b.TextFilledWithAnswer = rx.Replace(b.TextFilledWithAnswer, " ");
                     
                }
                
                    //Console.WriteLine(b.TextFilledWithAnswer);
             }
       }

        private String readingReplace(int seq, String str, String replacewith)
        {
            int seqPos = str.LastIndexOf(seq.ToString());
            if (seqPos < 0)
                return "";
            int index = str.IndexOf("__", seqPos);
            if (index < 0)
                index = str.IndexOf("    ", seqPos);

            int seqEndPos = str.LastIndexOf((seq+1).ToString());
            if (seqEndPos <= 0)
                seqEndPos = str.Length;
            if (index >= 0 && seqPos >= 0&& index <seqEndPos)
            {
                int end = index;
                while (end < str.Length && (str[end] == '_' || str[end] == ' '))
                {
                    end++;
                }
                int len = end - index;
                return str.Substring(0, index) + replacewith + str.Substring(index + len);
            }
            else
                return str; 
        }
        private String myReplace(String str, string toreplaced,String replacewith)
        {
            int index = str.IndexOf("_"+toreplaced);
         
            int additionLen = 1;
            if (index < 0)
                index = str.IndexOf( toreplaced + "_");
            if (index < 0)
            {
                index = str.IndexOf(" " + toreplaced + " ");
                additionLen = 2;
            }
            if (index < 0)
            {
                index = str.IndexOf(toreplaced);
                additionLen = 0;
            }
            if (index >= 0)
            {
                return str.Substring(0, index) + replacewith + str.Substring(index + toreplaced.Length + additionLen);
            }
            else
                return str; 
        }
        private Hashtable getOptionTable(List<Block> finalBlocks, Hashtable anstable)
        {
            Hashtable optionTable = new Hashtable();

            foreach (Block b in finalBlocks)
            {
                if (b.Type == Block.BLOCK_TYPE.selection 
                    || b.Type == Block.BLOCK_TYPE.closen||
                    b.Type == Block.BLOCK_TYPE.selection_listening||
                    b.Type == Block.BLOCK_TYPE.selection_cloze||
                    b.Type == Block.BLOCK_TYPE.selection_reading
                    || b.Type == Block.BLOCK_TYPE.reading)
                {
                    if (b.Seqs.Count ==0)
                    {
                        continue;
                    }
                    if (b.Seqs[0] == 36)
                    {
                        int f = 23;
                        int k = f;
                    }
                    
                    try
                    {
                        Regex r = new Regex("[.:]");
                        String[] splitedArray;
                        b.Seqs.Sort();
                        int end = b.Lines.Count;
                        int findNum = 0;
                        for (int i = b.Seqs.Count-1; i >=0 ; i--)
                        {
                            int k = end - 1;
                            while (k >= 0 && !b.Lines[k].Trim().StartsWith(b.Seqs[i].ToString()))
                            {
                                k--;                              
                            }
                            if (k < 0) continue;
                            String str = "";
                            for (int j = k; j < end; j++)
                            {
                                if (i + 1 == b.Seqs.Count || (i + 1 < b.Seqs.Count && !b.Lines[j].StartsWith(b.Seqs[i + 1].ToString())))
                                    str += b.Lines[j];
                                else
                                    break;
                            }
                            splitedArray = str.Split(selections, StringSplitOptions.RemoveEmptyEntries);
                            if (splitedArray.Length != 5)
                            {
                                splitedArray = str.Split(new String[] { "A", "B", "C", "D" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            if (splitedArray.Length == 5)
                            {
                                Hashtable tmp = new Hashtable();
                                tmp.Add("A", r.Replace(splitedArray[1], "").Trim());
                                tmp.Add("B", r.Replace(splitedArray[2], "").Trim());
                                tmp.Add("C", r.Replace(splitedArray[3], "").Trim());
                                tmp.Add("D", r.Replace(splitedArray[4], "").Trim());
                                if (!optionTable.ContainsKey(b.Seqs[i]))
                                    optionTable.Add(b.Seqs[i], tmp);
                                findNum++;
                                end = k;
                            }
                           

                        }
                        if (findNum != b.Seqs.Count)
                        {

                            splitedArray = b.getString().Split(selections, StringSplitOptions.RemoveEmptyEntries);
                            if (splitedArray.Length != 5)
                            {
                                try
                                {
                                    String str = b.getString().Substring(b.getString().IndexOf(b.Seqs[0].ToString()));
                                    splitedArray = str.Split(new String[] { "A", "B", "C", "D" }, StringSplitOptions.RemoveEmptyEntries);
                                }
                                catch (System.Exception ex)
                                {
                                    Console.WriteLine("getFinalBlockList:" + ex.Message);
                                }

                            }


                            if (splitedArray.Length == 5)
                            {
                                b.SelectionAnswerTable = new Hashtable();
                                b.SelectionAnswerTable.Add("A", r.Replace(splitedArray[1], "").Trim());
                                b.SelectionAnswerTable.Add("B", r.Replace(splitedArray[2], "").Trim());
                                b.SelectionAnswerTable.Add("C", r.Replace(splitedArray[3], "").Trim());
                                b.SelectionAnswerTable.Add("D", r.Replace(splitedArray[4], "").Trim());
                                if (!optionTable.ContainsKey(b.Seqs[0]))
                                    optionTable.Add(b.Seqs[0], b.SelectionAnswerTable);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }
            return optionTable;
        }
        private void matchAnswer(List<Block> finalBlocks, Hashtable anstable)
        {
            foreach (Block b in finalBlocks)
            {
                b.Seqs.Sort();
                for (int i = 0; i < b.Seqs.Count; i++)
                {
                    int seq = b.Seqs[i];
                    if (anstable.ContainsKey(seq))
                    {
                        b.Answers.Add((String)anstable[seq]);
                    }
                }
                     
            }
          
                
        }
        private void splitSelections(Block b)
        {
            String[] splitedArray;
            if (b.Type == Block.BLOCK_TYPE.selection || b.Type == Block.BLOCK_TYPE.closen)
            {
                
                //判断问题的位置，连续7个以上空格，或者有下划线夹着对应题号的。
                char[] chars;
                String text = b.getString();
                if(b.TextFilledWithAnswer!=null)
                    text = b.TextFilledWithAnswer ;
                chars = text.ToCharArray();
                int startpos = -1;
                int endpos = -1;
                for (int i = 0; i < chars.Length; i++)
                {
                    while(chars[i]=='_'){
                        startpos = startpos == -1 ? i : startpos;
                        i++;
                    }
                    if (startpos > 0)
                    {
                        int seq = 0;
                        while (chars[i] >= '0' && chars[i] <= '9')
                        {
                            seq = seq * 10 + (chars[i] - '0');
                            i++;
                        }
                        endpos = i;
                        if (seq == b.Seqs[0])
                        {
                            break;
                        }
                        else
                        {
                            startpos = -1;
                            endpos = -1;
                        }
                    }
                }
                if (startpos > 0)
                {
                    while (chars[endpos] == '_')
                        endpos++;
                    if (b.Answers.Count > 0 && b.SelectionAnswerTable.ContainsKey( b.Answers[0].Trim()))
                    {
                        String beReplaced = text.Substring(startpos, endpos - startpos);
                        b.TextFilledWithAnswer = text.Replace(beReplaced, (String)b.SelectionAnswerTable[b.Answers[0].Trim()]);
                    }
                    return;
                }
                startpos = -1;
                endpos = -1;
                for (int i = 0; i < chars.Length; i++)
                {
                    if(chars[i]!=' '){
                        startpos = i;
                        continue;
                    }
                    while (chars[i] == ' ')
                    {
                        i++;
                    }
                    endpos = i;
                    if (endpos - startpos - 1 >= 7)
                    {
                        if (b.Answers.Count > 0 && b.SelectionAnswerTable.ContainsKey(b.Answers[0].Trim()))
                        {
                            String beReplaced = text.Substring(startpos + 1, endpos - startpos - 1);
                            b.TextFilledWithAnswer = text.Replace(beReplaced, " " + (String)b.SelectionAnswerTable[b.Answers[0].Trim()] + " ");
                            //b.TextFilledWithAnswer = b.TextFilledWithAnswer.Substring(0, b.TextFilledWithAnswer.IndexOf("A"));
                            return;
                        }
                    }

                }
                int seqIndex = text.IndexOf(b.Seqs[0].ToString());
                if (seqIndex > 0)
                {
                    startpos = seqIndex - 1;
                    while (startpos > 0 && chars[startpos] == ' ' || chars[startpos] == '_')
                    {
                        startpos--;
                    }
                    startpos++;
                    endpos = seqIndex + 2;
                    while (endpos > 0 && chars[endpos] == ' ' || chars[endpos] == '_')
                    {
                        endpos++;
                    }
                    if (endpos - startpos   >= 7)
                    {
                        if (b.Answers.Count > 0 && b.SelectionAnswerTable.ContainsKey(b.Answers[0].Trim()))
                        {
                            String beReplaced = text.Substring(startpos, endpos - startpos - 1);
                            b.TextFilledWithAnswer = text.Replace(beReplaced, " " + (String)b.SelectionAnswerTable[b.Answers[0].Trim()] + " ");
                            //b.TextFilledWithAnswer = b.TextFilledWithAnswer.Substring(0, b.TextFilledWithAnswer.IndexOf("A"));
                            return;
                        }
                    }
                }
            }
        }
        public Hashtable getAnswer(List<Block> finalBlocks)
        {
            bool answerStarted = false ;
            bool hasreading = false ;
            Hashtable anstable = new Hashtable();
            foreach (Block b in finalBlocks)
            {
                if (b.Type == Block.BLOCK_TYPE.reading || b.Type == Block.BLOCK_TYPE.selection_reading)
                {
                    hasreading = true;
                    continue;
                }
                if (b.Type == Block.BLOCK_TYPE.keyline && b.getString().Contains("参考答案"))
                {
                    answerStarted = true;
                }
                if ((b.Type == Block.BLOCK_TYPE.answer && hasreading ) ||answerStarted)
                {
                     Regex r=null;
                     MatchCollection matches =null;
                    answerStarted = true;
                    foreach(String line in b.Lines){
                          r = new Regex("[0-9]+[.．][^0-9]*");
                          matches = r.Matches(line);
                        foreach (Match match in matches)
                        {
                            foreach (Capture capture in match.Captures)
                            {
                                try
                                {
                                    String value = capture.Value;
                                    int cur = 0;
                                    String ans = "";
                                    getSeq(value, out cur, out ans);
                                    //如果ans中含有A或B或C或D且比较短，说明是选择题答案
                                    ans = ans.Trim();
                                    ans = ans.Replace("_", "");
                                    if (ans.StartsWith("A") && ans.Length < 5)
                                    {
                                        ans = "A";
                                    }
                                    else if (ans.StartsWith("B") && ans.Length < 5)
                                    {
                                        ans = "B";
                                    }
                                    else if (ans.StartsWith("C") && ans.Length < 5)
                                    {
                                        ans = "C";
                                    }
                                    else if (ans.StartsWith("D") && ans.Length < 5)
                                    {
                                        ans = "D";
                                    }
                                    if (!ans.Equals("") && !anstable.ContainsKey(cur))
                                        anstable.Add(cur, ans);
                                }
                                catch (Exception e) { Console.WriteLine(e.StackTrace); }
                       
                            }
                        }
                    }
                    
                    String str = b.getString();

                    r = new Regex("([0-9]{1,3})[^0-9]{0,3}([0-9]{1,3})[^A-Za-z0-9]*([A-K]{3,}[\t ]*[A-K]*[\t ]*[A-K]*[\t ]*[A-K]*)");
                    matches = r.Matches(str);
                    foreach (Match match in matches)
                    {
                        try
                        {
                            int start = Int32.Parse(str.Substring(match.Groups[1].Index, match.Groups[1].Length));
                            int end = Int32.Parse(str.Substring(match.Groups[2].Index, match.Groups[2].Length));
                            String ansStr = str.Substring(match.Groups[3].Index, match.Groups[3].Length);
                            ansStr = ansStr.Replace(" ", "");
                            ansStr = ansStr.Replace("\t", "");
                            for (int k = start; k <= end; k++)
                            {
                                if (!anstable.ContainsKey(k) && ansStr.Length > k - start - 1)
                                    anstable.Add(k, ansStr.Substring(k - start, 1));
                            }
                           
                        }
                        catch (Exception e) { Console.WriteLine(e.StackTrace); }
                       
                       
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
        private List<Block> mergeReadingAndCloze(List<Block> finalBlocks)
        {
            List<Block> blocks = new List<Block>();

            for (int i = 0; i < finalBlocks.Count; )
            {
                if (finalBlocks.ElementAt(i).Type == Block.BLOCK_TYPE.reading ||
                    finalBlocks.ElementAt(i).Type == Block.BLOCK_TYPE.closen)
                {
                    Block newblock = finalBlocks.ElementAt(i);
                    int j = i + 1;
                    for (; j < finalBlocks.Count; j++)
                        if (finalBlocks.ElementAt(j).Type == Block.BLOCK_TYPE.selection ||
                            finalBlocks.ElementAt(j).Type == Block.BLOCK_TYPE.selection_cloze ||
                            (finalBlocks.ElementAt(j).Type == Block.BLOCK_TYPE.keyline&&j==i+1&&
                            finalBlocks.ElementAt(i).Type == Block.BLOCK_TYPE.reading))
                            newblock = mergeBlock(newblock, finalBlocks.ElementAt(j));
                        else
                            break;
                    i = j;
                    blocks.Add(newblock);
                }
                else
                {
                    blocks.Add(finalBlocks.ElementAt(i));
                    i++;
                }
                 
            }
            return blocks;
        }
        
        private Block mergeBlock(Block b1, Block b2)
        {
            Block block = new Block();
            block.Type = b1.Type;
            block.Lines.AddRange(b1.Lines);
            block.Lines.AddRange(b2.Lines);

            block.AddSeqsRange(b1.Seqs);
            block.AddSeqsRange(b2.Seqs);
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
        
        private void addSeqToBlock(List<Block> finalBlocks,bool notSplited)
        {
            int curseq = 1;
            foreach (Block block in finalBlocks)
            {
                if (notSplited && (block.Type == Block.BLOCK_TYPE.closen ||
                    block.Type == Block.BLOCK_TYPE.reading))
                {
                    int tmp = curseq;
                    while (!block.getString().Contains(tmp.ToString()) && tmp - curseq < 3)
                    {
                        tmp++;
                    }
                    if (tmp - curseq < 3)
                    {
                        curseq = tmp;
                        int count = 0;//完型填空20个题
                        while (block.getString().Contains(curseq.ToString()) && count<21)
                        {
                            block.AddSeq(curseq);
                            curseq++;
                            count++;
                        }
                    }
                }
                else if (block.Type == Block.BLOCK_TYPE.selection || block.Type == Block.BLOCK_TYPE.closen ||
                    block.Type == Block.BLOCK_TYPE.selection_reading || block.Type == Block.BLOCK_TYPE.selection_listening
                    || block.Type == Block.BLOCK_TYPE.selection_cloze)
                {

                    int tmp = curseq;
                    while (!block.getString().Contains(tmp.ToString()) && tmp - curseq < 3)
                    {
                        tmp++; 
                    }
                    //if (block.getString().Contains(curseq.ToString()))
                    //{
                    if (tmp - curseq < 3)
                    {
                        curseq = tmp;
                        if (block.Seqs.Count == 0)
                        {
                            block.AddSeq(curseq);
                            curseq++;
                        }
                        else
                            curseq = block.Seqs[0] + 1;
                    }
                    //}
                    
                }
                else if (block.Type == Block.BLOCK_TYPE.listening || block.Type == Block.BLOCK_TYPE.reading ||
                    block.Type == Block.BLOCK_TYPE.correction || block.Type == Block.BLOCK_TYPE.translation)
                {
                    int tmp = 0;
                    while (!block.getString().Contains(curseq.ToString()+".")&&
                        !block.getString().Contains(curseq.ToString() + ":") && tmp < 8) { curseq++; tmp++; }
                    while (block.getString().Contains(curseq.ToString()+".")||
                        block.getString().Contains(curseq.ToString() + ":"))
                    {
                        block.AddSeq(curseq);
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
                /*if (block.Type == Block.BLOCK_TYPE.closen)
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
                }*/
                if ( block.Type == Block.BLOCK_TYPE.correction
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
                if (blocks.ElementAt(k).Type == Block.BLOCK_TYPE.selection||
                    blocks.ElementAt(k).Type == Block.BLOCK_TYPE.selection_cloze&& qStart < 0)
                {
                    qStart = k;
                }
                if (blocks.ElementAt(k).Type != Block.BLOCK_TYPE.selection &&
                    blocks.ElementAt(k).Type != Block.BLOCK_TYPE.selection_cloze && qStart > 0)
                {
                    qEnd = k;
                    break;
                }
            }
            String str = blocks.ElementAt(qStart).Lines.ElementAt(0).Trim();
            int seq=0;
            if(str.ToCharArray()[1]>='0'&&str.ToCharArray()[1]<='9'){
                seq = Int32.Parse(str.Substring(0,2));
            }else
                seq = Int32.Parse(str.Substring(0, 1));
            List<String> list = splitClozeToSentences(blocks.ElementAt(blockPos), qEnd - qStart, seq);
            for (int i = 0; i < list.Count; i++)
            {
                clozeList.Add(newBlock(Block.BLOCK_TYPE.selection_cloze, list.ElementAt(i), blocks.ElementAt(qStart + i)));
            }
           
            for (int i = clozeList.Count - 1; i > 0; i--)
            {
                if(isListEqual(getSeqList(clozeList[i]),getSeqList(clozeList[i-1]))){
                    Block b = mergeClozeBlock(clozeList[i - 1], clozeList[i]);
                    clozeList.Insert(i - 1, b);
                    clozeList.RemoveAt(i );
                    clozeList.RemoveAt(i);
                    
                }
            }
            return qEnd;
        }
        private Block mergeClozeBlock(Block front, Block back)
        {
            Block b = new Block();
            b.Seqs = front.Seqs;
            b.Lines = front.Lines;
            b.SelectionAnswerTable = front.SelectionAnswerTable;
            b.TextFilledWithAnswer = front.TextFilledWithAnswer;
            b.Type = front.Type;
            b.Seqs.AddRange(back.Seqs);

            int start = 0;
            
            for (int i = back.Lines.Count - 1; i >= 0; i--)
            {
                if (back.Lines[i].Trim().StartsWith(back.Seqs[0].ToString()))
                {
                    start = i;
                    break;
                }
            }
            for (int i = start; i < back.Lines.Count; i++)
                b.Lines.Add(back.Lines[i]);
            return b;
        }
        private bool isListEqual(List<int> list1, List<int> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            for (int i = 0; i < list1.Count; i++)
                if (list1[i] != list2[i])
                    return false;
            return true;
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
                    }
                    else if (type == Block.BLOCK_TYPE.closen )
                    {
                        Regex r = new Regex("^[0-9]");
                        if (r.IsMatch(block.getString()) && block.Lines.Count <= 3) { 
                            block.Type = Block.BLOCK_TYPE.selection_cloze;
                        }else
                            block.Type = type;
                    }else
                        block.Type = type;
                }
                else if (block.Type == Block.BLOCK_TYPE.selection)
                {
                    if (type == Block.BLOCK_TYPE.listening)
                        block.Type = Block.BLOCK_TYPE.selection_listening;
                    else if (type == Block.BLOCK_TYPE.closen)
                        block.Type = Block.BLOCK_TYPE.selection_cloze;
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
        private Block newBlock(Block.BLOCK_TYPE type, String content, List<Block> questionblock)
        {
            Block block = new Block();
            block.StartLineNum = -1;
            block.EndLineNum = -1;
            block.Type = type;
            block.Lines.Add(content);
            block.Seqs = new List<int>();
            foreach (Block b in questionblock)
            {
                block.Lines.AddRange(b.Lines);
                block.Seqs.AddRange(b.Seqs);
            }
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
        private void printPaperFilledWithAnswer(List<Block> blocks,String outPath){
            StreamWriter writer = new StreamWriter(outPath, false,Encoding.GetEncoding("gbk"));
            foreach (Block b in blocks)
            {
                if (b.TextFilledWithAnswer != null)
                    writer.Write(b.TextFilledWithAnswer);
                else
                    writer.Write(b.getString());
                if (b.Answers.Count > 0)
                {
                    writer.Write("答案：\t");
                    for (int i = 0; i < b.Answers.Count; i++)
                    {
                        writer.Write(b.Seqs[i] + "." + b.Answers[i] + "\t");
                    }
                    writer.Write("\n");
                }
                writer.WriteLine(filename+"\n");
            }
        }
        private void printBlocks(List<Block> blocks,String outPath,bool isFilledAnswer)
        {
            StreamWriter writer = new StreamWriter(outPath, false, Encoding.GetEncoding("gbk"));
            Block.BLOCK_TYPE type = Block.BLOCK_TYPE.unknown;
            Boolean stop = false;
            Block preBlock = null;
            foreach (Block b in blocks)
            {

                if (b.Type == Block.BLOCK_TYPE.keyline)
                {
                    //listening
                    if (isblockContainsPattern(b, listeningpattern))
                    {
                        if (type != Block.BLOCK_TYPE.listening)
                            writer.Write("听力：\r\n");
                        type = Block.BLOCK_TYPE.listening;
                    }
                    else if (isblockContainsPattern(b, readingpattern))
                    {
                        if (type != Block.BLOCK_TYPE.reading)
                            writer.Write("阅读理解：\r\n");
                        type = Block.BLOCK_TYPE.reading;
                    }
                    else if (isblockContainsPattern(b, clozepattern))
                    {
                        if (type != Block.BLOCK_TYPE.closen)
                            writer.Write("完型填空：\r\n");
                        type = Block.BLOCK_TYPE.closen;
                    }
                    else if (isblockContainsPattern(b, correctionpattern))
                    {
                        if (type != Block.BLOCK_TYPE.correction)
                            writer.Write("短文改错：\r\n");
                        type = Block.BLOCK_TYPE.correction;
                    }
                    else if (isblockContainsPattern(b, translationpattern))
                    {
                        if (type != Block.BLOCK_TYPE.translation)
                            writer.Write("翻译：\r\n");
                        type = Block.BLOCK_TYPE.translation;
                    }

                    else if (isblockContainsPattern(b, selectionpattern))
                    {
                        if (type != Block.BLOCK_TYPE.selection)
                            writer.Write("单选题：\r\n");
                        type = Block.BLOCK_TYPE.selection;
                    }
                    else if (isblockContainsPattern(b, writingpattern))
                    {
                        if (type != Block.BLOCK_TYPE.writing)
                            writer.Write("写作：\r\n");
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
                
                    if (isFilledAnswer && b.TextFilledWithAnswer != null)
                    {
                        writer.Write(b.TextFilledWithAnswer + "\r\n");
                    }
                    else
                    {
                        if (b.Type == Block.BLOCK_TYPE.selection_cloze && preBlock!=null
                            && preBlock.Type == Block.BLOCK_TYPE.selection_cloze)
                        {
                            writer.Write(preBlock.QuestionStringFilledWithAnswer);
                        }
                        foreach (String s in b.Lines)
                        {
                            writer.Write(s + "\r\n");
                        }
                    }
                    if (b.Answers.Count > 0)
                    {
                        tmp = "";
                        writer.Write("答案：\t");
                        for (int i = 0; i < b.Answers.Count; i++)
                        {
                            writer.Write(b.Seqs[i] + "." + b.Answers[i] + "\t");
                        }
                        writer.Write("\r\n");
                        
                    }
                    writer.WriteLine(filename);
                    writer.Write("\r\n");
                }
                preBlock = b;
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
            Regex r = new Regex("^[0-9 \t]+[\\.．:：]");
            if(r.IsMatch(paper.ElementAt(pos).Trim())){
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
                    line = line.TrimStart();
                    line = line.Replace("　", " ");
                    line = line.Replace("＿", "_");
                    line = line.TrimStart(new char[] { '-' });
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
