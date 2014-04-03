using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AssociateDict.src;

namespace AssociateDict
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAssociate_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.tbAssociate.Text = openFileDialog.FileName;
                string path = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf("\\"));
                string name = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1);
             
                
            }
        }

        private void btnAbnormal_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择不规则词表文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbAbnormal.Text = dialog.SelectedPath;
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择输出文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbOutput.Text = dialog.SelectedPath;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            /*
            this.tbAssociate.Text = "D:\\gitrepo\\english-programs\\关联词表管理\\关联词表20130829.txt";
            this.tbAbnormal.Text = "D:\\gitrepo\\english-programs\\关联词表管理\\词表";
            this.tbOutput.Text = "D:\\gitrepo\\english-programs\\关联词表管理\\输出";
            */
            if (this.tbAssociate.Text.Equals(""))
            {
                MessageBox.Show("没有选择关联词表");
                return;
            }
            if (this.tbAbnormal.Text.Equals(""))
            {
                MessageBox.Show("没有选择不规则词表");
                return;
            }
            if (this.tbOutput.Text.Equals(""))
            {
                MessageBox.Show("没有配置输出目录");
                return;
            }
            Main m = new Main();
            m.main(this.tbAssociate.Text, this.tbAbnormal.Text, this.tbOutput.Text);
            MessageBox.Show("关联词表配置成功");
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
