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
    public partial class DepartmentsForm : Form
    {
        public DepartmentsForm()
        {
            InitializeComponent();
        }

        private void DepartmentsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var groups = new List<Group>();

                var cmd = new SqlCommand("Select * from Группы", sql);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    groups.Add(new Group
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1]
                    });
                }
                reader.Close();

                cmd = new SqlCommand("Select * from Отделы", sql);
                reader = cmd.ExecuteReader();
                dataGridView1.SuspendLayout();
                dataGridView1.Rows.Clear();
                while (reader.Read())
                {
                    var g = ((string)reader[2]).Split(',').Select(t => int.Parse(t));
                    dataGridView1.Rows.Add(reader[0], reader[1], reader[2], string.Join(", ", groups.Where(t => g.Any(t2 => t.ID == t2)).Select(t => t.Name)));
                }
                reader.Close();
                dataGridView1.ResumeLayout();

                sql.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new AddEditDepartment
            {
                Text = "Добавить отдел"
            };
            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Insert into Отделы values (@Name, @Groups)", sql);
                    cmd.Parameters.AddWithValue("@Name", form.nameBox.Text);
                    cmd.Parameters.AddWithValue("@Groups", string.Join(",", form.listBox1.Items.Cast<Group>().Select(t => t.ID)));
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

            var form = new AddEditDepartment(dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Split(',').Select(t => int.Parse(t)).ToArray())
            {
                Text = "Изменить отдел"
            };
            form.applyButton.Text = "Применить";

            var d = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            form.nameBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Update Отделы set Название = @Name, Коды = @Groups where ID = @ID", sql);
                    cmd.Parameters.AddWithValue("@ID", d);
                    cmd.Parameters.AddWithValue("@Name", form.nameBox.Text);
                    cmd.Parameters.AddWithValue("@Groups", string.Join(",", form.listBox1.Items.Cast<Group>().Select(t => t.ID)));
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
                        var cmd = new SqlCommand("Delete from Отделы where ID = @ID", sql);
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
