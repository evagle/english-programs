using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArticleRecognize.src.main;
using System.IO;

namespace ArticleRecognize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Judge judge = new Judge();

            tbThreshold.Text = "90";
           // judge.isSimilar(s1,s2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            double threshold = -1;
            try
            {
                threshold = Double.Parse(tbThreshold.Text);
                if (threshold < 0 || threshold > 100)
                {
                    MessageBox.Show("相似度必须在0-100,请重新设置");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("相似度必须在0-100,请重新设置");
                return;
            }

            if (result == DialogResult.OK) // Test result.
            {
                this.textBox1.Text = openFileDialog.FileName;
                Judge judge = new Judge();
                judge.THRESHOLD = threshold/100;
                List<String> remained  = judge.cleanSelf(openFileDialog.FileName );
                String outpath = openFileDialog.FileName.Split(new char[] { '.' })[0];
                print(outpath + "_去重后.txt", remained);
                MessageBox.Show(" 已经输出到：\n" + outpath + "_去重后.txt"  );
            }
        }
        private void print(String path, List<String> articles)
        {
            StreamWriter writer = new StreamWriter(path);
            foreach (String str in articles)
            {
                writer.WriteLine(str);
                writer.WriteLine("");
            }
            writer.Flush();
            writer.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.textBox2.Text = openFileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            double threshold = -1;
            try
            {
                threshold = Double.Parse(tbThreshold.Text);
                if (threshold < 0 || threshold > 100)
                {
                    MessageBox.Show("相似度必须在0-100,请重新设置");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("相似度必须在0-100,请重新设置");
                return;
            }
            
            if (result == DialogResult.OK) // Test result.
            {
                this.textBox3.Text = openFileDialog.FileName;
                if (!this.textBox2.Text.Equals(""))
                {
                    Judge judge = new Judge();
                    judge.THRESHOLD = threshold/100;
                    List<String> remained = judge.removeSame(this.textBox3.Text, this.textBox2.Text);
                    String outpath = openFileDialog.FileName.Split(new char[] { '.' })[0];
                    print(outpath + "_去重后.txt", remained);
                    MessageBox.Show(" 已经输出到：\n" + outpath + "_去重后.txt");
                }
            }

        }
        
    }
}
