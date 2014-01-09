using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BatchCopyPast.src;

namespace BatchCopyPast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.textBoxFile.Text = openFileDialog.FileName;
            }
        }

        private void btnOutDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择文件输出文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxOutDir.Text = dialog.SelectedPath;            
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.textBoxFile.Text == null || this.textBoxFile.Text == "")
            {
                MessageBox.Show("请选择含有文章的文件");
                return;
            }
            if (this.textBoxOutDir.Text == null || this.textBoxOutDir.Text == "")
            {
                MessageBox.Show("请先选择输出文件夹");
                return;
            }
            ClassifyArticles classify = new ClassifyArticles();
            classify.classify(this.textBoxFile.Text, this.textBoxOutDir.Text);
            MessageBox.Show("已经完成");
            this.textBoxFile.Text = "";
            this.textBoxOutDir.Text = "";
        }


    
    }
}
