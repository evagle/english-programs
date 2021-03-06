﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SrtTimeModify.src;
using System.IO;

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
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = dialog.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*this.textBox2.Text = "\\\\VBOXSVR\\ameliawang\\Downloads\\刘实-英语项目\\11-27字幕改动\\字幕1";
            this.tb_A.Text = "3";
            this.textBoxThreshold0.Text = "2";
            */
            if (this.textBox2.Text == null || this.textBox2.Text.Equals(""))
            {
                MessageBox.Show("需要先选择文件夹");
                return;
            }
            if (this.tb_A.Text == null || this.tb_A.Text.Equals(""))
            {
                MessageBox.Show("需要先填写A值");
                return;
            }

           
            listFiles0(this.textBox2.Text);
            MessageBox.Show("时间修改完成");
        }
        public void listFiles0(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                int span1 = (int)(Convert.ToDouble(this.tb_A.Text) * 1000);
               
                fixTime(files[i].FullName, span1,
                    checkBox1.Checked);
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                listFiles(dirs[i].FullName);
            }
        }
        private void fixTime(string fullPath, int span1, bool check)
        {

            string path = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            string name = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
 
            StretchTime stretch = new StretchTime(FileHandler.read(fullPath), span1, check);
        
            stretch.setThresholdA(Convert.ToDouble(this.textBoxThreshold0.Text) * 1000);
            List<string> list = stretch.stretch(0);

            Directory.CreateDirectory(path + "-改好时间\\");
            FileHandler.writeStartTimeEndTime(path + "-改好时间\\开始时间-结束时间-" + name, stretch.startTimeEndTime);
            FileHandler.write(path + "-改好时间\\改好时间-" + name, list);
             

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = dialog.SelectedPath;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /*this.textBox3.Text = "\\\\VBOXSVR\\ameliawang\\Downloads\\刘实-英语项目\\11-27字幕改动\\字幕1";
            this.textBox4.Text = "3";
            this.textBoxThreshold.Text = "2";
            this.textBoxParagraphSpan2.Text = "0.5";
            */
            if (this.textBox4.Text == null || this.textBox4.Text.Equals(""))
            {
                MessageBox.Show("需要先填写a值");
                return;
            }
            if (this.textBox3.Text == null || this.textBox3.Text.Equals(""))
            {
                MessageBox.Show("需要先选择文件夹");
                return;
            }

            listFiles(this.textBox3.Text);
            MessageBox.Show("分段完成");
        }
        public void listFiles(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                int span1 = (int)(Convert.ToDouble(this.textBox4.Text)*1000);
                int span2 = (int)(Convert.ToDouble(this.textBoxParagraphSpan2.Text)*1000);
                splitAndFixTime(files[i].FullName, span1,
                    checkBox2.Checked, span2);
            }
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                listFiles(dirs[i].FullName);
            }
        }
        private void splitAndFixTime(string fullPath, int span1, bool check,int span2)
        {

            SplitToParagraphs merge = new SplitToParagraphs();

           // String fullPath = this.textBox3.Text;
            string path = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            string name = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);

            List<string> list = merge.split(FileHandler.read(fullPath));
            StretchTime stretch = new StretchTime(list, span1, check);
            stretch.setThresholdA(Convert.ToDouble(this.textBoxThreshold.Text) * 1000);
            list = stretch.stretch(span2);
            
            Directory.CreateDirectory(path + "-改好时间\\");
            FileHandler.writeStartTimeEndTime(path + "-改好时间\\开始时间-结束时间-" + name, stretch.startTimeEndTime);
            FileHandler.write(path + "-改好时间\\改好时间-" + name, list);
            FileHandler.write(path + "-改好时间\\调试用-改好时间-" + name, stretch.getDebugResultArticle());
            
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
            int secondBetweenParagraphs = (int)(Convert.ToDouble(this.textBoxParagraphSpan1.Text) * 1000);

            StretchTime stretch = new StretchTime(list,0,false);
            stretch.setThresholdA(Convert.ToDouble(this.textBoxThreshold.Text) * 1000);
            stretch.articleToParagraphBlocks();
            stretch.mergeBlockByTimeSpanBetweenParagraphs(secondBetweenParagraphs);
             

            FileHandler.write(path + "\\分好段-" + name, stretch.getResultArticle());
            MessageBox.Show("已经输出到：" + path + "\\分好段-" + name);
        }


        private void btnAddTitleSeq_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择需要加序号的文件所在的文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tBAddTitleSeq.Text = dialog.SelectedPath;
            }
        }

        private void btnAddSeqOK_Click(object sender, EventArgs e)
        {
            if (tBAddTitleSeq.Text == "")
            {
                MessageBox.Show("请先选择文件夹");
                return;
            }
            AddTitleSeq instance = new AddTitleSeq();
            List<string> stat = new List<string>();
            instance.listFiles(tBAddTitleSeq.Text, stat);

            Directory.CreateDirectory(tBAddTitleSeq.Text+"-加标题序号");
            FileHandler.write(tBAddTitleSeq.Text+"-加标题序号\\段数统计.txt", stat);
            MessageBox.Show("加标题序号成功,结果在：“" + tBAddTitleSeq.Text + "-加标题序号” 文件夹 ", "加标题序号成功");
        }


 
    }
}
