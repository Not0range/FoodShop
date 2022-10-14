using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodShop
{
    internal static class Program
    {
        public static SqlConnectionStringBuilder sql = new SqlConnectionStringBuilder
        {
            DataSource = "localhost",
            InitialCatalog = "Food_Shop",
            IntegratedSecurity = false,
            UserID = "sa",
            Password = "123qwertY*"
        };
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
