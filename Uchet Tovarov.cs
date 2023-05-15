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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;


namespace TestReload
{
    enum RowState
    {
        Existed,
        New,
        Modifed,
        ModifedNew,
        Deleted
    }

    public partial class Uchet_Tovarov : Form
    {
        private readonly checkUser _user;
        DataBase DataBase = new DataBase();


        int selectedRow;


        public Uchet_Tovarov(checkUser user)
        {
            
            InitializeComponent();
            _user = user;
            StartPosition = FormStartPosition.CenterScreen;//открытие новой формы по центру экрана
        }


        private void isAdmin()
        {

            toolStripButton1.Enabled = _user.IsAdmin;
            new_query.Enabled = _user.IsAdmin;
            delete_query.Enabled = _user.IsAdmin;
            redact_query.Enabled = _user.IsAdmin;
            save_query.Enabled = _user.IsAdmin;
            toolStripButton4.Enabled = _user.IsAdmin;
            toolStripLabel1.Enabled = _user.IsAdmin;
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id","id");
            dataGridView1.Columns.Add("type_of", "Имя товара");
            dataGridView1.Columns.Add("count_of", "Количество");
            dataGridView1.Columns.Add("NameProvidera", "Поставщик");
            dataGridView1.Columns.Add("price","Цена");
            dataGridView1.Columns.Add("IsNew", String.Empty);
        }

        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void ReadSingleRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetString(3), record.GetInt32(4), RowState.ModifedNew);
        }

        private void RefreshDataGrid(DataGridView dgw)//метод вывода данных в таблицу из БД
        {
            ClearFields();
            dgw.Rows.Clear();

            string querysting = $"SELECT * FROM test_db";
            SqlCommand command = new SqlCommand(querysting, DataBase.GetConnection());
            DataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRows(dgw, reader);
            }
            reader.Close();
        }

        private void save_query_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Uchet_Tovarov_Load(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = $"{_user.Login}, Role:{_user.Status}";
            isAdmin();
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if(e.RowIndex >= 0) 
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                textBox4.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void new_query_Click(object sender, EventArgs e)
        {
            Add_Tovar add = new Add_Tovar();
            this.Hide();
            add.ShowDialog();
            this.Show();
        }

        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string searchString = $"SELECT * FROM test_db WHERE CONCAT(id,type_of,count_of,NameProvidera,price) LIKE '%" + textBox6.Text + "%'";
            SqlCommand com = new SqlCommand(searchString, DataBase.GetConnection());
            DataBase.openConnection();
            SqlDataReader read = com.ExecuteReader();
            while (read.Read())
            {
                ReadSingleRows(dgw, read);
            }
            read.Close();
        }


        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void DeleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;
            if(dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
        }

        private void Update()
        {
            DataBase.openConnection();
            for(int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[5].Value;

                if(rowState == RowState.Existed)
                {
                    continue;
                }

                if(rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"DELETE FROM test_db WHERE id={id}";
                    var command = new SqlCommand(deleteQuery, DataBase.GetConnection());
                    command.ExecuteNonQuery();
                }

                if(rowState == RowState.Modifed)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var type = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var count = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var postavshik = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var price = dataGridView1.Rows[index].Cells[4].Value.ToString();

                    var changeQuery = $"UPDATE test_db set type_of = '{type}', count_of = '{count}', NameProvidera = '{postavshik}', price = '{price}' WHERE id = '{id}'";

                    var command = new SqlCommand(changeQuery, DataBase.GetConnection());
                    command.ExecuteNonQuery();
                }

            }
            DataBase.closeConnection();
        }



        private void delete_query_Click(object sender, EventArgs e)
        {
            DeleteRow();
            ClearFields();
        }

        private void save_query_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = textBox1.Text;
            var type = textBox2.Text;
            var count = textBox3.Text;
            var postavshik = textBox4.Text;
            int price;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                if (int.TryParse(textBox5.Text, out price))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, type, count, postavshik, price);
                    dataGridView1.Rows[selectedRowIndex].Cells[5].Value = RowState.Modifed;
                }
                else
                {
                    MessageBox.Show("Цена должна иметь числовой формат");
                }

            }
        }

        private void redact_query_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AdminPanel admin = new AdminPanel();
            this.Hide();
            admin.ShowDialog();
            this.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Info info = new Info();
            this.Hide();
            info.ShowDialog();
            this.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Loginka log = new Loginka();
            this.Hide();
            log.ShowDialog();
            this.Show();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Providers prov = new Providers();
            this.Hide();
            prov.ShowDialog();
            this.Hide();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            object Missing = Type.Missing;
            Workbook workbook = excel.Workbooks.Add(Missing);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            int StartCol = 1;
            int StartRow = 1;
            for(int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                Range myRange = (Range)sheet1.Cells[StartRow, StartCol + j];
                myRange.Value2 = dataGridView1.Columns[j].HeaderText;
            }
            StartRow++;
            for(int i =0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    Range myRange = (Range)sheet1.Cells[StartRow + i, StartCol + j];
                    myRange.Value2 = dataGridView1[j, i].Value == null ? "" : dataGridView1[j, i].Value;
                    myRange.Select();
                }
            }
        }
    }
}
