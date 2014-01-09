using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EnglishTest
{
    public partial class DeleteWordListForm : Form
    {
        public DeleteWordListForm()
        {
            InitializeComponent(); 
            for (int i = 0; i < GlobalData.WordList.Count; i++)
            {
                this.checkedListBox1.Items.Add(GlobalData.WordList[i].Name);            
            }        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string name = (string)checkedListBox1.Items[i];
                    for (int j = 0; j < GlobalData.WordList.Count; j++)
                    {
                        if (GlobalData.WordList[j].Name.Equals(name))
                        {
                            GlobalData.WordList.RemoveAt(j);
                            break;
                        }
                    }
                }
            }
           
            MessageBox.Show("删除词表成功");
            this.Dispose();
        }

    }
}
