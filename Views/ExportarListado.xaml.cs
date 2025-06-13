using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
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
using static DiaParts_Globo.Views.VerListado;
using PdfDoc = iTextSharp.text.Document;
using WordDoc = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace DiaParts_Globo.Views
{
    /// <summary>
    /// Lógica de interacción para ExportarListado.xaml
    /// </summary>
    public partial class ExportarListado : Window
    {
        private List<Parte> partes;

        // Constructor que recibe la lista desde la ventana que llama
        public ExportarListado(List<Parte> lista)
        {
            InitializeComponent();
            partes = lista;
        }

        private void BtnExportarPDF_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "ListadoDiaParts.pdf";

            // Usa el nombre completo para evitar ambigüedad
            iTextSharp.text.Document doc = new iTextSharp.text.Document();

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                doc.Add(new iTextSharp.text.Paragraph("Listado de Partes - DiaParts Globo"));
                doc.Add(new iTextSharp.text.Paragraph(" "));

                PdfPTable table = new PdfPTable(4);
                table.AddCell("Nombre");
                table.AddCell("Número");
                table.AddCell("Modelo");
                table.AddCell("Imagen");

                foreach (var p in partes)
                {
                    table.AddCell(p.Nombre);
                    table.AddCell(p.NumeroParte);
                    table.AddCell(p.Modelo);
                    table.AddCell(p.ImagenPath);
                }

                doc.Add(table);
                doc.Close();
            }

            MessageBox.Show($"Exportado como PDF: {System.IO.Path.GetFullPath(filePath)}");

        }

        private void BtnExportarWord_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "ListadoDiaParts.docx";

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();

                var body = new DocumentFormat.OpenXml.Wordprocessing.Body();

                body.Append(
                    new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                        new DocumentFormat.OpenXml.Wordprocessing.Run(
                            new DocumentFormat.OpenXml.Wordprocessing.Text("Listado de Partes - DiaParts Globo")
                        )
                    )
                );

                body.Append(
                    new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                        new DocumentFormat.OpenXml.Wordprocessing.Run(
                            new DocumentFormat.OpenXml.Wordprocessing.Text(" ")
                        )
                    )
                );

                var table = new DocumentFormat.OpenXml.Wordprocessing.Table();

                var headerRow = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                headerRow.Append(
                    CrearCelda("Nombre"),
                    CrearCelda("Número"),
                    CrearCelda("Modelo"),
                    CrearCelda("Imagen")
                );
                table.Append(headerRow);

                foreach (var parte in partes)
                {
                    var row = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                    row.Append(
                        CrearCelda(parte.Nombre),
                        CrearCelda(parte.NumeroParte),
                        CrearCelda(parte.Modelo),
                        CrearCelda(parte.ImagenPath)
                    );
                    table.Append(row);
                }

                body.Append(table);
                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            MessageBox.Show($"Exportado como Word: {System.IO.Path.GetFullPath(filePath)}");
        }
        private DocumentFormat.OpenXml.Wordprocessing.TableCell CrearCelda(string texto)
        {
            return new DocumentFormat.OpenXml.Wordprocessing.TableCell(
                new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                    new DocumentFormat.OpenXml.Wordprocessing.Run(
                        new DocumentFormat.OpenXml.Wordprocessing.Text(texto ?? "")
                    )
                )
            );
        }
    }
}
