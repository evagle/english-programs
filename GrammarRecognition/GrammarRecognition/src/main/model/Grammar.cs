using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarRecognition.src.main.model
{
    class Grammar
    {
        public static int T_GRAMMAR = 1;
        public static int T_PHRASE = 2;
        private String text;
        private String name;
        private String abbreviation;
        private String[] pattern;
        private String[] patternLowerCase;


        private int type;


        private int seq;
        public int frequency = 0;
        public Grammar() {
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        public Grammar(Grammar g) 
        {
            this.text = g.Text;
            this.name = g.Name;
            this.abbreviation = g.Abbreviation;
            this.type = g.type;
            this.Seq = g.Seq;
            this.pattern = g.Pattern;
        }
        public Grammar subGrammar(int start)
        {
            Grammar g = new Grammar();
            g.Pattern = new string[this.pattern.Length - start];
            g.PatternLowerCase = new string[this.pattern.Length - start];
            for (int i = start; i < this.pattern.Length; i++ )
            {
                g.Pattern[i - start] = this.pattern[i];
                g.PatternLowerCase[i - start] = this.PatternLowerCase[i];
            }
            return g;
        }
        public String Text
        {
            get { return text; }
            set { text = value; }
        }
        public String[] Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        public String Abbreviation
        {
            get { return abbreviation; }
            set { abbreviation = value; }
        }
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }
        public String ToString()
        {
            return text;
        }

        public String[] PatternLowerCase
        {
            get { return patternLowerCase; }
            set { patternLowerCase = value; }
        }
    }
}
