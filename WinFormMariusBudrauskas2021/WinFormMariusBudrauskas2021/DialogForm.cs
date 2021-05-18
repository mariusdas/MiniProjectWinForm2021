using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WinFormMariusBudrauskas2021
{
    public partial class DialogForm : Form
    {
        public enum DialogType
        {
            Add,
            Update
        }

        public DialogType Type;

        public User User;

        public DialogForm(DialogType type, User user)
        {
            InitializeComponent();
            Type = type;
            User = user;
            if (Type == DialogType.Add)
            {
                this.dateTimePicker.Value = DateTime.Now;
            }
            else if(Type == DialogType.Update && User != null)
            {
                this.txtName.Text = User.Name;
                this.txtLastName.Text = User.LastName;
                this.txtPhone.Text = User.Phone;
                this.dateTimePicker.Value = User.Date;
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if(Type == DialogType.Add)
            {
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-K8A9DVA\\SQLEXPRESS;Initial Catalog=WinFormDatabase;Integrated Security=True"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("AddUser", con))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Name",txtName.Text);
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            cmd.Parameters.AddWithValue("@Date", dateTimePicker.Value.ToString());
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Close();                           
                        }
                        catch(Exception)
                        {
                            MessageBox.Show("Not Added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if(Type == DialogType.Update)
            {
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-K8A9DVA\\SQLEXPRESS;Initial Catalog=WinFormDatabase;Integrated Security=True"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateUser", con))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", User.ID);
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            cmd.Parameters.AddWithValue("@Date", dateTimePicker.Value.ToString());
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Not Updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
