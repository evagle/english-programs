using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrammarRecognition.src.main.logical;
using GrammarRecognition.src.main.model;
 
 
namespace GrammarRecognition
{
    public partial class MainForm : Form
    {
        private String type;
        private String grammarAbbr;
        private Controler controler;
        public MainForm()
        {
            InitializeComponent();
            cbType.Items.Add("文章");
            cbType.Items.Add("句子");
            cbType.SelectedIndex = 0;
            cbType2.Items.Add("文章");
            cbType2.Items.Add("句子");
            cbType2.SelectedIndex = 0;
        }

        private void btnSelectPOS_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择词表所在文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbPOS.Text = dialog.SelectedPath;
            }
        }

        private void btnSelectGrammar_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                tbGrammar.Text = openFileDialog.FileName;
            }
        }

        private void btPhrase_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
            }
        }

        private void btnParagraph_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文章所在文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbParagraph.Text = dialog.SelectedPath;
            }
        }

        private void btnOutDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择输出文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbOutDir.Text = dialog.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //////////////////////////////////////////////////////////////////////////
            //测试用
            /*tbPOS.Text = "D:\\刘实-英语项目\\语法识别程序\\词表";
            tbGrammar.Text = "D:\\刘实-英语项目\\语法识别程序\\语法加固定搭配20140416补充版.txt";
            tbOutDir.Text = "D:\\刘实-英语项目\\语法识别程序\\结果目录";
            tbParagraph.Text = "D:\\刘实-英语项目\\语法识别程序\\测试文章";
            */
            //////////////////////////////////////////////////////////////////////////
            if (tbPOS.Text == "")
            {
                MessageBox.Show("请选择词性表所在目录");
                return;
            }
            if (tbGrammar.Text == "")
            {
                MessageBox.Show("请选择语法表");
                return;
            }
            if (tbParagraph.Text == "")
            {
                MessageBox.Show("请选择文章所在目录");
                return;
            }
            if (tbOutDir.Text == "")
            {
                MessageBox.Show("请选择输出目录");
                return;
            }

            controler = new Controler(tbPOS.Text,tbGrammar.Text,
                tbParagraph.Text,tbOutDir.Text);
            foreach(Grammar g in controler.getGrammarList()){
                cbGrammar.Items.Add(g.Abbreviation);
            }
            String tmp = "";
            int n = 0;
            foreach (Grammar g in controler.getGrammarList())
            {
                n++;
                tmp += (g.Text+"\n");
            }
            rtbNameAbbr.Text = tmp;
            rtbNameAbbr.Font = new System.Drawing.Font("宋体", 12);
            MessageBox.Show("已经完成");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
           
            if (controler!=null&&controler.getGrammarList().Count > 0&&cbGrammar.SelectedIndex>=0)
            {
                List<string> list= controler.getTextByHighestGrammar(cbGrammar.Items[cbGrammar.SelectedIndex].ToString(), cbType.SelectedIndex);
                new ShowText(list).Show();
            }
            else if (cbGrammar.SelectedIndex == -1)
            {
                MessageBox.Show("请选择语法");
            }
            else
            {
                MessageBox.Show("还未进行语法分析，不能查询");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnQuery2_Click(object sender, EventArgs e)
        {
            String[] inputGrammars = tbGrammars.Text.Split(new char[] { ',', '，' });
            if (inputGrammars.Length > 0)
            {
                List<string> list = controler.getTextHasGrammars(
                 inputGrammars, cbType2.SelectedIndex);
                new ShowText(list).Show();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
