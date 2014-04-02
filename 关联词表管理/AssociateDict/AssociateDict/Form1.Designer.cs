namespace AssociateDict
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
            this.tbAssociate = new System.Windows.Forms.TextBox();
            this.btnAssociate = new System.Windows.Forms.Button();
            this.btnAbnormal = new System.Windows.Forms.Button();
            this.tbAbnormal = new System.Windows.Forms.TextBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbAssociate
            // 
            this.tbAssociate.Location = new System.Drawing.Point(103, 32);
            this.tbAssociate.Name = "tbAssociate";
            this.tbAssociate.Size = new System.Drawing.Size(280, 20);
            this.tbAssociate.TabIndex = 0;
            // 
            // btnAssociate
            // 
            this.btnAssociate.Location = new System.Drawing.Point(405, 30);
            this.btnAssociate.Name = "btnAssociate";
            this.btnAssociate.Size = new System.Drawing.Size(75, 23);
            this.btnAssociate.TabIndex = 1;
            this.btnAssociate.Text = "选择文件";
            this.btnAssociate.UseVisualStyleBackColor = true;
            this.btnAssociate.Click += new System.EventHandler(this.btnAssociate_Click);
            // 
            // btnAbnormal
            // 
            this.btnAbnormal.Location = new System.Drawing.Point(405, 75);
            this.btnAbnormal.Name = "btnAbnormal";
            this.btnAbnormal.Size = new System.Drawing.Size(75, 23);
            this.btnAbnormal.TabIndex = 3;
            this.btnAbnormal.Text = "选择文件夹";
            this.btnAbnormal.UseVisualStyleBackColor = true;
            this.btnAbnormal.Click += new System.EventHandler(this.btnAbnormal_Click);
            // 
            // tbAbnormal
            // 
            this.tbAbnormal.Location = new System.Drawing.Point(103, 78);
            this.tbAbnormal.Name = "tbAbnormal";
            this.tbAbnormal.Size = new System.Drawing.Size(280, 20);
            this.tbAbnormal.TabIndex = 2;
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(405, 124);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(75, 23);
            this.btnOutput.TabIndex = 5;
            this.btnOutput.Text = "选择文件夹";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(103, 127);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(280, 20);
            this.tbOutput.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "输出文件夹";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "不规则词表";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "旧关联词表";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(308, 188);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(405, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 248);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.btnAbnormal);
            this.Controls.Add(this.tbAbnormal);
            this.Controls.Add(this.btnAssociate);
            this.Controls.Add(this.tbAssociate);
            this.Name = "Form1";
            this.Text = "关联词表管理程序";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAssociate;
        private System.Windows.Forms.Button btnAssociate;
        private System.Windows.Forms.Button btnAbnormal;
        private System.Windows.Forms.TextBox tbAbnormal;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}

