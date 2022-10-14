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
    public partial class AddEditDepartment : Form
    {
        List<int> ids = new List<int>();
        public AddEditDepartment(int[] ids = null)
        {
            InitializeComponent();
            if (ids != null)
                this.ids.AddRange(ids);
        }

        private void AddEditDepartment_Load(object sender, EventArgs e)
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
                comboBox1.SuspendLayout();
                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    var group = new Group
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1]
                    };
                    comboBox1.Items.Add(group);
                }
                reader.Close();
                comboBox1.ResumeLayout();

                sql.Close();
            }
            listBox1.Items.AddRange(comboBox1.Items.Cast<Group>().Where(t => ids.Any(t2 => t2 == t.ID)).ToArray());
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text) || listBox1.Items.Count == 0)
            {
                MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && !listBox1.Items.Cast<Group>().Any(t => t == comboBox1.SelectedItem as Group))
            {
                listBox1.Items.Add(comboBox1.SelectedItem);
                comboBox1.SelectedItem = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                listBox1.Items.Remove(listBox1.SelectedItem);
        }
    }
}
