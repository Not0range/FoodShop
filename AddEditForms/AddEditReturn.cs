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
    public partial class AddEditReturn : Form
    {
        public AddEditReturn()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("Select t1.*, t2.Название from Поставки t1 inner join Товары t2 on t1.[ID товара] = t2.ID", sql);
                var reader = cmd.ExecuteReader();
                productBox.SuspendLayout();
                productBox.Items.Clear();
                while (reader.Read())
                {
                    var product = new Supply
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[5],
                        Date = (DateTime)reader[2]
                    };
                    productBox.Items.Add(product);
                }
                reader.Close();
                productBox.ResumeLayout();

                sql.Close();
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (productBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(reasonBox.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
