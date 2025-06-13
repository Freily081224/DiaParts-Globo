using DiaParts_Globo.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;
using DiaParts_Globo.Views;

namespace DiaParts_Globo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SQLiteDbContext.InitializeDatabase();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text == "Admin" && txtPass.Password == "Ad30032003")
            {
                var agregarVentana = new Views.AgregarParte();
                agregarVentana.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
