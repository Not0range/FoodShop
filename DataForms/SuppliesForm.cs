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
    public partial class SuppliesForm : Form
    {
        public SuppliesForm()
        {
            InitializeComponent();
        }

        private void SuppliesForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("Select t1.*, t2.Название as Товар from Поставки t1 inner join Товары t2 on t1.[ID товара] = t2.ID", sql);
                var reader = cmd.ExecuteReader();
                dataGridView1.SuspendLayout();
                dataGridView1.Rows.Clear();
                while (reader.Read())
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[5], reader[2], reader[3], reader[4]);
                reader.Close();
                dataGridView1.ResumeLayout();

                sql.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new AddEditSupply
            {
                Text = "Добавить поставку"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Insert into Поставки values (@Product, @Date, @Provider, @Count)", sql);
                    cmd.Parameters.AddWithValue("@Product", (form.productBox.SelectedItem as Product).ID);
                    cmd.Parameters.AddWithValue("@Date", form.datetimeBox.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Provider", form.providerBox.Text);
                    cmd.Parameters.AddWithValue("@Count", (int)form.countBox.Value);
                    cmd.ExecuteNonQuery();

                    sql.Close();
                }
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            var form = new AddEditSupply
            {
                Text = "Добавить поставку"
            };
            form.applyButton.Text = "Применить";

            var s = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            var p = int.Parse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
            form.productBox.SelectedItem = form.productBox.Items.Cast<Product>().First(t => t.ID == p);
            form.datetimeBox.Value = DateTime.Parse(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
            form.providerBox.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            form.countBox.Value = int.Parse(dataGridView1.SelectedRows[0].Cells[5].Value.ToString());

            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Update Поставки set [ID товара] = @Product, [Дата и время поставки] = @Date, Поставщик = @Provider, Количество = @Count where ID = @ID", sql);
                    cmd.Parameters.AddWithValue("@ID", s);
                    cmd.Parameters.AddWithValue("@Product", (form.productBox.SelectedItem as Product).ID);
                    cmd.Parameters.AddWithValue("@Date", form.datetimeBox.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@Provider", form.providerBox.Text);
                    cmd.Parameters.AddWithValue("@Count", (int)form.countBox.Value);
                    cmd.ExecuteNonQuery();

                    sql.Close();
                }
                LoadData();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("Вы действительно желаете удалить выбранную запись?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var d = dataGridView1.SelectedRows[0];
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    try
                    {
                        var cmd = new SqlCommand("Delete from Поставки where ID = @ID", sql);
                        cmd.Parameters.AddWithValue("@ID", int.Parse(d.Cells[0].Value.ToString()));
                        cmd.ExecuteNonQuery();
                        dataGridView1.Rows.Remove(d);
                    }
                    catch (SqlException)
                    {
                        MessageBox.Show("Данную запись удалить невозможно, так как она связана с другой записью", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    sql.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
