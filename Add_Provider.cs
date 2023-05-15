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
    public partial class Add_Provider : Form
    {
        DataBase DataBase = new DataBase();

        public Add_Provider()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;/*открытие новой формы по центру экрана*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase.openConnection();
            var NameProvidera = textBox1.Text;
            var telephone = textBox2.Text;
            var adddress = textBox3.Text;
            var zapros = $"INSERT INTO provider(NameProvidera,telephone,Addres) VALUES ('{NameProvidera}', '{telephone}', '{adddress}')"; //Вставляем в БД данные с textboxов
            var command = new SqlCommand(zapros, DataBase.GetConnection());
            command.ExecuteNonQuery(); //выполняет sql-выражение и возвращает количество измененных записей.Подходит для sql - выражений INSERT, UPDATE, DELETE, CREATE.
            MessageBox.Show("Поставщик добавлен!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Providers prov = new Providers();
            this.Hide();
            prov.ShowDialog();
            this.Hide();
        }
    }
}
