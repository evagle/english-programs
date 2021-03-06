﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FindAndMoveFilesWithSameName.src;

namespace FindAndMoveFilesWithSameName
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<String> possibleTargetFolders = new List<String>();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSelectIndexFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); 
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK) // Test result.
            {
                this.indexFileBox.Text = openFileDialog.FileName;
            }
        }

        private void btnSelectDestFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            
            dialog.Description = "请选择目标文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.DestFolderBox.Text = dialog.SelectedPath;
            }
        }

        private void btnStartCopy_Click(object sender, EventArgs e)
        {
            /************************************************************************/
            /* Test                                                                     */
            /************************************************************************/
            //this.indexFileBox.Text = "F:\\Downloads\\刘实-英语项目\\文件查找拷贝\\indexfile.txt";
            //this.rtbPossibleTargetFolders.Text = "F:\\Downloads\\刘实-英语项目\\文件查找拷贝\\src - Copy";
            //this.DestFolderBox.Text = "F:\\Downloads\\刘实-英语项目\\文件查找拷贝\\destination";
            /*End**/
            if (this.indexFileBox.Text == null || this.indexFileBox.Text == "") {
                MessageBox.Show("请选择含有文章标题的索引文件");
                return;
            }
            if (this.rtbPossibleTargetFolders.Text == null || this.rtbPossibleTargetFolders.Text == "")
            {
                MessageBox.Show("请选择可能的文件夹路径");
                return;
            }
            if (this.DestFolderBox.Text == null || this.DestFolderBox.Text == "")
            {
                MessageBox.Show("请选需要移动至的路径");
                return;
            }

            MainController controller = new MainController(this.indexFileBox.Text,
                this.rtbPossibleTargetFolders.Lines, this.DestFolderBox.Text);
            controller.moveFiles();
            MessageBox.Show("文件成功移动到: " + this.DestFolderBox.Text, "移动成功");
        }

        private void btnSelectTargetFolders_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择目标文件夹";
            //this.rtbPossibleTargetFolders.Text = "fasdf\ndsufi\n";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.rtbPossibleTargetFolders.Text = this.rtbPossibleTargetFolders.Text +
                    dialog.SelectedPath + "\n";
            }
             
        }
       
    }

}
