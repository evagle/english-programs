using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CutPaper.src.cutpaper;
using System.Collections;

namespace TestSystem
{
    public partial class Form1 : Form
    {
        private List<Block> blocks;
        private int curpos=-1;
        private String[] answerList = new String[200];
        public Form1()
        {
            InitializeComponent();
            rtbQuestion.Font =  new System.Drawing.Font("Times New Roman", 12);
            anwserpanel.MouseWheel += new MouseEventHandler(FormSample_MouseWheel);
            anwserpanel.MouseEnter += new EventHandler(anwserpanel_MouseEnter);
        }
        void anwserpanel_MouseEnter(object sender,  EventArgs e)
        {
            anwserpanel.Focus();
        }
        void FormSample_MouseWheel(object sender, MouseEventArgs e)
        {
            //获取光标位置
            Point mousePoint = new Point(e.X, e.Y);
            //换算成相对本窗体的位置
            mousePoint.Offset(this.Location.X, this.Location.Y);
            //判断是否在panel内
            if (anwserpanel.RectangleToScreen(anwserpanel.DisplayRectangle).Contains(mousePoint))
            {
                //滚动
                anwserpanel.AutoScrollPosition = new Point(0, anwserpanel.VerticalScroll.Value - e.Delta);
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                curpos = -1;
                this.tbSelectPaper.Text = openFileDialog.FileName;
                String outpath = openFileDialog.FileName.Split(new char[] { '.' })[0];
                outpath += "_split.txt";
                CutPapers cuter = new CutPapers();
                blocks = cuter.process(tbSelectPaper.Text, outpath);
                //outpathLable.Text = "切分好的卷子已经输出到："+outpath;
               // MessageBox.Show("切分好的卷子已经输出到：" + outpath);
                btnNext.PerformClick();
                
            }
        }
        private GroupBox CreateSelectionBox(String name,Point location)
        {
            GroupBox box = new GroupBox();
            box.Location = location;
            box.Name = "groupBox1";
            box.Size = new System.Drawing.Size(400, 50);
            box.TabIndex = 0;
            box.TabStop = false;
            //box.Text = name;
            Label lable = new Label();
            lable.Text = name+". ";
            lable.Location = new Point(10, 25);
            lable.Width = 20;
            box.Controls.Add(lable);
            
            RadioButton[] rbs = new RadioButton[4];
            
            String[] selections = new String[]{"A","B","C","D"};
            for (int i = 0; i <  4; i++)
            {

                RadioButton rb = new RadioButton();
                rb.Text = selections[i];
                rb.Location = new Point(80 * i+50, 20);
                rb.Width = 80;
                rb.Name = name;
                if (answerList[Int32.Parse(name)] != null && selections[i].Equals(answerList[Int32.Parse(name)]))
                {
                    rb.Checked = true;
                }
                rb.CheckedChanged += new EventHandler(RBs_CheckedChanged);
                rbs[i] = rb;
                box.Controls.Add(rb);
            }
            return box;
        }
        public GroupBox createBlankBox(String name, Point location,int seq, String  ans)
        {
            GroupBox box = new GroupBox();
            box.Location = location;
            box.Name = name;
            box.Size = new System.Drawing.Size(400, 50);
            box.TabIndex = 0;
            box.TabStop = false;

            Label lable = new Label();
            lable.Text = seq.ToString() + ". ";
            lable.Location = new Point(10, 25);
            lable.Width = 20;
            box.Controls.Add(lable);

            TextBox textbox = new TextBox();
            textbox.Name = seq.ToString();
            if(answerList[seq]!=null)
                textbox.Text = answerList[seq];
            textbox.Location = new Point(30, 25);
            textbox.Width = 200;
            textbox.MouseLeave += new EventHandler(textbox_MouseLeave);
            box.Controls.Add(textbox);
            return box;
        }

        void textbox_MouseLeave(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            answerList[Int32.Parse(textbox.Name)] = textbox.Text;
          //  MessageBox.Show(textbox.Text);
        }
        public void removeAllChild(Control view)
        {
            for (int i = view.Controls.Count - 1; i >= 0; i--)
            {
                view.Controls[i].Dispose();
            }
        }
        private void createAnwserBlock(Block block)
        {
            Hashtable table = new Hashtable();
            table.Add("A","");
            table.Add("B","");
            table.Add("C","");
            table.Add("D","");
            Boolean isSelection = true;
            for (int i = 0; i < block.Answers.Count; i++){
                if(!table.ContainsKey(block.Answers.ElementAt(i).Trim() )){
                    isSelection = false;
                    break;
                }
            }
            for (int i = 0; i < block.Seqs.Count; i++)
            {
                if (block.Answers.Count > i)
                {
                    if (isSelection)
                    {
                        GroupBox box = CreateSelectionBox(block.Seqs[i].ToString(), new Point(30, 10 + 50 * i));
                        this.anwserpanel.Controls.Add(box);
                    }
                    else
                    {
                        GroupBox box = createBlankBox(block.Seqs[i].ToString(),
                            new Point(30, 10 + 50 * i), block.Seqs[i], block.Answers[i]);
                        this.anwserpanel.Controls.Add(box);
                    }
                }
                else
                {
                    if (block.Type == Block.BLOCK_TYPE.selection || block.Type == Block.BLOCK_TYPE.closen ||
                        block.Type == Block.BLOCK_TYPE.selection_reading)
                    {
                        GroupBox box = CreateSelectionBox(block.Seqs[i].ToString(), new Point(30, 10 + 50 * i));
                        this.anwserpanel.Controls.Add(box);
                    }
                    else
                    {
                        GroupBox box = createBlankBox(block.Seqs[i].ToString(),
                            new Point(30, 10 + 50 * i), block.Seqs[i], "");
                        this.anwserpanel.Controls.Add(box);
                    }
                }
            }
        }
        
        private void RBs_CheckedChanged(object sender, System.EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked == true)
            {
               // MessageBox.Show(rb.Name + "  " + rb.Text);
                answerList[Int32.Parse(rb.Name)] = rb.Text;
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (blocks == null)
            {
                MessageBox.Show("请先选择试卷");
                return;
            }
            curpos++;
            if (curpos == blocks.Count)
            {
                MessageBox.Show("已经是最后一题了");
                curpos--;
            }
            else
            {
                rtbQuestion.Clear();
                this.rtbQuestion.SelectedText = blocks.ElementAt(curpos).getString();
                FontFamily family = new FontFamily("宋体");

                Font font = new Font(family, 12.0f,
                FontStyle.Regular );

                rtbQuestion.SelectionFont = font;
                rtbQuestion.Font = font;
                removeAllChild(this.anwserpanel);
                createAnwserBlock(blocks.ElementAt(curpos));
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (blocks == null)
            {
                MessageBox.Show("请先选择试卷");
                return;
            }
            curpos--;
            if (curpos == -1)
            {
                MessageBox.Show("已经是第一题了");
                curpos = 0;
            }
            else
            {
                rtbQuestion.Clear();
                this.rtbQuestion.SelectedText = blocks.ElementAt(curpos).getString();
                FontFamily family = new FontFamily("宋体");
                Font font = new Font(family, 12.0f,
                FontStyle.Regular );

                rtbQuestion.SelectionFont = font;
                rtbQuestion.Font = font;
                removeAllChild(this.anwserpanel);
                createAnwserBlock(blocks.ElementAt(curpos));
            }
        }

         
    }
}
