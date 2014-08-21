namespace SrtTimeModify
{
    partial class Form1
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnAddSeqOK = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cbAll = new System.Windows.Forms.CheckBox();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbAll);
            this.groupBox4.Controls.Add(this.btnAddSeqOK);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.tbFolder);
            this.groupBox4.Controls.Add(this.btnSelect);
            this.groupBox4.Location = new System.Drawing.Point(38, 24);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(453, 130);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "排序";
            // 
            // btnAddSeqOK
            // 
            this.btnAddSeqOK.Location = new System.Drawing.Point(359, 78);
            this.btnAddSeqOK.Name = "btnAddSeqOK";
            this.btnAddSeqOK.Size = new System.Drawing.Size(75, 21);
            this.btnAddSeqOK.TabIndex = 15;
            this.btnAddSeqOK.Text = "开始排序";
            this.btnAddSeqOK.UseVisualStyleBackColor = true;
            this.btnAddSeqOK.Click += new System.EventHandler(this.btnAddSeqOK_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "选择文件夹";
            // 
            // tbFolder
            // 
            this.tbFolder.Location = new System.Drawing.Point(124, 32);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.Size = new System.Drawing.Size(211, 21);
            this.tbFolder.TabIndex = 9;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(359, 30);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 21);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnAddTitleSeq_Click);
            // 
            // cbAll
            // 
            this.cbAll.AutoSize = true;
            this.cbAll.Location = new System.Drawing.Point(30, 81);
            this.cbAll.Name = "cbAll";
            this.cbAll.Size = new System.Drawing.Size(120, 16);
            this.cbAll.TabIndex = 16;
            this.cbAll.Text = "所有文件一起排序";
            this.cbAll.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 190);
            this.Controls.Add(this.groupBox4);
            this.Name = "Form1";
            this.Text = "语速排序";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnAddSeqOK;
        private System.Windows.Forms.CheckBox cbAll;
    }
}

