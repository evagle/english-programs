using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrammarRecognition
{
    public partial class ShowText : Form
    {
        private List<String> content;
        public ShowText(List<String> content)
        {
            InitializeComponent();
            this.content = content;
            rtbText.Font = new System.Drawing.Font("宋体", 12);
            foreach (String s in content)
            {
                rtbText.Text = rtbText.Text + s + "\n\n";
            }
            
        }

        private void rtbText_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
