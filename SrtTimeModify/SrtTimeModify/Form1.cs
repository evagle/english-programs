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
                string  path = openFileDialog.FileName.Substring(0,openFileDialog.FileName.LastIndexOf("\\"));
                string name = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\")+1);

                MergeBlankLine merge = new MergeBlankLine();
                List<string> list= merge.merge(FileHandler.read(openFileDialog.FileName));

                FileHandler.write(path + "\\分好段-" + name, list);
                MessageBox.Show("已经输出到：" + path + "\\分好段-" + name);
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
            StretchTime stretch = new StretchTime();
            List<string> list = stretch.stretch(FileHandler.read(fullPath), Convert.ToInt32(this.timeStretched.Text), out startEndTime);
            FileHandler.writeStartTimeEndTime(path + "\\开始时间-结束时间-" + name, startEndTime);
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

            MergeBlankLine merge = new MergeBlankLine();
            StretchTime stretch = new StretchTime();
            String fullPath = this.textBox3.Text;
            string path = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            string name = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
            List<string> startEndTime = new List<string>();
            List<string> list = merge.merge(FileHandler.read(fullPath));
            list = stretch.stretch(list, Convert.ToInt32(this.textBox4.Text), out startEndTime);
            FileHandler.writeStartTimeEndTime(path + "\\开始时间-结束时间-" + name, startEndTime);
            FileHandler.write(path + "\\改好时间-" + name, list);
            MessageBox.Show("已经输出到：" + path + "\\改好时间-" + name);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
 
 
    }
}
