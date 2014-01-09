using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrammarRecognition.src.main.model
{
    class Grammar
    {
        private String text;
        private String name;
        private String abbreviation;
        private String[] pattern;
        private int seq;
        public Grammar() {
        }
        public Grammar(Grammar g) {
            this.text = g.Text;
            this.name = g.Name;
            this.abbreviation = g.Abbreviation;
          
            this.Seq = g.Seq;
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
    }
}
