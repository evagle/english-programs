using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AssociateDict.src
{
    class AssociateModel : IComparable
    {
        public String prototype;
        public HashSet<string> derivates;
        //常规的加s、d、ed、ing结尾的派生词
        public HashSet<string> normalDerivates;
        public HashSet<string> otherDerivates;
        public HashSet<string> otherDerivatesWithoutPrototype;

        public String getOtherDerivatesWithoutPrototype()
        {
            StringBuilder builder = new StringBuilder();

            foreach (String s in otherDerivatesWithoutPrototype)
            {
                builder.Append(s).Append(" ");
            }
            return builder.ToString().TrimEnd();
        }
        public String getDerivates()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.prototype + " ");
            foreach (String s in derivates)
            {
                builder.Append(s).Append(" ");
            }
            return builder.ToString().TrimEnd();
        }
        public String getNormalDerivates()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.prototype + " ");
            foreach (String s in normalDerivates)
            {
                builder.Append(s).Append(" ");
            }
            return builder.ToString().TrimEnd();
        }
        public String getOtherDerivates()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.prototype + " ");
            foreach (String s in otherDerivates)
            {
                builder.Append(s).Append(" ");
            }
            return builder.ToString().TrimEnd();
        }

        public AssociateModel(String proto, String[] words)
        {
            this.prototype = proto;
            derivates = new HashSet<string>();
            normalDerivates = new HashSet<string>();
            otherDerivates = new HashSet<string>();
            otherDerivatesWithoutPrototype = new HashSet<string>();
            setDerivatives(words);
        }
        public void setDerivatives(String[] words)
        {
            foreach (String w in words)
            {
                if(!w.Equals(this.prototype))
                    addDerivative(w);              
            }
        }
        
        public void addDerivative(String w)
        {
            w = w.Trim();

            if (w != null && !w.Equals("")  )
            {
                derivates.Add(w);
                if (w.Equals(prototype + "s") ||
                    w.Equals(prototype + "d") ||
                    w.Equals(prototype + "ed") ||
                    w.Equals(prototype + "ing"))
                {
                    normalDerivates.Add(w);
                }
                else
                {
                    otherDerivates.Add(w);
                }
            }
        }
        /************************************************************************/
        /* 将otherDerivates中符合s,ed,ing,d等正常后缀的词挪到normalDerivates       
         * 例如Aback back backed backer backers backing backless backs backside backsides Backup
           本来词根是back，所以backer backers都放在了otherDerivates里面
         * 现在将backer backers移动到normalDerivate里面，在otherDeriviates里面只保留backer
         */
        /************************************************************************/
        public void findNoramlAndMove()
        {
            List<String> list = new List<String>(otherDerivates);
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Equals(list[i] + "s") ||
                        list[j].Equals(list[i] + "d") ||
                        list[j].Equals(list[i] + "ed") ||
                        list[j].Equals(list[i] + "ing"))
                    {
                        normalDerivates.Add(list[j]);
                        normalDerivates.Add(list[i]);
                        otherDerivates.Remove(list[j]);
                    }
                }
            }
        }
        /************************************************************************/
        /* 将normalDerivates中的每个单词，判断是不是原型，是就加
         * s,d,ed,ing进行扩展，并且将扩展也加入到normalDerivatives中
         * 然后扩展后需要从表2中删除
         */
        /************************************************************************/
        public void expandNormalDerivates()
        {
            List<String> list = new List<String>(normalDerivates);
            list.Add(prototype);
            foreach (String w in list)
            {
                if (w.EndsWith("ss"))
                {
                    normalDerivates.Add(w + "es");
                }
                else
                {
                    if(!w.EndsWith("s")&&
                       !w.EndsWith("d")&&
                        !w.EndsWith("ed")&&
                        !w.EndsWith("ing")){

                            normalDerivates.Add(w + "s");
                            normalDerivates.Add(w + "d");
                        // 如果以e结尾，则不能再加ed，ing形式也要去e加ing
                            if (!w.EndsWith("e"))
                            {
                                normalDerivates.Add(w + "ed");
                                normalDerivates.Add(w + "ing");
                            }
                            else
                            {
                                normalDerivates.Add(w.Substring(0,w.Length-1) + "ing");
                            }

                    }

                }

            }
            foreach (String w in normalDerivates)
            {
                otherDerivates.Remove(w);
            }
        }

        /************************************************************************/
        /* 将扩展后的normalDerivative和otherDerivate合并生成总表                                                                     */
        /************************************************************************/
        public void mergeDerivates()
        {
            foreach (string w in normalDerivates)
            {
                derivates.Add(w);
            }

        }
        /************************************************************************/
        /* 删除表2中的原形，只要在表一出现过又在表2出现过的就是原形                                                                     */
        /************************************************************************/
        public void mergeDerivates2()
        {
            foreach (string w in otherDerivates)
            {
                if (!normalDerivates.Contains(w))
                {
                    otherDerivatesWithoutPrototype.Add(w);
                    otherDerivatesWithoutPrototype.Add(getPluralForm(w));
                }
            }
        }
        /************************************************************************/
        /* 输入单词，得到其复数形式，先简单做，即：
         * 1.直接加s 
         * 2.e结尾直接加s
         * 3.ch结尾直接加es
         * 4.y结尾去e加ies
         */
        /************************************************************************/
        public string getPluralForm(String word)
        {
            if (word.EndsWith("y"))
            {
                return word.Substring(0, word.Length - 1) + "ies";
            }
            else if (word.EndsWith("ch") ||
                word.EndsWith("sh") ||
                word.EndsWith("s") ||
                word.EndsWith("x"))
            {
                return word + "es";
            }
            else if (word.EndsWith("f"))
            {
                return word.Substring(0, word.Length - 1) + "ves";
            }
            else if (word.EndsWith("fe"))
            {
                return word.Substring(0, word.Length - 2) + "ves";
            }else
                return word + "s";
        }
        #region IComparable 成员

        public int CompareTo(object obj)
        {
            AssociateModel model = (AssociateModel)obj;
            return this.prototype.CompareTo(model.prototype);
        }

        #endregion
    }
}
