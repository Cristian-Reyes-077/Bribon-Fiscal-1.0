using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using ProyectoExpress.Formularios;

namespace ProyectoExpress.Clases.Generales
{
    public static class Funciones
    {
        public static void ExportarDataGridsAExcel(DataGridView[] dataGrids, string[] nombresHojas)
        {
            // Validar que los DataGridView y los nombres de hojas coincidan
            if (dataGrids.Length != nombresHojas.Length)
            {
                MessageBox.Show("El número de DataGridView y nombres de hojas no coincide.", "Error");
                return;
            }

            // Crear el SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "Proveedores",
                Filter = "Archivos de Excel (*.xlsx)|*.xlsx",
                Title = "Guardar archivo de Excel"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            string rutaArchivo = saveFileDialog.FileName;

            // Crear una instancia de Excel
            var excelApp = new Excel.Application();
            var workbook = excelApp.Workbooks.Add();

            try
            {
                for (int i = 0; i < dataGrids.Length; i++)
                {
                    DataGridView dgv = dataGrids[i];
                    string nombreHoja = nombresHojas[i];

                    // Crear una nueva hoja o seleccionar la existente
                    Excel.Worksheet worksheet = (i < workbook.Sheets.Count)
                        ? (Excel.Worksheet)workbook.Sheets[i + 1]
                        : (Excel.Worksheet)workbook.Sheets.Add();

                    worksheet.Name = nombreHoja;

                    // Exportar encabezados
                    for (int col = 0; col < dgv.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col + 1] = dgv.Columns[col].HeaderText;
                    }

                    // Exportar datos
                    for (int row = 0; row < dgv.Rows.Count; row++)
                    {
                        for (int col = 0; col < dgv.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1] = dgv.Rows[row].Cells[col].Value;
                        }
                    }
                }

                // Guardar el archivo
                workbook.SaveAs(rutaArchivo);
                MessageBox.Show("Datos exportados exitosamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar: {ex.Message}", "Error");
            }
            finally
            {
                // Liberar recursos
                workbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }
    }
}
