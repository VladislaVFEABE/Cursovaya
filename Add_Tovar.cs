using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TestReload
{


    public partial class Add_Tovar : Form
    {
        //создание экземпляра класса
        DataBase DataBase = new DataBase();

        public Add_Tovar()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;/*открытие новой формы по центру экрана*/

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //открываем соединение с БД
            DataBase.openConnection();
            var type = textBox1.Text; /*заносим переменную в textBox*/
            var count = textBox2.Text;
            var postavshik = comboBox1.Text;
            int price;
            if(int.TryParse(textBox4.Text, out price))  //проверка поля textBox4 на тип данных int
            {
                var addQuery = $"INSERT INTO test_db(type_of,count_of,NameProvidera,price) VALUES ('{type}', '{count}', '{postavshik}', '{price}')"; //Вставляем в БД данные с textboxов
                var command = new SqlCommand(addQuery, DataBase.GetConnection());
                command.ExecuteNonQuery(); //выполняет sql-выражение и возвращает количество измененных записей.Подходит для sql - выражений INSERT, UPDATE, DELETE, CREATE.

                 MessageBox.Show("Товар добавлен!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Неправильный тип данных в одном из полей", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            DataBase.closeConnection();
        }

        private void Add_Tovar_Load(object sender, EventArgs e)
        {
            LoadComboBox();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadComboBox()
        {
            SqlConnection sql = new SqlConnection(@"Data Source=DESKTOP-081C48G;Initial Catalog=test;Integrated Security=True");
            sql.Open();
            SqlCommand sc = new SqlCommand("select NameProvidera from provider", sql);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("id_providera", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "NameProvidera";
            comboBox1.DataSource = dt;
            sql.Close();

        }
    }
}
