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
    public partial class AdminPanel : Form
    {

        DataBase DataBase = new DataBase();


        public AdminPanel()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;//открытие новой формы по центру экрана
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_user", "ID");
            dataGridView1.Columns.Add("Login", "Login");
            dataGridView1.Columns.Add("Password", "Password");
            var checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "Adminka";
            dataGridView1.Columns.Add(checkColumn);
        }

        private void ReadSingleRows(IDataRecord record)
        {
           dataGridView1.Rows.Add(record.GetInt32(0), record.GetString(1),  record.GetString(2), record.GetBoolean(3));
        }

        private void RefreshDataGrid()
        {
            dataGridView1.Rows.Clear();
            string Query = $"SELECT * FROM users";
            SqlCommand cmd = new SqlCommand(Query, DataBase.GetConnection());
            DataBase.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRows(reader);
            }
            reader.Close();

            DataBase.closeConnection();
        }

        private void saveUsers_Click(object sender, EventArgs e)
        {
            DataBase.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                var isadmin = dataGridView1.Rows[index].Cells[3].Value.ToString();

                var ChangeQuery = $"UPDATE users SET is_admin = '{isadmin}' WHERE id_user = '{id}'";

                var command = new SqlCommand(ChangeQuery, DataBase.GetConnection());
                command.ExecuteNonQuery();
            }

            DataBase.closeConnection();

            RefreshDataGrid();
        }

        private void deleteUsers_Click(object sender, EventArgs e)
        {
            DataBase.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            var delete = $"DELETE FROM users WHERE id_user = '{id}'";
            var command = new SqlCommand(delete, DataBase.GetConnection());
            command.ExecuteNonQuery();
            DataBase.closeConnection();

            RefreshDataGrid();
        }
    }
}
