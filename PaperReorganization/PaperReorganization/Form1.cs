using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PaperReorganization.src.main.logical;
using PaperReorganization.src.main.model;

namespace PaperReorganization
{
    public partial class 试卷重组 : Form
    {
        public 试卷重组()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            if (textBoxShengCi.Text == "")
            {
                MessageBox.Show("请指定生词表文件");
                return;
            }
            if (textBoxGuanLianCi.Text == "")
            {
                MessageBox.Show("请选择关联词表文件");
                return;
            }
            if (textBoxGrammar.Text == "")
            {
                MessageBox.Show("请选择语法表");
                return;
            }
            if (textBoxZhenTi.Text == "")
            {
                MessageBox.Show("请选择真题所在目录");
                return;
            }
            if (textBoxJieGuo.Text == "")
            {
                MessageBox.Show("请选择输出目录");
                return;
            }

            string shengCiPath = this.textBoxShengCi.Text;
            string guanlianPath = this.textBoxGuanLianCi.Text;
            string tiXingPath = this.textBoxZhenTi.Text;
            string grammarPath = this.textBoxGrammar.Text;
            try
            {
                Config.outPath = textBoxJieGuo.Text + "\\";
                Config.minSentenceLen = Convert.ToInt32(this.textBoxZuiDuanJuChang.Text);
                Config.totalWordsCount = Convert.ToInt32(this.textBoxTotalWord.Text);
                Config.juChangQuanZhi = Convert.ToInt32(this.textBoxPingJunJuChang.Text);
                Config.shengCiLvQuanZhi = Convert.ToInt32(this.textBoxJingShengCi.Text);
                Config.shengCiCiPingQuanZhi = Convert.ToInt32(this.textBoxPingJunCiPing.Text);
                Config.grammarQuanZhi = Convert.ToInt32(this.textBoxYuFa.Text);
                Config.grammarDensity = Convert.ToInt32(this.textBoxYuFaMiDu.Text);
                
                string[] bili = this.textBoxTiXingBiLi.Text.Split(new Char[] { ':' });
                Config.tiXingBiLi = new List<int>();
                foreach (string s in bili)
                {
                    Config.tiXingBiLi.Add(Convert.ToInt32(s));
                }
            } catch  (System.Exception ex) {
                CLog.error("参数出错：error:" + ex.Message + "  trace:" + ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(ex.Message);
                return;
            }
            /*
            string shengCiPath = "\\\\VBOXSVR\\abing\\Downloads\\真题重组卷\\附件\\生词表-有数.txt";
            string guanlianPath = "\\\\VBOXSVR\\abing\\Downloads\\真题重组卷\\附件\\关联词总表+新总语法-20170519.txt";
            string tiXingPath = "\\\\VBOXSVR\\abing\\Downloads\\真题重组卷\\附件\\真题";
            string grammarPath = "\\\\VBOXSVR\\abing\\Downloads\\真题重组卷\\附件\\语法加固定搭配20140416补充版.txt";
            */
            
            /*
            Config.minSentenceLen = 2;
            Config.totalWordsCount = 3500;
            Config.juChangQuanZhi = 1;
            Config.shengCiLvQuanZhi = 1;
            Config.shengCiCiPingQuanZhi = 1;
            Config.grammarQuanZhi = 1;
            Config.grammarDensity = 30;
            Config.outPath = "";
            Config.tiXingBiLi = new List<int>() { 1,2,4};
            */
            try
            {
                Controler control = new Controler();
                control.initAssosiateWords(guanlianPath);
                control.initShengCi(shengCiPath);
                control.initGrammar(grammarPath);
                control.initZhenTi(tiXingPath);
                control.generateNewPaper();
                MessageBox.Show("试卷生成成功");
            }
            catch (System.Exception ex)
            {
                CLog.error("出错：error:" + ex.Message + "  trace:" + ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void btnShengCiBiao_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxShengCi.Text = openFileDialog.FileName;
            }
        }

        private void btnGuanLianCiBiao_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxGuanLianCi.Text = openFileDialog.FileName;
            }
        }

        private void buttonYuFaWenJian_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxGrammar.Text = openFileDialog.FileName;
            }
        }

        private void buttonZhenTiMuLu_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择真题文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxZhenTi.Text = dialog.SelectedPath;
            }
        }

        private void buttonJieGuoMuLu_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择结果文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxJieGuo.Text = dialog.SelectedPath;
            }
        }

       
       
    }
}
