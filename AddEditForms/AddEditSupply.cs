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
    public partial class AddEditSupply : Form
    {
        public AddEditSupply()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("Select * from Товары", sql);
                var reader = cmd.ExecuteReader();
                productBox.SuspendLayout();
                productBox.Items.Clear();
                while (reader.Read())
                {
                    var product = new Product
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1]
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
                string.IsNullOrWhiteSpace(providerBox.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;

        }
    }

    class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
