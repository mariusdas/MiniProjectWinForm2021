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

namespace WinFormMariusBudrauskas2021
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            LoadDataGrid();
        }

        public void LoadDataGrid()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-K8A9DVA\\SQLEXPRESS;Initial Catalog=WinFormDatabase;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from Users", con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    dt.Load(reader);
                    dt.Columns[0].ColumnName = "ID";
                    dt.Columns[1].ColumnName = "Name";
                    dt.Columns[2].ColumnName = "Last Name";
                    dt.Columns[3].ColumnName = "Phone";
                    dt.Columns[4].ColumnName = "Date Time";
                }
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            var addDialogForm = new DialogForm(DialogForm.DialogType.Add, null);
            addDialogForm.Show();
            addDialogForm.FormClosed += AddDialogForm_FormClosed;
        }

        private void AddDialogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoadDataGrid();
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            if(this.dataGridView1.SelectedRows.Count == 1)
            {
                var row = this.dataGridView1.SelectedRows.Cast<DataGridViewRow>().First();

                var user = new User();
                user.ID = int.Parse(row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "ID").Value.ToString());
                user.Name = row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "Name").Value.ToString();
                user.LastName = row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "Last Name").Value.ToString();
                user.Phone = row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "Phone").Value.ToString();
                user.Date = DateTime.Parse(row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "Date Time").Value.ToString());

                var updateDialogForm = new DialogForm(DialogForm.DialogType.Update, user);
                updateDialogForm.Show();
                updateDialogForm.FormClosed += AddDialogForm_FormClosed;
            }
            else
            {
                MessageBox.Show("Please select a row","Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 1)
            {
                var row = this.dataGridView1.SelectedRows.Cast<DataGridViewRow>().First();
                int userId = int.Parse(row.Cells.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == "ID").Value.ToString());

                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-K8A9DVA\\SQLEXPRESS;Initial Catalog=WinFormDatabase;Integrated Security=True"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("DeleteUser", con))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", userId);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            LoadDataGrid();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Not Deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
