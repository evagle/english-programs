using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CutPaper.src.cutpaper;
using System.IO;

namespace CutPaper
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
                this.tbPath.Text = openFileDialog.FileName;
                String outpath = openFileDialog.FileName.Split(new char[] { '.' })[0];
                //outpath += "_split.txt";
                CutPapers cuter = new CutPapers();
                cuter.process(tbPath.Text,outpath);
                //outpathLable.Text = "切分好的卷子已经输出到："+outpath;
                MessageBox.Show("切分好的卷子已经输出到：\n" + outpath + "_split.txt" + "\n和\n" + outpath + "_填入答案后的试卷.txt");
            }
        }

        private void btt2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择试卷所在文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.SelectedPath;
                String splitPath = dialog.SelectedPath + "_split";
                if (!Directory.Exists(splitPath))
                {
                    Directory.CreateDirectory(splitPath);
                }
                CutPapers cuter = new CutPapers();
                
                DirectoryInfo dirInfo = new DirectoryInfo(dialog.SelectedPath);
                FileInfo[] files = dirInfo.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    //handleFile(files[i].FullName, files[i].Name, map);
                    cuter.process(files[i].FullName, splitPath + "\\" + files[i].Name.Split(new char[] { '.' })[0]);
                }
                MessageBox.Show("结果已经输出到文件夹：" + splitPath);
            }
        }
        
    }
}
