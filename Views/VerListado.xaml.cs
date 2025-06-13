using DiaParts_Globo.Database;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// Lógica de interacción para VerListado.xaml
    /// </summary>
    public partial class VerListado : Window
    {
        public VerListado()
        {
            InitializeComponent();
            CargarPartes();
        }
        private void CargarPartes()
        {
            var lista = new List<Parte>();

            using (var conn = new SQLiteConnection(SQLiteDbContext.GetConnectionString()))
            {
                conn.Open();
                string query = "SELECT * FROM Partes";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Parte
                        {
                            Nombre = reader["Nombre"].ToString(),
                            NumeroParte = reader["NumeroParte"].ToString(),
                            Modelo = reader["Modelo"].ToString(),
                            ImagenPath = reader["ImagenPath"].ToString()
                        });
                    }
                }
            }

            dgPartes.ItemsSource = lista;
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Función de impresión/exportación será implementada en el siguiente paso.");
        }
        public class Parte
        {
            public string Nombre { get; set; }
            public string NumeroParte { get; set; }
            public string Modelo { get; set; }
            public string ImagenPath { get; set; }
        }
        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            var listaActual = (List<Parte>)dgPartes.ItemsSource;
            var exportarVentana = new ExportarListado(listaActual);
            exportarVentana.ShowDialog();
        }
    }
}
