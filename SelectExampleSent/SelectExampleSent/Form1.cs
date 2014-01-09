using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace SelectExampleSent
{
    public partial class Form1 : Form
    {
        Hashtable freqTable=new Hashtable();

        public Form1()
        {
            InitializeComponent();
            initFreqTable();
        }
        private Hashtable initFreqTable( )
        {
            Hashtable tmptable = new Hashtable();
            try
            {
                StreamReader objReader = new StreamReader("最大频率表(带数).txt", Encoding.Default);
                string sLine = "";
                
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine == null)
                        break;
                    sLine = sLine.ToLower();
                    if (!sLine.Equals(""))
                    {
                        
                        string[] fields = sLine.Split(new char[]{' ','\t'},StringSplitOptions.RemoveEmptyEntries );
                        if (fields.Length == 2)
                        {
                            tmptable.Add(fields[0], fields[1]);
                        }
                    }
                }
                objReader.Close();
                sLine = "";
                objReader = new StreamReader("关联词表20130118.txt", Encoding.Default);
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine == null)
                        break;
                    sLine = sLine.ToLower();
                    if (!sLine.Equals(""))
                    {
                        string freqence="";
                        string[] fields = sLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string f in fields)
                        {
                            if (tmptable.Contains(f)){
                                freqence =  (string)tmptable[f] ;
                                break;
                            }
                        }
                        if (!freqence.Equals(""))
                        {
                            foreach (string f in fields)
                            {
                                if(!tmptable.ContainsKey(f))
                                    tmptable.Add(f, freqence);
                            }
                        }
                    }
                }
                objReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return tmptable;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
