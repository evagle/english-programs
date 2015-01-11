using System;
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
      
        private void btnAddTitleSeq_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择需要加序号的文件所在的文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnAddSeqOK_Click(object sender, EventArgs e)
        {
            if (tbFolder.Text == "")
            {
                MessageBox.Show("请先选择文件夹");
                return;
            }


            SortBySpeed s = new SortBySpeed(tbFolder.Text, checkBox2.Checked);
             
            MessageBox.Show("成功,结果在：“" + tbFolder.Text + "-排序” 文件夹 ", "排序成功");
        }

        private void buttonStart2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请先选择文件夹");
                return;
            }


            SortBySpeedByFolder s = new SortBySpeedByFolder(textBox2.Text, checkBox3.Checked);

            MessageBox.Show("成功,结果在：“" + textBox2.Text + "-排序” 文件夹 ", "排序成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择需要加序号的文件所在的文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = dialog.SelectedPath;
            }
        }

        private void button0_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择需要加序号的文件所在的文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox0.Text = dialog.SelectedPath;
            }
        }

        private void buttonStart0_Click(object sender, EventArgs e)
        {
            if (textBox0.Text == "")
            {
                MessageBox.Show("请先选择文件夹");
                return;
            }

            SortBySpeedInFile s = new SortBySpeedInFile(textBox0.Text, checkBox1.Checked);

            MessageBox.Show("成功,结果在：“" + textBox0.Text + "-排序” 文件夹 ", "排序成功");
        }



 
    }
}
