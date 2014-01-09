using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EnglishTest
{
    class WizardPanel : Panel
    {
        public Form form;
        public Button button;
        public ComboBox cbWordList;
        public RadioButton rbMethodWordList;
        public RadioButton rbMethodJump;
        public ComboBox cbJump1;
        public ComboBox cbJump2;
        public RichTextBox rtbInput;
        public List<int> ten = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public List<int> ten1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public WizardPanel(Form form)
        {
            this.form = form;
            InitializeComponent();
            InitListeners();
        }
        public void InitListeners()
        {
            rtbInput.MouseClick += new MouseEventHandler(rtbInput_MouseClick);
            button.Click += new EventHandler(button_Click);
        }

        void rtbInput_MouseClick(object sender, MouseEventArgs e)
        {
            if (rtbInput.Text == "在此输入英文原文")
            {
                rtbInput.Text = "";
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            if (rtbInput.Text == "" || rtbInput.Text == "在此输入英文原文")
            {
                MessageBox.Show("请先输入英文原文");
                rtbInput.Text = "在此输入英文原文";
                return;
            }
            else
            {
                GlobalData.original_article = rtbInput.Text;
                if (rbMethodWordList.Checked)
                {
                    GlobalData.examType = GlobalData.Exam_Type.WORD_LIST;
                    GlobalData.selectedWordList = GlobalData.WordList.ElementAt(cbWordList.SelectedIndex);
                    DialogResult dialogResult = MessageBox.Show(@"确定用词表挖空法?
将根据词表“" + GlobalData.selectedWordList.Name+"”进行挖空", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Cancel)
                        return;
                }
                else
                {
                    GlobalData.examType = GlobalData.Exam_Type.JUMP_WORD;
                    GlobalData.jumpNum = cbJump1.SelectedIndex + 1;
                    GlobalData.selectNum = cbJump2.SelectedIndex + 1;
                    DialogResult dialogResult = MessageBox.Show(@"确定用间隔挖空法?
隔" + GlobalData.jumpNum + "个词挖" + GlobalData.selectNum +"个词", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Cancel)
                        return;
                   
                }              
            }
            
            PaperPanel panel= new PaperPanel(form);
            panel.Location =  new Point(0, 0);
            panel.Size = new Size(form.Width,form.Height);
            form.Controls.RemoveAt(form.Controls.Count - 1);
            form.Controls.Add(panel);
            
 

        }


        public void InitializeComponent()
        {

            rbMethodWordList = new RadioButton();
            rbMethodWordList.Location = new Point(20, 35);
            rbMethodWordList.Size = new Size(80, 30);
            rbMethodWordList.Text = "词表挖词";
            rbMethodWordList.Checked = true;
            this.Controls.Add(rbMethodWordList);

            rbMethodJump = new RadioButton();
            rbMethodJump.Location = new Point(20, 70);
            rbMethodJump.Size = new Size(80, 30);
            rbMethodJump.Text = "间隔挖词";
            this.Controls.Add(rbMethodJump);

            cbWordList = new ComboBox();
            cbWordList.Location = new Point(100, 40);
            cbWordList.Size = new Size(140, 30);
            
            for (int i = 0; i < GlobalData.WordList.Count; i++)
            {
                cbWordList.Items.Add(GlobalData.WordList[i].Name);
            }
            cbWordList.SelectedIndex = 0;
            cbWordList.DropDown += new EventHandler(cbWordList_DropDown);
            this.Controls.Add(cbWordList);

        

            Label label = new Label();
            label.Text = "每隔";
            label.Location = new Point(100, 78);
            label.Size = new Size(35, 30);
            this.Controls.Add(label);

            cbJump1 = new ComboBox();
            cbJump1.DataSource = ten;
            cbJump1.Location = new Point(135, 75);
            cbJump1.Size = new Size(40, 30);
            this.Controls.Add(cbJump1);

            label = new Label();
            label.Text = "个单词，挖去";
            label.Location = new Point(175, 78);
            label.Size = new Size(80, 30);
            this.Controls.Add(label);

            cbJump2 = new ComboBox();
            cbJump2.DataSource = ten1;
            cbJump2.Location = new Point(260, 75);
            cbJump2.Size = new Size(40, 30);
            this.Controls.Add(cbJump2);

            label = new Label();
            label.Text = "个单词";
            label.Location = new Point(300, 78);
            label.Size = new Size(50, 30);
            this.Controls.Add(label);

            rtbInput = new RichTextBox();
            rtbInput.Location = new Point(20, 108);
            rtbInput.Size = new Size(600, 300);
            rtbInput.Text = "在此输入英文原文";
            rtbInput.Font = new System.Drawing.Font("Times New Roman",12);
            
            this.Controls.Add(rtbInput);

            button = new Button();
            button.Location = new Point(270, 410);
            button.Size = new Size(100, 30);
            button.Text = "生成试题";
            this.Controls.Add(button);

        }

        void cbWordList_DropDown(object sender, EventArgs e)
        {
            cbWordList.Items.Clear();
            for (int i = 0; i < GlobalData.WordList.Count; i++)
            {
                cbWordList.Items.Add(GlobalData.WordList[i].Name);
            }
            cbWordList.SelectedIndex = 0;
        }

    }
}
