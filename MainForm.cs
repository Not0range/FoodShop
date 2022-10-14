using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodShop
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new GroupsForm().ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new DepartmentsForm().ShowDialog(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new ProductsForm().ShowDialog(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new SuppliesForm().ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new SellsForm().ShowDialog(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new ReturnsForm().ShowDialog(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new Queries().ShowDialog(this);
        }
    }
}
