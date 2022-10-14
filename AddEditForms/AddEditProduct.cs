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

namespace FoodShop
{
    public partial class AddEditProduct : Form
    {
        public AddEditProduct()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("Select * from Группы", sql);
                var reader = cmd.ExecuteReader();
                groupBox.SuspendLayout();
                groupBox.Items.Clear();
                while (reader.Read())
                {
                    var group = new Group
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1]
                    };
                    groupBox.Items.Add(group);
                }
                reader.Close();
                groupBox.ResumeLayout();

                sql.Close();
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) ||
                string.IsNullOrWhiteSpace(unitBox.Text) ||
                groupBox.SelectedItem == null)
            {
                MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;

        }
    }
}
