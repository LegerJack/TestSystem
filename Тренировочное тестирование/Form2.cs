using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Тренировочное_тестирование
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        public static int numbOfTest;

        private void button3_Click(object sender, EventArgs e)
        {
            numbOfTest = (int)comboBox1.SelectedValue;
            Form3 fm3 = new Form3();
            fm3.Show();
            this.Hide();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.История". При необходимости она может быть перемещена или удалена.
            this.историяTableAdapter.Fill(this.database1DataSet.История);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.Тест". При необходимости она может быть перемещена или удалена.
            this.тестTableAdapter.Fill(this.database1DataSet.Тест);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form1")
                {
                    frm.Show();
                }
            }
            this.Close();
        }
    }
}
