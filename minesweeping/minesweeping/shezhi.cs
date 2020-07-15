using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace saolei
{
    public partial class shezhi : Form
    {

        private static int Leishuliang=50;
        public shezhi()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9')&&!(e.KeyChar=='\b'))
            { e.Handled = true;}
            
        }

        private void shezhi_Load(object sender, EventArgs e)
        {
            textBox1.Text = Leishuliang.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                return;
            }
            if (int.Parse(textBox1.Text) > 400 || int.Parse(textBox1.Text) < 10)
            {
                MessageBox.Show("请输入10---400之内的数字");
                textBox1.Text = string.Empty;
            }
            else
            {
                Program.form.Leishu= int.Parse(textBox1.Text);
                Leishuliang = int.Parse(textBox1.Text); 
                Program.form.Fuyuan();
                this.Close();
              
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.form.Leishu = 50;
            Leishuliang = 50;
            Program.form.Fuyuan();
             this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
