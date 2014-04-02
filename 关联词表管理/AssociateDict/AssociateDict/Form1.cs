using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
