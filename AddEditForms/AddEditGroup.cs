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
    public partial class AddEditGroup : Form
    {
        public AddEditGroup()
        {
            InitializeComponent();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameBox.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;

        }
    }
}
