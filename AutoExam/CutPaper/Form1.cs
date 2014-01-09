using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CutPaper.src.cutpaper;
using System.IO;
using System.Collections;

namespace CutPaper
{
    public partial class Form1 : Form
    {
        private Hashtable userAnswer;
        private CutPapers cuter;
        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            userAnswer = new Hashtable();
            this.richTextBox1.Font = new System.Drawing.Font("宋体", 12);
           
        }
 
        private void selectPaper_Click(object sender, EventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                String splitPath = dialog.FileName  ;
                this.textBox1.Text = dialog.FileName;
                cuter = new CutPapers();
                cuter.process(dialog.FileName);
                this.richTextBox1.Text = cuter.getPaper();
                generateAnswerSheet(cuter.getAnswerTable());
            }
        }
        private void generateAnswerSheet(Hashtable table)
        {
            if (table == null)
            {
                MessageBox.Show("试卷没有提供标准答案！");
                return;
            }
            int top = 0;
            List<int> keys = new List<int>();
            this.answerpanel.Controls.Clear();
            foreach (int s in table.Keys)
            {
                keys.Add(s);
            }
            
            keys.Sort();
             
            for (int i = 0; i < keys.Count; i++)
            {
                String ans = (String)table[keys[i]];
 
                if (ans.Equals("A") || ans.Equals("B") || ans.Equals("C") || ans.Equals("D"))
                {
                    this.answerpanel.Controls.Add(radioGroup(keys[i], top, 60, Color.White, null, null));
                    top += 60;
                }
                else
                {
                    this.answerpanel.Controls.Add(blankGroup(keys[i], top, 40, Color.White, null, null));
                    top += 40;
                }
            }
            this.answerpanel.AutoScroll = true;
            
        }
        //height = 40
        private GroupBox blankGroup(int seq, int top, int height,Color color,String userAns,String ans)
        {
            GroupBox box = new GroupBox();
            Label lable = new Label();
            lable.Text = seq.ToString() + ". ";
            lable.Location = new System.Drawing.Point(10, 15);
            lable.Size = new System.Drawing.Size(30, 20);
            box.Controls.Add(lable);
            
            TextBox text = new TextBox();
            text.AutoSize = false;
            text.Text = "";
            text.Name = seq.ToString();
            text.Location = new System.Drawing.Point(40, 13);
            text.Size = new System.Drawing.Size(200, 20);
            text.MouseLeave += new EventHandler(text_MouseLeave);

            if (ans != null)
            {
                Label ansLable = new Label();
                ansLable.Text = ans;
                ansLable.ForeColor = color;
                ansLable.Location = new System.Drawing.Point(40, 45);
                ansLable.Size = new System.Drawing.Size(100, 15);
                box.Controls.Add(ansLable);
            }
            if (userAns != null)
                text.Text = userAns;
            box.Controls.Add(text);
            box.Location = new System.Drawing.Point(10, top);
            box.Size = new System.Drawing.Size(300,height);
            box.Name = "groupBox"+seq.ToString();
            box.TabIndex = 0;
            box.TabStop = false;

            return box;
        }

        void text_MouseLeave(object sender, EventArgs e)
        {
            TextBox text = (TextBox)sender;
            int seq = int.Parse(text.Name);
            if (!text.Text.Equals(""))
            {
                userAnswer[seq] = text.Text;
            }
        }
        //height = 60
        private GroupBox radioGroup(int seq, int top,int height, Color color, String userAns,String ans)
        {
            GroupBox box1 = new GroupBox();
            
            Label lable = new Label();
            lable.Text = seq.ToString()+". ";
            lable.Location = new System.Drawing.Point(10, 10);
            lable.Size = new System.Drawing.Size(30, 20);
            box1.Controls.Add(lable);
            
            RadioButton button1 = new RadioButton();
            button1.AutoSize = false;
            button1.Text = "A";
            button1.Name = seq.ToString();
            button1.Location = new System.Drawing.Point(40, 10);
            button1.Size = new System.Drawing.Size(30, 20);
            button1.CheckedChanged += new EventHandler(button_CheckedChanged);
            box1.Controls.Add(button1);

            RadioButton button2 = new RadioButton();
            button2.Text = "B";
            button2.Name = seq.ToString();
            button2.Location = new System.Drawing.Point(170, 10);
            button2.Size = new System.Drawing.Size(30, 20);
            button2.CheckedChanged += new EventHandler(button_CheckedChanged);
            box1.Controls.Add(button2);

            RadioButton button3 = new RadioButton();
            button3.Text = "C";
            button3.Name = seq.ToString();
            button3.Location = new System.Drawing.Point(40, 35);
            button3.Size = new System.Drawing.Size(30, 20);
            button3.CheckedChanged += new EventHandler(button_CheckedChanged);
            box1.Controls.Add(button3);
            
            RadioButton button4 = new RadioButton();
            button4.Text = "D";
            button4.Name = seq.ToString();
            button4.Location = new System.Drawing.Point(170, 35);
            button4.Size = new System.Drawing.Size(30, 20);
            button4.CheckedChanged += new EventHandler(button_CheckedChanged);
            box1.Controls.Add(button4);
            if (userAns == null)
                Console.Write("");
            else if (userAns.Equals("A"))
                button1.Checked = true;
            else if (userAns.Equals("B"))
                button2.Checked = true;
            else if (userAns.Equals("C"))
                button3.Checked = true;
            else if (userAns.Equals("D"))
                button4.Checked = true;

            if (ans != null)
            {
                Label ansLable = new Label();
                ansLable.Text = ans;
                ansLable.ForeColor = color;
                ansLable.Location = new System.Drawing.Point(40, 60);
                ansLable.Size = new System.Drawing.Size(100, 20);
                box1.Controls.Add(ansLable);
            }

            box1.Location = new System.Drawing.Point(10, top);
            box1.Size = new System.Drawing.Size(300, height);
            box1.Name = seq.ToString();
            box1.TabIndex = 0;
            box1.TabStop = false;
 
            return box1;
        }

        void button_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            int seq = int.Parse(btn.Name);
            if (btn.Checked)
            {
                userAnswer[seq] = btn.Text;
            }
        }

        private void submit_Click(object sender, EventArgs e)
        {
            Hashtable standardAnswer = cuter.getAnswerTable();
            if (standardAnswer == null)
            {
                MessageBox.Show("试卷没有提供标准答案！");
                return;
            }
            List<int > keys = new List<int >();
            foreach (int s in standardAnswer.Keys)
            {
                keys.Add(s);
            }
            int top = 0;
            keys.Sort();
            this.answerpanel.Controls.Clear();
            int rightCount = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                int key = keys[i];
                if(standardAnswer[key].Equals(userAnswer[key])){
                    rightCount++;
                    String ans = (String)standardAnswer[keys[i]];
     
                    if (ans.Equals("A") || ans.Equals("B") || ans.Equals("C") || ans.Equals("D"))
                    {
                        this.answerpanel.Controls.Add(
                            radioGroup(keys[i], top, 85, Color.Green, ans, "正确"));
                        top += 85;
                    }
                    else
                    {
                        this.answerpanel.Controls.Add(blankGroup(keys[i], top, 65, Color.Green,ans, "正确"));
                        top += 65;
                    }
                }else{
                    String ans = (String)standardAnswer[key];
 
                    if (ans.Equals("A") || ans.Equals("B") || ans.Equals("C") || ans.Equals("D"))
                    {
                        this.answerpanel.Controls.Add(radioGroup(keys[i], top, 85, Color.Red,
                            (String)userAnswer[key],"×  "+"正确答案:"+ans));
                        top += 85;
                    }
                    else
                    {
                        this.answerpanel.Controls.Add(blankGroup(keys[i], top, 65, Color.Red,
                            (String)userAnswer[key], "×  " + "正确答案:" + ans));
                        top += 65;
                    }
                }
            }
            if (standardAnswer!=null&&standardAnswer.Count > 0)
                this.ScoreLable.Text = String.Format((rightCount * 100 / standardAnswer.Count).ToString());
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

   

     
        
    }
}
