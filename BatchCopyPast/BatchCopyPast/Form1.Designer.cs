namespace BatchCopyPast
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
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.textBoxOutDir = new System.Windows.Forms.TextBox();
            this.输出文件夹 = new System.Windows.Forms.Label();
            this.btnOutDir = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择文件：";
            // 
            // textBoxFile
            // 
            this.textBoxFile.Location = new System.Drawing.Point(95, 31);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.Size = new System.Drawing.Size(347, 20);
            this.textBoxFile.TabIndex = 1;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(473, 31);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "选择";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // textBoxOutDir
            // 
            this.textBoxOutDir.Location = new System.Drawing.Point(95, 83);
            this.textBoxOutDir.Name = "textBoxOutDir";
            this.textBoxOutDir.Size = new System.Drawing.Size(347, 20);
            this.textBoxOutDir.TabIndex = 3;
            // 
            // 输出文件夹
            // 
            this.输出文件夹.AutoSize = true;
            this.输出文件夹.Location = new System.Drawing.Point(22, 83);
            this.输出文件夹.Name = "输出文件夹";
            this.输出文件夹.Size = new System.Drawing.Size(67, 13);
            this.输出文件夹.TabIndex = 4;
            this.输出文件夹.Text = "输出文件夹";
            // 
            // btnOutDir
            // 
            this.btnOutDir.Location = new System.Drawing.Point(473, 81);
            this.btnOutDir.Name = "btnOutDir";
            this.btnOutDir.Size = new System.Drawing.Size(75, 23);
            this.btnOutDir.TabIndex = 5;
            this.btnOutDir.Text = "选择";
            this.btnOutDir.UseVisualStyleBackColor = true;
            this.btnOutDir.Click += new System.EventHandler(this.btnOutDir_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(473, 141);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 210);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnOutDir);
            this.Controls.Add(this.输出文件夹);
            this.Controls.Add(this.textBoxOutDir);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "批量复制粘贴文本";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox textBoxOutDir;
        private System.Windows.Forms.Label 输出文件夹;
        private System.Windows.Forms.Button btnOutDir;
        private System.Windows.Forms.Button btnStart;
    }
}

