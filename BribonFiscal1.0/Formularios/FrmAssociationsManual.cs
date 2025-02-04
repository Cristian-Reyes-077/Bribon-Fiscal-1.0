using ProyectoExpress.Clases.Generales;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoExpress.Formularios
{
    public partial class FrmAssociationsManual : Form
    {
        // lista de seleccionados
        List<String> uuidLista = new List<String>();

        public FrmAssociationsManual()
        {
            InitializeComponent();
        }

        private void FrmAssociationsManual_Load(object sender, EventArgs e)
        {
            /*We remove the first column in datagridview*/
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView3.RowHeadersVisible = false;

            /*We set the columns to take the entire length of the datagridview*/
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            /*Read onluy*/
            this.dataGridView1.ReadOnly = true;
            //this.dataGridView2.ReadOnly = true;
            //dataGridView2.ReadOnly = false;
            this.dataGridView3.ReadOnly = true;

            /* Activar edición al seleccionar una celda */
            dataGridView2.EditMode = DataGridViewEditMode.EditOnEnter;


            /*We remove the last row of the datagridview*/
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToAddRows = false;

            /*We selected all row*/
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            /*Combobox readonly*/
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            llenarRfcsCombo();
        }

        public void llenarRfcsCombo()
        {
            String query = $"select rfc from etaxes_xml.empresas where impuesto_simplificado= 'S' and usuario = 'x_nano_32@hotmail.com' order by rfc asc;";

            if (!MySqlFunciones.datos_combobox_mysql(query, comboBox1))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
                this.comboBox1.SelectedIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            String fechaIni = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            String fechaFin = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            String rfc = this.comboBox1.Text;

            String query = $"call bancos_auxiliarbancario('{rfc}','{fechaIni}','{fechaFin}');";

            DataTable dt = new DataTable();
            if (!MySqlFunciones.consulta_entabla_mysql(query, dt))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
            {
                this.dataGridView1.DataSource = dt;
                this.dataGridView1.Columns["estado_cta_bancaria_det_id"].Visible = false;
                this.panel1.Enabled = true;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Verificar si hay filas seleccionadas
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener el valor de la columna oculta "rfc" en la fila seleccionada
                VariablesSesion.Edo_Cta_Bancaria_Det_Id = dataGridView1.SelectedRows[0].Cells["estado_cta_bancaria_det_id"].Value.ToString();

                ActBacosAux();
            }
        }
        private void ActBacosAux()
        {
            String query = $"call bancos_auxiliarbancario_det({VariablesSesion.Edo_Cta_Bancaria_Det_Id});";
            DataTable dt = new DataTable();


            if (!MySqlFunciones.consulta_entabla_mysql(query, dt))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
            {
                this.dataGridView2.DataSource = dt;
                this.label10.Text = $"Total filas: {dataGridView2.Rows.Count.ToString()}";
                habilitaColumnaData(this.dataGridView2, "importe_aplicado");
            }
        }

        private void habilitaColumnaData(DataGridView elGrid, String columnaHabilitar)
        {
            foreach (DataGridViewColumn column in elGrid.Columns)
            {
                if (column.Name == columnaHabilitar)
                {
                    column.ReadOnly = false;
                }
                else
                {
                    column.ReadOnly = true;
                }
            }
            // Permite ediciones en el DataGridView
            elGrid.ReadOnly = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)/*Click dentro del grido*/
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                dataGridView1.Rows[e.RowIndex].Selected = true;
                VariablesSesion.Edo_Cta_Bancaria_Det_Id = row.Cells["estado_cta_bancaria_det_id"].Value.ToString();
               //MessageBox.Show(VariablesSesion.Edo_Cta_Bancaria_Det_Id);
                ActBacosAux();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ActCFDIAux(this.textBox2.Text.ToString(), this.textBox3.Text.ToString(), this.dateTimePicker3.Value, this.dateTimePicker4.Value);
        }
        private void ActCFDIAux(string rfcemisor, string rfcreceptor, DateTime fechaini, DateTime fechafin)
        {
            String query = $"call bancos_auxiliarcfdi('{rfcemisor}','{rfcreceptor}','{fechaini.ToString("yyyy-MM-dd")}','{fechafin.ToString("yyyy-MM-dd")}');";
            DataTable dt = new DataTable();


            if (!MySqlFunciones.consulta_entabla_mysql(query, dt))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
                this.dataGridView3.DataSource = dt;
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si hay filas seleccionadas
            if (dataGridView1.SelectedRows.Count > 0)
            {
                uuidLista = new List<String>();

                // filas seleccionadas
                foreach (DataGridViewRow fila in dataGridView3.SelectedRows)
                {
                    // verificar que no sea null
                    if (fila.Cells["uuid"].Value != null || !String.IsNullOrEmpty(fila.Cells["uuid"].Value.ToString()))
                    {
                        uuidLista.Add(fila.Cells["uuid"].Value.ToString());
                    }
                }
                // Obtener el valor de la columna oculta "rfc" en la fila seleccionada y concatenar ;
                //VariablesSesion.Uuid_Vta_Manual = dataGridView3.SelectedRows[0].Cells["uuid"].Value.ToString();
                VariablesSesion.Uuid_Vta_Manual = string.Join("; ", uuidLista);
                this.textBox1.Text = VariablesSesion.Uuid_Vta_Manual.ToString();
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Asegúrate de que no sea un encabezado de columna
            if (e.RowIndex >= 0)
            {
                // Obtener el valor actualizado
                string columna = dataGridView2.Columns[e.ColumnIndex].Name; // Nombre de la columna
                string nuevoValor = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                string idFila = dataGridView2.Rows[e.RowIndex].Cells["cfdi_movto_bancario_id"].Value?.ToString(); // Suponiendo que tienes una columna 'id'

                // Realizar la actualización en la base de datos
                ActualizarDatoEnBaseDeDatos(idFila, columna, nuevoValor);
            }
        }

        // Función para actualizar los datos en la base de datos
        private void ActualizarDatoEnBaseDeDatos(string idFila, string columna, string nuevoValor)
        {
            try
            {

                String queryUpd = $"UPDATE `etaxes_xml`.`cfdis_movimientos_bancarios` SET `importe_aplicado` = '{nuevoValor}' WHERE (`cfdi_movto_bancario_id` = '{idFila}');";

                if (!MySqlFunciones.query_abierto_mysql(queryUpd))
                    MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
                else
                    ActBacosAux();

                //MessageBox.Show($"{idFila},{columna},{nuevoValor}");
                //2881,importeaplicado,90//
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           agregarCFDI_MovtoBancario();
        }
        private void agregarCFDI_MovtoBancario()
        {
            foreach (var uuid in uuidLista)
            {
                String queryAdd = $"call bancos_agregarasociacioncfdi({VariablesSesion.Edo_Cta_Bancaria_Det_Id},'{uuid}',null);";
                if (!MySqlFunciones.query_abierto_mysql(queryAdd))
                    MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            }
                    ActBacosAux();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Array de DataGridView
            DataGridView[] dataGrids = { dataGridView3, dataGridView2, dataGridView1 };

            // Array de nombres para hojas de Excel
            string[] nombresHojas = { $"Info01", $"Info02", "Proveedores" };

            Funciones.ExportarDataGridsAExcel(dataGrids, nombresHojas);
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            decimal suma = 0;
            decimal sumaC = 0;

            foreach (DataGridViewRow row in dataGridView3.SelectedRows)
            {
                if (row.Cells["total"].Value != null || row.Cells["total"].Value.ToString() != "")
                {
                    decimal valor;
                    if (decimal.TryParse(row.Cells["total"].Value.ToString(), out valor))
                    {
                        suma += valor;
                    }
                }

                if (row.Cells["MontoPagadoCFDI"].Value != null || row.Cells["MontoPagadoCFDI"].Value.ToString() != "")
                {
                    decimal valorC;
                    if (decimal.TryParse(row.Cells["MontoPagadoCFDI"].Value.ToString(), out valorC))
                    {
                        sumaC += valorC;
                    }
                }

            }

            this.label9.Text = $"Total = {suma:C}     Monto Pagado CFDI = {sumaC:C}";
        }

        private void dataGridView2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    VariablesSesion.Cfdi_Movto_Mancario_Id = dataGridView2.Rows[e.RowIndex].Cells[0].Value?.ToString();

                    // Mostrar en un MessageBox
                    //MessageBox.Show($"Valor de la primera columna: {VariablesSesion.Cfdi_Movto_Mancario_Id}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void eliminarVinculoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String queryAdd = $"CALL etaxes_xml.BORRAR_CFDI_MOVIMIENTO_BANCARIO({VariablesSesion.Cfdi_Movto_Mancario_Id});";
            if (!MySqlFunciones.query_abierto_mysql(queryAdd))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
                ActBacosAux();
        }
    }
}

//2884