using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EnglishTest
{
    class PaperPanel : Panel
    {
        public RichTextBox rtbExam;
        public TestPaper testpaper;
        public Button submit;
        public List<string> answerList;
 
        public int correctAnswerNumber;
        public Label scorelabel;
        public Button returnBtn;
        public Button showAnswerButton;
        public Form baseform;
        public PaperPanel(Form form)
        {
            testpaper = new TestPaper(GlobalData.original_article);          
            InitializeComponent();
            InitListeners();
            answerList = new List<string>();
            this.baseform = form;
           
        }
        public void InitializeComponent()
        {

            Label label = new Label();
            label.Text = "分数: ";
            label.Location = new Point(20, 35);
            label.Size = new Size(50, 25);
            label.Font = new System.Drawing.Font("Times New Roman", 12);
            this.Controls.Add(label);

            scorelabel = new Label();
            scorelabel.Text = "";
            scorelabel.Location = new Point(75, 35);
            scorelabel.Size = new Size(50, 25);
            scorelabel.Font = new System.Drawing.Font("Times New Roman", 12);
            scorelabel.ForeColor = Color.Red;
            this.Controls.Add(scorelabel);

            rtbExam = new RichTextBox();
            rtbExam.Location = new Point(20, 70);
            rtbExam.Size = new Size(600, 340);
            rtbExam.Text = testpaper.getPaper();
            rtbExam.Font = new System.Drawing.Font("Times New Roman", 12);
            for (int i = 0; i < testpaper.questionIndexes.Count; i++)
            {
                rtbExam.Select(testpaper.questionIndexes[i], 7);
                FontStyle fontStyle = FontStyle.Underline;
                rtbExam.SelectionFont = new Font("Times New Roman", rtbExam.SelectionFont.Size, fontStyle);                           
            }
            rtbExam.Refresh();
            rtbExam.Select(0, 0);
            rtbExam.WordWrap = true;
            this.Controls.Add(rtbExam);

            submit = new Button();
            submit.Location = new Point(190, 415);
            submit.Size = new Size(70, 30);
            submit.Text = "交卷";
            this.Controls.Add(submit);

            showAnswerButton = new Button();
            showAnswerButton.Location = new Point(270, 415);
            showAnswerButton.Size = new System.Drawing.Size(100, 30);
            showAnswerButton.Text = "显示正确答案";
            this.Controls.Add(showAnswerButton);

            returnBtn = new Button();
            returnBtn.Location = new Point(380, 415);
            returnBtn.Size = new Size(70, 30);
            returnBtn.Text = "返回";
            this.Controls.Add(returnBtn);
        }

        public void InitListeners() {
            rtbExam.MouseClick += new MouseEventHandler(rtbExam_MouseClick);
           // rtbExam.TextChanged += new EventHandler(rtbExam_TextChanged);
            submit.Click += new EventHandler(submit_Click);
            showAnswerButton.MouseClick += new MouseEventHandler(showAnswerButton_MouseClick);
            returnBtn.Click += new EventHandler(returnBtn_Click);
        }

        void returnBtn_Click(object sender, EventArgs e)
        {
            WizardPanel panel= new WizardPanel(baseform);
            panel.Location =  new Point(0, 0);
            panel.Size = new Size(baseform.Width,baseform.Height);
            baseform.Controls.RemoveAt(baseform.Controls.Count-1);
            baseform.Controls.Add(panel);
        }

        void showAnswerButton_MouseClick(object sender, MouseEventArgs e)
        {
            showCorrectAnswer();
            showAnswerButton.Enabled = false;
            submit.Enabled = false; 
        }

        public void CheckAnswer()
        {
            correctAnswerNumber =0;
            for (int i = 0; i < answerList.Count && i < testpaper.QuestionList.Count; i++)
            {
                if (answerList[i].Equals(testpaper.QuestionList[i]))
                {
                    correctAnswerNumber++;
                }
            }
        }

        private void showCorrectAnswer()
        {
            getAnswer();
            string origin = testpaper.getPaper();
            StringBuilder corrected = new StringBuilder();
            //用户原始答案的位置
            List<Point> userAnsPositions = new List<Point>();
            //对错标记的位置
            List<Point> RWMarkPositions = new List<Point>();
            //正确答案的位置
            List<Point> correctAnsPositions = new List<Point>();

            int preIndex = 0;
            Point point;
            if (answerList.Count < testpaper.QuestionList.Count)
            {
                int n=testpaper.QuestionList.Count - answerList.Count;
                for (int i = 0; i <n ; i++)
                    answerList.Add("");
            }

            for (int i = 0; i < testpaper.QuestionList.Count; i++)
            {
                corrected.Append(origin.Substring(preIndex, testpaper.questionIndexes[i]-preIndex));             
                point = new Point();
                point.X = corrected.Length;
                corrected.Append(answerList[i]+" ");
                point.Y = corrected.Length;
                userAnsPositions.Add(point);

                point = new Point();      
                point.X = corrected.Length;
                if (answerList[i] == testpaper.QuestionList[i])
                    corrected.Append("[√] ");
                else
                    corrected.Append("[×] ");

                point.Y = corrected.Length;
                RWMarkPositions.Add(point);
                 
                if (answerList[i] != testpaper.QuestionList[i])
                {
                    point = new Point();
                    point.X = corrected.Length;
                    corrected.Append(testpaper.QuestionList[i]+" ");
                    point.Y = corrected.Length;
                    correctAnsPositions.Add(point);
                }
                //corrected.Append(" ");
                preIndex = testpaper.questionIndexes[i] + 7;
            }
            corrected.Append(origin.Substring(preIndex,origin.Length -preIndex));
            rtbExam.Text = corrected.ToString();
            rtbExam.SelectAll();
            rtbExam.ForeColor = Color.Black;
            rtbExam.Font = new Font("Times New Roman", 12, FontStyle.Regular);
            FontStyle fontStyle = FontStyle.Underline;
            for (int i = 0; i < userAnsPositions.Count; i++)
            {
                point = userAnsPositions[i];
                if (point.Y > point.X)
                {
                    rtbExam.Select(point.X, point.Y - point.X);
                    rtbExam.SelectionFont = new Font("Times New Roman", 12, fontStyle);
                    rtbExam.SelectionColor = Color.Blue;
                }
            }
            for (int i = 0; i < RWMarkPositions.Count; i++)
            {
                point = RWMarkPositions[i];
                if (point.Y > point.X) { 
                    rtbExam.Select(point.X, point.Y - point.X);
                    rtbExam.SelectionFont = new Font("Times New Roman", 12, fontStyle);
                    if (answerList[i] != testpaper.QuestionList[i])
                        rtbExam.SelectionColor = Color.Red;
                    else
                        rtbExam.SelectionColor = Color.Green;
                }
            }
            for (int i = 0; i < correctAnsPositions.Count; i++)
            {
                point = correctAnsPositions[i];
                if (point.Y > point.X)
                {
                    rtbExam.Select(point.X, point.Y - point.X);
                    rtbExam.SelectionFont = new Font("Times New Roman", 12, fontStyle);
                    rtbExam.SelectionColor = Color.Green;
                }
            }
           // showAnswerButton.Enabled = false;
            //submit.Enabled = false;
        }
        public void getAnswer()
        {
            answerList = new List<string>();
            for (int i = 0; i < rtbExam.TextLength; )
            {
                int len = 1;
                rtbExam.Select(i, len);
                while (rtbExam.SelectionFont!=null&&rtbExam.SelectionFont.Style == FontStyle.Underline)
                {
                    len++;
                    rtbExam.Select(i, len);
                }
                if (len > 1 )
                {
                    string str = rtbExam.Text.Substring(i, len - 1);
                    if (str.StartsWith("]"))
                    {
                        i += len;
                        continue;
                    }
                    int start = 0;
                    while (start< str.Length&& !((str[start] >= 'a' && str[start] <= 'z') ||
                        (str[start] >= 'A' && str[start] <= 'Z') ||
                        str[start] == '-' || str[start] == '\'' || str[start] == '’'))
                    {
                        start++;
                    }
                    int end = start;
                    while (end<str.Length&&((str[end] >= 'a' && str[end] <= 'z') ||
                        (str[end] >= 'A' && str[end] <= 'Z') ||
                        str[end] == '-' || str[end] == '\'' || str[end] == '’'))
                    {
                        end++;
                    }
                    if (end > start )
                        answerList.Add(str.Substring(start, end - start));
                    else
                        answerList.Add("");
                }
                i += len;
            }

            for (int i = 0; i < answerList.Count; i++)
                answerList[i] = answerList[i].Trim();
           
        }
        void submit_Click(object sender, EventArgs e)
        {

            getAnswer();
            CheckAnswer();
 
            if (testpaper.QuestionList.Count > 0)
                scorelabel.Text = (correctAnswerNumber * 100 / testpaper.QuestionList.Count) + "";
        }

        void rtbExam_TextChanged(object sender, EventArgs e)
        {
            rtbExam.Select(rtbExam.SelectionStart, 1);
            if (rtbExam.SelectionFont.Style != FontStyle.Underline)
                rtbExam.ReadOnly = true;
            else
                rtbExam.ReadOnly = false;
        }

        void rtbExam_MouseClick(object sender, MouseEventArgs e)
        {
            rtbExam.Select(rtbExam.SelectionStart, 1);
            if (rtbExam.SelectionFont.Style != FontStyle.Underline)
                rtbExam.ReadOnly = true;
            else
                rtbExam.ReadOnly = false;
               
        }

    }

}
