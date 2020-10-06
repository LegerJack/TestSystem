using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Тренировочное_тестирование
{
    public partial class Form1 : Form
    {
        public static string connetcionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='Z:\МДК 02.02\TrainingTest\Тренировочное тестирование\Database1.mdf';Integrated Security = True";
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control tb in this.tabPage2.Controls)
            {
                if (tb is TextBox)
                {
                    tb.Text = "";
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox9.UseSystemPasswordChar == false)
                textBox9.UseSystemPasswordChar = true;
            else
                textBox9.UseSystemPasswordChar = false;
        }

        public static string login;
        private void button3_Click(object sender, EventArgs e)
        {
            login = textBox8.Text;
            string passw = textBox9.Text;

            string query = $"SELECT [Имя], [Фамилия] FROM [Пользователь] " +
                   $"WHERE [Логин] = '{login}' AND [Пароль] = '{passw}'";
            using (SqlConnection connect = new SqlConnection(connetcionString))
            {
                connect.Open();

                SqlCommand cmd = new SqlCommand(query, connect);
                SqlDataReader read = cmd.ExecuteReader();

                if (read.HasRows)
                {

                    while (read.Read())
                    {
                        MessageBox.Show("Добро пожаловать! \n" + read.GetString(0) + " " + read.GetString(1));
                    }

                    Form2 fm2 = new Form2();
                    fm2.Show();
                    this.Hide();
                }

                else
                {
                    label10.Text = "Неправильный логин или пароль";
                }
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] queryValue = new string[5];

            for (int i = 0; i < queryValue.Length; i++)
            {
                queryValue[i] = (this.tabPage2.Controls["textBox" + (i + 2).ToString()] as TextBox).Text;
            }

            MessageBox.Show($"{queryValue[3]} {queryValue[4]} {queryValue[1]} {queryValue[0]} {queryValue[2]}");

            if(queryValue[4] == textBox7.Text)
            {
                string query = "INSERT INTO [Пользователь]" +
                                $"VALUES ('{queryValue[3]}', '{queryValue[4]}', '{queryValue[1]}', '{queryValue[0]}', '{queryValue[2]}')";
                using (SqlConnection connect = new SqlConnection(connetcionString))
                {
                    connect.Open();
                    SqlCommand cmd = new SqlCommand(query, connect);
                    try
                    {
                        cmd.ExecuteNonQuery();

                    }
                    catch
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
            }

            else if (queryValue[4].Length < 6)
            {
                MessageBox.Show("Пароль не может быть менее 6 символов", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка ввода",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
    }
}
