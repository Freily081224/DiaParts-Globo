using DiaParts_Globo.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
using System.Windows.Shapes;


namespace DiaParts_Globo.Views
{
    /// <summary>
    /// Lógica de interacción para AgregarParte.xaml
    /// </summary>
    public partial class AgregarParte : Window
    {
        private string _imagenPath;
        public AgregarParte()
        {
            InitializeComponent();
            CargarModelos();
        }
        private void CargarModelos()
        {
            cbModelos.Items.Clear();

            using (var conn = new SQLiteConnection(SQLiteDbContext.GetConnectionString()))
            {
                conn.Open();
                string query = "SELECT Nombre FROM Modelos";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbModelos.Items.Add(reader["Nombre"].ToString());
                    }
                }
            }
        }

        private void BtnSeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Imágenes (*.png;*.jpg)|*.png;*.jpg"
            };

            if (dlg.ShowDialog() == true)
            {
                _imagenPath = dlg.FileName;
                lblImagenPath.Text = _imagenPath;
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                            string.IsNullOrWhiteSpace(txtNumeroParte.Text) ||
                            cbModelos.SelectedItem == null)
            {
                MessageBox.Show("Complete todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var conn = new SQLiteConnection(SQLiteDbContext.GetConnectionString()))
            {
                conn.Open();
                string insert = @"INSERT INTO Partes (Nombre, NumeroParte, Modelo, ImagenPath) 
                                  VALUES (@nombre, @numero, @modelo, @imagen)";

                using (var cmd = new SQLiteCommand(insert, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@numero", txtNumeroParte.Text);
                    cmd.Parameters.AddWithValue("@modelo", cbModelos.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@imagen", _imagenPath ?? "");

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Parte guardada correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            txtNombre.Clear();
            txtNumeroParte.Clear();
            cbModelos.SelectedIndex = -1;
            lblImagenPath.Text = "(Ninguna imagen seleccionada)";
            _imagenPath = null;
        }

        private void BtnVerListado_Click(object sender, RoutedEventArgs e)
        {
            var listado = new VerListado();
            listado.ShowDialog();
        }
    }
    
}
