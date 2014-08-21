namespace GrammarRecognition
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbPOS = new System.Windows.Forms.TextBox();
            this.btnSelectPOS = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbGrammar = new System.Windows.Forms.TextBox();
            this.btnSelectGrammar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbParagraph = new System.Windows.Forms.TextBox();
            this.btnParagraph = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbOutDir = new System.Windows.Forms.TextBox();
            this.btnOutDir = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbGrammar = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tbGrammars = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnQuery2 = new System.Windows.Forms.Button();
            this.cbType2 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rtbNameAbbr = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbAssociate = new System.Windows.Forms.TextBox();
            this.buttonAssociate = new System.Windows.Forms.Button();
            this.cbAssociate = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "词性表所在目录";
            // 
            // tbPOS
            // 
            this.tbPOS.Location = new System.Drawing.Point(139, 18);
            this.tbPOS.Name = "tbPOS";
            this.tbPOS.Size = new System.Drawing.Size(180, 21);
            this.tbPOS.TabIndex = 1;
            // 
            // btnSelectPOS
            // 
            this.btnSelectPOS.Location = new System.Drawing.Point(341, 16);
            this.btnSelectPOS.Name = "btnSelectPOS";
            this.btnSelectPOS.Size = new System.Drawing.Size(77, 23);
            this.btnSelectPOS.TabIndex = 2;
            this.btnSelectPOS.Text = "选择";
            this.btnSelectPOS.UseVisualStyleBackColor = true;
            this.btnSelectPOS.Click += new System.EventHandler(this.btnSelectPOS_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "语法表文件";
            // 
            // tbGrammar
            // 
            this.tbGrammar.Location = new System.Drawing.Point(139, 60);
            this.tbGrammar.Name = "tbGrammar";
            this.tbGrammar.Size = new System.Drawing.Size(180, 21);
            this.tbGrammar.TabIndex = 4;
            // 
            // btnSelectGrammar
            // 
            this.btnSelectGrammar.Location = new System.Drawing.Point(342, 55);
            this.btnSelectGrammar.Name = "btnSelectGrammar";
            this.btnSelectGrammar.Size = new System.Drawing.Size(76, 20);
            this.btnSelectGrammar.TabIndex = 5;
            this.btnSelectGrammar.Text = "选择";
            this.btnSelectGrammar.UseVisualStyleBackColor = true;
            this.btnSelectGrammar.Click += new System.EventHandler(this.btnSelectGrammar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "文章所在目录";
            // 
            // tbParagraph
            // 
            this.tbParagraph.Location = new System.Drawing.Point(139, 175);
            this.tbParagraph.Name = "tbParagraph";
            this.tbParagraph.Size = new System.Drawing.Size(180, 21);
            this.tbParagraph.TabIndex = 10;
            // 
            // btnParagraph
            // 
            this.btnParagraph.Location = new System.Drawing.Point(341, 173);
            this.btnParagraph.Name = "btnParagraph";
            this.btnParagraph.Size = new System.Drawing.Size(76, 20);
            this.btnParagraph.TabIndex = 11;
            this.btnParagraph.Text = "选择";
            this.btnParagraph.UseVisualStyleBackColor = true;
            this.btnParagraph.Click += new System.EventHandler(this.btnParagraph_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "结果输出目录";
            // 
            // tbOutDir
            // 
            this.tbOutDir.Location = new System.Drawing.Point(139, 220);
            this.tbOutDir.Name = "tbOutDir";
            this.tbOutDir.Size = new System.Drawing.Size(180, 21);
            this.tbOutDir.TabIndex = 13;
            // 
            // btnOutDir
            // 
            this.btnOutDir.Location = new System.Drawing.Point(341, 221);
            this.btnOutDir.Name = "btnOutDir";
            this.btnOutDir.Size = new System.Drawing.Size(76, 20);
            this.btnOutDir.TabIndex = 14;
            this.btnOutDir.Text = "选择";
            this.btnOutDir.UseVisualStyleBackColor = true;
            this.btnOutDir.Click += new System.EventHandler(this.btnOutDir_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(341, 264);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(73, 23);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(243, 265);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 22);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "1. 语法";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(364, 14);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(61, 23);
            this.btnQuery.TabIndex = 19;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(217, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "序数最高的";
            // 
            // cbGrammar
            // 
            this.cbGrammar.FormattingEnabled = true;
            this.cbGrammar.Location = new System.Drawing.Point(61, 18);
            this.cbGrammar.Name = "cbGrammar";
            this.cbGrammar.Size = new System.Drawing.Size(150, 20);
            this.cbGrammar.TabIndex = 22;
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(290, 18);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(68, 20);
            this.cbType.TabIndex = 23;
            // 
            // tbGrammars
            // 
            this.tbGrammars.Location = new System.Drawing.Point(14, 73);
            this.tbGrammars.Name = "tbGrammars";
            this.tbGrammars.Size = new System.Drawing.Size(386, 21);
            this.tbGrammars.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(191, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "2. 含有语法（多个请逗号隔开）：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(406, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 26;
            this.label8.Text = "的";
            // 
            // btnQuery2
            // 
            this.btnQuery2.Location = new System.Drawing.Point(90, 97);
            this.btnQuery2.Name = "btnQuery2";
            this.btnQuery2.Size = new System.Drawing.Size(75, 21);
            this.btnQuery2.TabIndex = 27;
            this.btnQuery2.Text = "查询";
            this.btnQuery2.UseVisualStyleBackColor = true;
            this.btnQuery2.Click += new System.EventHandler(this.btnQuery2_Click);
            // 
            // cbType2
            // 
            this.cbType2.FormattingEnabled = true;
            this.cbType2.Location = new System.Drawing.Point(14, 97);
            this.cbType2.Name = "cbType2";
            this.cbType2.Size = new System.Drawing.Size(60, 20);
            this.cbType2.TabIndex = 28;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbType2);
            this.groupBox1.Controls.Add(this.btnQuery2);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbGrammars);
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.cbGrammar);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 315);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 145);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询区";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(482, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 12);
            this.label9.TabIndex = 30;
            this.label9.Text = "语法名称简写对应表";
            // 
            // rtbNameAbbr
            // 
            this.rtbNameAbbr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbNameAbbr.Location = new System.Drawing.Point(485, 46);
            this.rtbNameAbbr.Name = "rtbNameAbbr";
            this.rtbNameAbbr.Size = new System.Drawing.Size(391, 388);
            this.rtbNameAbbr.TabIndex = 31;
            this.rtbNameAbbr.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 125);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "关联词表";
            // 
            // tbAssociate
            // 
            this.tbAssociate.Location = new System.Drawing.Point(139, 124);
            this.tbAssociate.Name = "tbAssociate";
            this.tbAssociate.Size = new System.Drawing.Size(180, 21);
            this.tbAssociate.TabIndex = 33;
            // 
            // buttonAssociate
            // 
            this.buttonAssociate.Location = new System.Drawing.Point(342, 122);
            this.buttonAssociate.Name = "buttonAssociate";
            this.buttonAssociate.Size = new System.Drawing.Size(76, 20);
            this.buttonAssociate.TabIndex = 34;
            this.buttonAssociate.Text = "选择";
            this.buttonAssociate.UseVisualStyleBackColor = true;
            this.buttonAssociate.Click += new System.EventHandler(this.buttonAssociate_Click);
            // 
            // cbAssociate
            // 
            this.cbAssociate.AutoSize = true;
            this.cbAssociate.Location = new System.Drawing.Point(36, 98);
            this.cbAssociate.Name = "cbAssociate";
            this.cbAssociate.Size = new System.Drawing.Size(96, 16);
            this.cbAssociate.TabIndex = 35;
            this.cbAssociate.Text = "使用关联词表";
            this.cbAssociate.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 489);
            this.Controls.Add(this.cbAssociate);
            this.Controls.Add(this.buttonAssociate);
            this.Controls.Add(this.tbAssociate);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.rtbNameAbbr);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnOutDir);
            this.Controls.Add(this.tbOutDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnParagraph);
            this.Controls.Add(this.tbParagraph);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSelectGrammar);
            this.Controls.Add(this.tbGrammar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectPOS);
            this.Controls.Add(this.tbPOS);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "语法识别";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPOS;
        private System.Windows.Forms.Button btnSelectPOS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbGrammar;
        private System.Windows.Forms.Button btnSelectGrammar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbParagraph;
        private System.Windows.Forms.Button btnParagraph;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbOutDir;
        private System.Windows.Forms.Button btnOutDir;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbGrammar;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox tbGrammars;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnQuery2;
        private System.Windows.Forms.ComboBox cbType2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox rtbNameAbbr;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbAssociate;
        private System.Windows.Forms.Button buttonAssociate;
        private System.Windows.Forms.CheckBox cbAssociate;
    }
}

