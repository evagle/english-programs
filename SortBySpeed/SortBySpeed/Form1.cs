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


            SortBySpeed s = new SortBySpeed(tbFolder.Text, cbAll.Checked);
             

            //Directory.CreateDirectory(tbFolder.Text+"-加标题序号");
            //FileHandler.write(tbFolder.Text+"-加标题序号\\段数统计.txt", stat);
            MessageBox.Show("成功,结果在：“" + tbFolder.Text + "-排序” 文件夹 ", "排序成功");
        }


 
    }
}
