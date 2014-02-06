namespace FindAndMoveFilesWithSameName
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
            this.label1 = new System.Windows.Forms.Label();
            this.indexFileBox = new System.Windows.Forms.TextBox();
            this.btnSelectIndexFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DestFolderBox = new System.Windows.Forms.TextBox();
            this.btnSelectDestFolder = new System.Windows.Forms.Button();
            this.btnStartCopy = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbPossibleTargetFolders = new System.Windows.Forms.RichTextBox();
            this.btnSelectTargetFolders = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择含有文章和标题的文件";
            // 
            // indexFileBox
            // 
            this.indexFileBox.Location = new System.Drawing.Point(37, 61);
            this.indexFileBox.Name = "indexFileBox";
            this.indexFileBox.Size = new System.Drawing.Size(355, 20);
            this.indexFileBox.TabIndex = 1;
            // 
            // btnSelectIndexFile
            // 
            this.btnSelectIndexFile.Location = new System.Drawing.Point(429, 61);
            this.btnSelectIndexFile.Name = "btnSelectIndexFile";
            this.btnSelectIndexFile.Size = new System.Drawing.Size(85, 21);
            this.btnSelectIndexFile.TabIndex = 2;
            this.btnSelectIndexFile.Text = "选择文件";
            this.btnSelectIndexFile.UseVisualStyleBackColor = true;
            this.btnSelectIndexFile.Click += new System.EventHandler(this.btnSelectIndexFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "指定将文件移动至下面的文件夹";
            // 
            // DestFolderBox
            // 
            this.DestFolderBox.Location = new System.Drawing.Point(37, 366);
            this.DestFolderBox.Name = "DestFolderBox";
            this.DestFolderBox.Size = new System.Drawing.Size(355, 20);
            this.DestFolderBox.TabIndex = 4;
            // 
            // btnSelectDestFolder
            // 
            this.btnSelectDestFolder.Location = new System.Drawing.Point(429, 365);
            this.btnSelectDestFolder.Name = "btnSelectDestFolder";
            this.btnSelectDestFolder.Size = new System.Drawing.Size(85, 21);
            this.btnSelectDestFolder.TabIndex = 5;
            this.btnSelectDestFolder.Text = "选择文件夹";
            this.btnSelectDestFolder.UseVisualStyleBackColor = true;
            this.btnSelectDestFolder.Click += new System.EventHandler(this.btnSelectDestFolder_Click);
            // 
            // btnStartCopy
            // 
            this.btnStartCopy.Location = new System.Drawing.Point(429, 404);
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(85, 21);
            this.btnStartCopy.TabIndex = 6;
            this.btnStartCopy.Text = "开始复制";
            this.btnStartCopy.UseVisualStyleBackColor = true;
            this.btnStartCopy.Click += new System.EventHandler(this.btnStartCopy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "指出可能含有文件的文件夹";
            // 
            // rtbPossibleTargetFolders
            // 
            this.rtbPossibleTargetFolders.Location = new System.Drawing.Point(43, 128);
            this.rtbPossibleTargetFolders.Name = "rtbPossibleTargetFolders";
            this.rtbPossibleTargetFolders.Size = new System.Drawing.Size(471, 182);
            this.rtbPossibleTargetFolders.TabIndex = 8;
            this.rtbPossibleTargetFolders.Text = "";
            // 
            // btnSelectTargetFolders
            // 
            this.btnSelectTargetFolders.Location = new System.Drawing.Point(207, 96);
            this.btnSelectTargetFolders.Name = "btnSelectTargetFolders";
            this.btnSelectTargetFolders.Size = new System.Drawing.Size(85, 21);
            this.btnSelectTargetFolders.TabIndex = 9;
            this.btnSelectTargetFolders.Text = "选择";
            this.btnSelectTargetFolders.UseVisualStyleBackColor = true;
            this.btnSelectTargetFolders.Click += new System.EventHandler(this.btnSelectTargetFolders_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 460);
            this.Controls.Add(this.btnSelectTargetFolders);
            this.Controls.Add(this.rtbPossibleTargetFolders);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStartCopy);
            this.Controls.Add(this.btnSelectDestFolder);
            this.Controls.Add(this.DestFolderBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectIndexFile);
            this.Controls.Add(this.indexFileBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "文件查找和移动";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox indexFileBox;
        private System.Windows.Forms.Button btnSelectIndexFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DestFolderBox;
        private System.Windows.Forms.Button btnSelectDestFolder;
        private System.Windows.Forms.Button btnStartCopy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtbPossibleTargetFolders;
        private System.Windows.Forms.Button btnSelectTargetFolders;
    }
}

