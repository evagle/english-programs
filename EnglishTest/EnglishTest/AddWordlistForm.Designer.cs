namespace EnglishTest
{
    partial class AddWordlistForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbZhName = new System.Windows.Forms.TextBox();
            this.tbEnName = new System.Windows.Forms.TextBox();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.benOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "词表中文名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "词表英文名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "词表路径：";
            // 
            // tbZhName
            // 
            this.tbZhName.Location = new System.Drawing.Point(134, 20);
            this.tbZhName.Name = "tbZhName";
            this.tbZhName.Size = new System.Drawing.Size(174, 20);
            this.tbZhName.TabIndex = 3;
            // 
            // tbEnName
            // 
            this.tbEnName.Location = new System.Drawing.Point(134, 58);
            this.tbEnName.Name = "tbEnName";
            this.tbEnName.Size = new System.Drawing.Size(174, 20);
            this.tbEnName.TabIndex = 4;
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(100, 101);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(133, 20);
            this.tbPath.TabIndex = 5;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(239, 97);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(69, 26);
            this.btnSelectFile.TabIndex = 6;
            this.btnSelectFile.Text = "选择文件";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(134, 151);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // benOK
            // 
            this.benOK.Location = new System.Drawing.Point(230, 151);
            this.benOK.Name = "benOK";
            this.benOK.Size = new System.Drawing.Size(78, 23);
            this.benOK.TabIndex = 8;
            this.benOK.Text = "确定";
            this.benOK.UseVisualStyleBackColor = true;
            this.benOK.Click += new System.EventHandler(this.benOK_Click);
            // 
            // AddWordlistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 193);
            this.Controls.Add(this.benOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.tbEnName);
            this.Controls.Add(this.tbZhName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddWordlistForm";
            this.Text = "AddWordlistForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbZhName;
        private System.Windows.Forms.TextBox tbEnName;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button benOK;
    }
}