using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EnglishTest
{

    public partial class AddWordlistForm : Form
    {
        private OpenFileDialog openFileDialog1;
        private string filename;
        public AddWordlistForm()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                tbPath.Text = openFileDialog1.FileName;
                filename = openFileDialog1.SafeFileName;
            }
        }

        private void benOK_Click(object sender, EventArgs e)
        {
            WordListModel model = new WordListModel();
            model.Path = tbPath.Text;
            model.Name = tbZhName.Text;
            model.EnName = tbEnName.Text;
            if (model.Name.Equals(""))
            {
                MessageBox.Show("中文名称不能为空");
                return;
            }
            if (model.EnName.Equals(""))
            {
                MessageBox.Show("英文名称不能为空");
                return;
            }
            if (model.Path.Equals(""))
            {
                MessageBox.Show("词表路径不能为空");
                return;
            }
            for (int i = 0; i < GlobalData.WordList.Count; i++)
            {
                if (GlobalData.WordList[i].Name.Equals(model.Name))
                {
                    MessageBox.Show("词表\"" + model.Name + "\"已存在！");
                    return;
                }
            }
            string path = "data\\" + filename;
            while (File.Exists(path))
            {
                filename = "x"+filename;
                path = "data\\" + filename;
            }
            File.Copy(model.Path, path, false);
            model.Path = path;
            GlobalData.WordList.Add(model);
             
            MessageBox.Show("添加成功");
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
