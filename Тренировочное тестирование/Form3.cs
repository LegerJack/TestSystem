using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Тренировочное_тестирование
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form2")
                {
                    frm.Show();
                }
                else
                {
                    Form2 f2 = new Form2();
                }
            }
        }

        private void RadioCheck()
        {
            if (radioButton1.Checked)
            {
                usAnswer = 1;
            }
            else if (radioButton2.Checked)
            {
                usAnswer = 2;
            }
            else if (radioButton3.Checked)
            {
                usAnswer = 3;
            }

            if (usAnswer == answer)
            {
                points++;
            }
        }

        public static int numOfQuestion = 1;
        public int answer = 0; 

        private void Form3_Load(object sender, EventArgs e)
        {
            numOfQuestion = 1;

            label3.Text = minuts + ":" + sec;
            timer1.Start();

            string query = "SELECT [Название], [Вопрос], [Вариант 1], [Вариант 2], [Вариант 3], [Ответ] " +
                $"FROM [Вопрос], [Тест] WHERE [Номер] = [Номер теста] AND [Номер теста] = '{Form2.numbOfTest}' AND [Номер вопроса] = '{numOfQuestion}'";

            string query2 = $"SELECT COUNT(*) FROM [Вопрос] WHERE [Номер теста] = '{Form2.numbOfTest}'";

            using (SqlConnection connect = new SqlConnection(Form1.connetcionString))
            {
                connect.Open();

                SqlCommand cmd = new SqlCommand(query2, connect);
                object count = cmd.ExecuteScalar();

                label4.Text = numOfQuestion + "/" + count.ToString();
                connect.Close();

                connect.Open();

                cmd = new SqlCommand(query, connect);
                SqlDataReader read = cmd.ExecuteReader();

                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        label1.Text = read.GetString(0);
                        label2.Text = read.GetString(1);

                        answer = read.GetInt32(5);

                        radioButton1.Text = read.GetString(2);
                        radioButton2.Text = read.GetString(3);
                        radioButton3.Text = read.GetString(4);
                    }

                }
            }
        }

        public int points = 0;
        public int usAnswer = 0;
        public int count;
        private void button1_Click(object sender, EventArgs e)
        {

            RadioCheck();
            
            numOfQuestion++;

            string query = "SELECT [Вопрос], [Вариант 1], [Вариант 2], [Вариант 3], [Ответ] " +
               $"FROM [Вопрос] WHERE [Номер теста] = '{Form2.numbOfTest}' AND [Номер вопроса] = '{numOfQuestion}'";

            string query2 = $"SELECT COUNT(*) FROM [Вопрос] WHERE [Номер теста] = '{Form2.numbOfTest}'";

            usAnswer = 0;

            using (SqlConnection connect = new SqlConnection(Form1.connetcionString))
            {
                connect.Open();

                SqlCommand cmd = new SqlCommand(query2, connect);
                count = Convert.ToInt32(cmd.ExecuteScalar());

                label4.Text = numOfQuestion + "/" + count.ToString();

                if (numOfQuestion == count)
                {
                    button1.Visible = false;
                    button2.Visible = true;
                    button2.Location = new Point(369, 372);

                    numOfQuestion = count;
                }

                connect.Close();

                connect.Open();

                cmd = new SqlCommand(query, connect);
                SqlDataReader read = cmd.ExecuteReader();

                if (read.HasRows)
                {
                    while (read.Read())
                    {
                        label2.Text = read.GetString(0);

                        answer = read.GetInt32(4);

                        radioButton1.Text = read.GetString(1);
                        radioButton2.Text = read.GetString(2);
                        radioButton3.Text = read.GetString(3);
                    }

                }
                connect.Close();

            }
        }
        public int minuts = 10;
        public int sec = 0;

        private void EndTime()
        {
            int time = 10;
            if (sec < 50)
                alltime = (time - minuts) + ":" + (60 - sec);
            else
                alltime = (time - minuts) + ":0" + (60 - sec);


            curDate = DateTime.Now;

            MessageBox.Show(alltime + " " + curDate.ToString("d"));

            string query = "INSERT INTO [История] ([Логин],[Номер теста],[Результат],[Кол_баллов],[Время],[Дата]) " +
                    $"VALUES('{Form1.login}', '{Form2.numbOfTest}', '{result}', '{points}', '{alltime}', '{curDate.ToString("d")}')";

            using (SqlConnection connect = new SqlConnection(Form1.connetcionString))
            {
                connect.Open();

                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.ExecuteNonQuery();
            }
        }

        public string alltime;
        public DateTime curDate;
        private void timer1_Tick(object sender, EventArgs e)
        {

            label3.Text = minuts + ":0" + sec;
            if (sec == 0)
            {
                minuts--;
                sec = 59;
                label3.Text = minuts + ":" + sec;
            }
            else if (minuts == 0 && sec == 0)
            {
                this.Close();
                timer1.Stop();


                EndTime();
            }
            else
            {
                sec--;
                if (sec < 10)
                {
                    label3.Text = minuts + ":0" + sec;
                }
                else
                {
                    label3.Text = minuts + ":" + sec;
                }
            }
        }

        public int result;

        private void button2_Click(object sender, EventArgs e)
        {
            RadioCheck();

            if (points == (int)count)
                result = 5;
            else if (points == (int)count - 1)
                result = 4;
            else if (points == ((int)count / 2))
                result = 3;
            else result = 2;
            
            EndTime();
            
            this.Close();
        }
    }
}
