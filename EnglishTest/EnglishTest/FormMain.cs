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
    public partial class FormMain : Form
    {
        public Panel pWizard;
        public FormMain()
        {
            InitializeComponent();
            
            loadConfig();

            pWizard = new WizardPanel(this);
            pWizard.Location = new Point(0, 0);
            pWizard.Size = new Size(600, 450);
            this.Controls.Add(pWizard);
            this.FormClosed += new FormClosedEventHandler(FormMain_FormClosed);
        }

        void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveConfig();
        }


        public void loadConfig()
        {

            string  path = GlobalData.WORD_LIST_CONFIG_PATH;
            StreamReader reader = new StreamReader(path);
            String str;
            while ((str = reader.ReadLine()) != null)
            {
                //从文本txt中读取航线信息
                //每行4个数字，起点的X，Y坐标，终点的X，Y坐标
                if (str.Equals(""))
                    continue;
                String[] tmp = str.Split(new char[] { '#' });
                if (tmp.Length == 3)
                {
                    WordListModel model = new WordListModel();
                    model.Name = tmp[0];
                    model.Path = tmp[1];
                    model.EnName = tmp[2];
                    GlobalData.WordList.Add(model);
                }             
            }
            reader.Close();
        }
        public void saveConfig()
        {
            string path = GlobalData.WORD_LIST_CONFIG_PATH;
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < GlobalData.WordList.Count; i++)
                {
                    String data = GlobalData.WordList.ElementAt(i).ToString();
                    writer.WriteLine(data);
                }
                writer.Flush();
                writer.Close();
            }
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddWordlistForm form = new AddWordlistForm();
            form.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteWordListForm form = new DeleteWordListForm();
            form.Show();
        }
 
    }
}
