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
    public partial class Queries : Form
    {
        public Queries()
        {
            InitializeComponent();
        }

        private void Queries_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadExpired();
            LoadMove();
        }

        private void LoadProducts()
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

        private void LoadExpired()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("select t3.Название, sum(t2.Количество - ISNULL(t1.Количество, 0)) as Остаток from " +
                    "(select[ID товара], sum(Количество) as Количество from Реализации group by [ID товара]) t1 " +
                    "right join(Поставки t2 inner join Товары t3 on t2.[ID товара] = t3.ID) on t1.[ID товара] = t2.ID " +
                    "where DATEADD(DAY, t3.[Срок годности], t2.[Дата и время поставки]) < GETDATE()" +
                    "group by t3.Название having sum(t2.Количество - ISNULL(t1.Количество, 0)) > 0", sql);
                var reader = cmd.ExecuteReader();
                dataGridView2.SuspendLayout();
                dataGridView2.Rows.Clear();
                while (reader.Read())
                    dataGridView2.Rows.Add(reader[0], reader[1]);
                reader.Close();
                dataGridView2.ResumeLayout();

                sql.Close();
            }
        }

        private void LoadMove()
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("select t1.Название, ISNULL(t2.Поставлено, 0) as Поставлено, " +
                    "ISNULL(t3.Реализовано, 0) as Реализовано, ISNULL(t4.Возвращено, 0) as Возвращено, " +
                    "(ISNULL(t2.Поставлено, 0) - ISNULL(t3.Реализовано, 0) + ISNULL(t4.Возвращено, 0)) as Остаток " +
                    "from ((Товары t1 left join (select [ID товара], sum(Количество) as Поставлено from Поставки " +
                    "group by [ID товара]) t2 on t1.ID = t2.[ID товара]) left join " +
                    "(select t2.[ID товара], sum(t1.Количество) as Реализовано from Реализации t1 inner join Поставки t2 on t1.[ID товара] = t2.ID " +
                    "group by t2.[ID товара]) t3 on t1.ID = t3.[ID товара]) left join " +
                    "(select t2.[ID товара], sum(t1.Количество) as Возвращено from Возвраты t1 inner join Поставки t2 on t1.[ID товара] = t2.ID " +
                    "group by t2.[ID товара]) t4 on t1.ID = t4.[ID товара]", sql);
                var reader = cmd.ExecuteReader();
                dataGridView3.SuspendLayout();
                dataGridView3.Rows.Clear();
                while (reader.Read())
                    dataGridView3.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                reader.Close();
                dataGridView3.ResumeLayout();

                sql.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var sql = new SqlConnection(Program.sql.ConnectionString))
            {
                sql.Open();

                var cmd = new SqlCommand("Select * from Поставки where [ID товара] = @ID", sql);
                cmd.Parameters.AddWithValue("@ID", (productBox.SelectedItem as Product).ID);
                var reader = cmd.ExecuteReader();
                dataGridView1.SuspendLayout();
                dataGridView1.Rows.Clear();
                while (reader.Read())
                    dataGridView1.Rows.Add(reader[0], reader[2], reader[3], reader[4]);
                reader.Close();
                dataGridView1.ResumeLayout();

                sql.Close();
            }
        }
    }
}
