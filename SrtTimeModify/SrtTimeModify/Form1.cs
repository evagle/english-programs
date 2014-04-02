using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SrtTimeModify.src;

namespace SrtTimeModify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.textBox1.Text = openFileDialog.FileName;
               
            }
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

      

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.timeStretched.Text == null || this.timeStretched.Text.Equals(""))
            {
                MessageBox.Show("需要先填写间隔时间");
                return;
            }

            if (this.textBox2.Text == null || this.textBox2.Text.Equals(""))
            {
                MessageBox.Show("需要先选择文件");
                return;
            }

            String fullPath = this.textBox2.Text ;
            string path = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            string name = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
            List<string> startEndTime = new List<string>();
            StretchTime stretch = new StretchTime(FileHandler.read(fullPath), Convert.ToInt32(this.timeStretched.Text),checkBox1.Checked);
            List<String> list = stretch.stretch(0);
            FileHandler.writeStartTimeEndTime(path + "\\开始时间-结束时间-" + name, stretch.startTimeEndTime);
           
            FileHandler.write(path + "\\改好时间-" + name, list);
            MessageBox.Show("已经输出到：" + path + "\\改好时间-" + name);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.textBox3.Text = openFileDialog.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.textBox4.Text == null || this.textBox4.Text.Equals(""))
            {
                MessageBox.Show("需要先填写时间延长几秒");
                return;
            }
            if (this.textBox3.Text == null || this.textBox3.Text.Equals(""))
            {
                MessageBox.Show("需要先选择文件");
                return;
            }

            SplitToParagraphs merge = new SplitToParagraphs();
            
            String fullPath = this.textBox3.Text;
            string path = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            string name = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
           
            List<string> list = merge.split(FileHandler.read(fullPath));
            StretchTime stretch = new StretchTime(list, Convert.ToInt32(this.textBox4.Text), checkBox2.Checked);
            list = stretch.stretch(Convert.ToInt32(this.textBoxParagraphSpan2.Text));
            FileHandler.writeStartTimeEndTime(path + "\\开始时间-结束时间-" + name, stretch.startTimeEndTime);
            FileHandler.write(path + "\\改好时间-" + name, list);
            MessageBox.Show("已经输出到：" + path + "\\改好时间-" + name);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            String text = this.textBox1.Text;
            string path = text.Substring(0, text.LastIndexOf("\\"));
            string name = text.Substring(text.LastIndexOf("\\") + 1);

            SplitToParagraphs merge = new SplitToParagraphs();
            List<string> list = merge.split(FileHandler.read(text));
            int secondBetweenParagraphs = int.Parse(this.textBoxParagraphSpan1.Text);

            StretchTime stretch = new StretchTime(list,0,false);
            stretch.articleToParagraphBlocks();
            stretch.mergeBlockByTimeSpanBetweenParagraphs(secondBetweenParagraphs);


            FileHandler.write(path + "\\分好段-" + name, stretch.getResultArticle());
            MessageBox.Show("已经输出到：" + path + "\\分好段-" + name);
        }
 
 
    }
}
