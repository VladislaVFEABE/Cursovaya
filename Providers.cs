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

    enum RowStatee
    {
        Existed,
        New,
        Modifed,
        ModifedNew,
        Deleted
    }


    public partial class Providers : Form
    {

        DataBase DataBase = new DataBase();
        public Providers()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;//открытие новой формы по центру экрана
        }

        private void Providers_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_providera", "ID_Поставщика");
            dataGridView1.Columns.Add("NameProvidera", "Имя_Поставщика");
            dataGridView1.Columns.Add("telephone", "Телефон");
            dataGridView1.Columns.Add("Addres", "Адрес_Поставщика");
        }
        private void ReadSingleRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3));
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string querysting = $"SELECT * FROM provider";
            SqlCommand command = new SqlCommand(querysting, DataBase.GetConnection());
            DataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRows(dgw, reader);
            }
            reader.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Add_Provider prov = new Add_Provider();
            this.Hide();
            prov.ShowDialog();
            this.Hide();
        }
    }
}
