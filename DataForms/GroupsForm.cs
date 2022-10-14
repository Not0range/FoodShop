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
    public partial class GroupsForm : Form
    {
        public GroupsForm()
        {
            InitializeComponent();
        }

        private void Groups_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("Select * from Группы", sql);
                var reader = cmd.ExecuteReader();
                listBox1.SuspendLayout();
                listBox1.Items.Clear();
                while (reader.Read())
                {
                    var group = new Group
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1]
                    };
                    listBox1.Items.Add(group);
                }
                reader.Close();
                listBox1.ResumeLayout();

                sql.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new AddEditGroup
            {
                Text = "Добавить группу"
            };

            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Insert into Группы values (@Name)", sql);
                    cmd.Parameters.AddWithValue("@Name", form.nameBox.Text);
                    cmd.ExecuteNonQuery();

                    sql.Close();
                }
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            var form = new AddEditGroup
            {
                Text = "Изменить группу"
            };
            form.applyButton.Text = "Применить";
            var g = listBox1.SelectedItem as Group;
            form.nameBox.Text = g.Name;

            if (form.ShowDialog() == DialogResult.OK)
            {
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    var cmd = new SqlCommand("Update Группы set Название = @Name where ID = @ID", sql);
                    cmd.Parameters.AddWithValue("@ID", g.ID);
                    cmd.Parameters.AddWithValue("@Name", form.nameBox.Text);
                    cmd.ExecuteNonQuery();

                    sql.Close();
                }
                LoadData();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem == null)
                return;

            if (MessageBox.Show("Вы действительно желаете удалить выбранную запись?", "Подтверждение", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var g = listBox1.SelectedItem as Group;
                using (var sql = new SqlConnection(Program.sql.ConnectionString))
                {
                    sql.Open();

                    try
                    {
                        var cmd = new SqlCommand("Delete from Группы where ID = @ID", sql);
                        cmd.Parameters.AddWithValue("@ID", g.ID);
                        cmd.ExecuteNonQuery();
                        listBox1.Items.Remove(g);
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
    }

    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
