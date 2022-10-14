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
    public partial class ProductsForm : Form
    {
        public ProductsForm()
        {
            InitializeComponent();
        }

        private void ProductsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("Select t1.*, t2.Название as Группа from Товары t1 inner join Группы t2 on t1.[ID группы] = t2.ID", sql);
                var reader = cmd.ExecuteReader();
                dataGridView1.SuspendLayout();
                dataGridView1.Rows.Clear();
                while (reader.Read())
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4], reader[6], reader[5]);
                reader.Close();
                dataGridView1.ResumeLayout();

                sql.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new AddEditProduct
            {
                Text = "Добавить товар"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Insert into Товары values (@Name, @Price, @Unit, @Group, @Expiration)", sql);
                    cmd.Parameters.AddWithValue("@Name", form.nameBox.Text);
                    cmd.Parameters.AddWithValue("@Price", form.priceBox.Value);
                    cmd.Parameters.AddWithValue("@Unit", form.unitBox.Text);
                    cmd.Parameters.AddWithValue("@Group", (form.groupBox.SelectedItem as Group).ID);
                    cmd.Parameters.AddWithValue("@Expiration", (int)form.expirationBox.Value);
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

            var form = new AddEditProduct
            {
                Text = "Изменить товар"
            };
            form.applyButton.Text = "Применить";

            var p = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            form.nameBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            form.priceBox.Value = decimal.Parse(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            form.unitBox.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            form.groupBox.SelectedItem = form.groupBox.Items.Cast<Group>().First(t => t.ID == int.Parse(dataGridView1.SelectedRows[0].Cells[4].Value.ToString()));
            form.expirationBox.Value = int.Parse(dataGridView1.SelectedRows[0].Cells[6].Value.ToString());

            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Update Товары set Название = @Name, Стоимость =  @Price, [Ед. измерения] = @Unit, [ID группы] = @Group, [Срок годности] = @Expiration where ID = @ID", sql);
                    cmd.Parameters.AddWithValue("@ID", p);
                    cmd.Parameters.AddWithValue("@Name", form.nameBox.Text);
                    cmd.Parameters.AddWithValue("@Price", form.priceBox.Value);
                    cmd.Parameters.AddWithValue("@Unit", form.unitBox.Text);
                    cmd.Parameters.AddWithValue("@Group", (form.groupBox.SelectedItem as Group).ID);
                    cmd.Parameters.AddWithValue("@Expiration", (int)form.expirationBox.Value);
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
                        var cmd = new SqlCommand("Delete from Товары where ID = @ID", sql);
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
