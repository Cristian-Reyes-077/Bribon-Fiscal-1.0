using ProyectoExpress.Clases.Generales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoExpress.Formularios
{
    public partial class FrmProveedores : Form
    {
        private DataTable dtProveedores = new DataTable(); 
        private bool permitirEjecucionEvento = true;
        string RFC_Proveedor = string.Empty;


        public FrmProveedores()
        {
            InitializeComponent();
        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            /*We remove the first column in datagridview*/
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView3.RowHeadersVisible = false;

            /*We set the columns to take the entire length of the datagridview*/
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            /*Read onluy*/
           // this.dataGridView1.ReadOnly = true;
            this.dataGridView2.ReadOnly = true;
            this.dataGridView3.ReadOnly = true;

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
           
            consultaProveedores();
        }

        private void consultaProveedores()
        {
            String rfc = this.comboBox1.Text;

            String query = $"call bancos_proveedoreslista('{rfc}');";

            if (!MySqlFunciones.consulta_entabla_mysql(query, dtProveedores))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
            {
                FiltrarSinClasificadores();
                this.groupBox3.Enabled = true;
                this.groupBox4.Enabled = true;
            }
        }

        private void FiltrarSinClasificadores()
        {
            // Validamos que tenga datos el datatable
            if (dtProveedores.Rows.Count <= 0)
                return;

            // Desactivar temporalmente la ejecución del evento
            permitirEjecucionEvento = false;

            // Crear una vista para filtrar los datos
            DataView vista = dtProveedores.DefaultView;

            if (checkBox1.Checked)
            {
                // Mostrar solo filas donde clasificadores esté vacío o nulo
                vista.RowFilter = "clasificador IS NULL OR clasificador = ''";
            }
            else
            {
                // Mostrar todos los registros
                vista.RowFilter = string.Empty;
            }

            // Asignar la vista filtrada al DataGridView
            dataGridView1.DataSource = vista;
            this.dataGridView1.Columns["proveedor_id"].Visible = false;
            habilitaColumnaData(this.dataGridView1, "clasificador");

            // Limpiar selección inicial
            dataGridView1.ClearSelection();

            label2.Text = $"Registros totales: {dataGridView1.Rows.Count.ToString()}";

            // Reactivar la ejecución del evento
            permitirEjecucionEvento = true;
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            FiltrarSinClasificadores();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //// Verificar si se permite la ejecución
            //if (!permitirEjecucionEvento) return;

            //// Verificar si hay filas seleccionadas
            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            //    string RFC_Proveedor = dataGridView1.SelectedRows[0].Cells["RFC_Proveedor"].Value.ToString();

            //    string suple = $"List of items purchased from historical supplier:{RFC_Proveedor}.";
            //    string purchases = $"Last 20 purchases: {RFC_Proveedor}.";

            //    this.groupBox3.Text = suple;
            //    this.groupBox4.Text = purchases;

            //   ActDatasGrid(RFC_Proveedor);
            //}
        }


        private void ActDatasGrid(String Proveedor)
        {
            String rfc = this.comboBox1.Text;

            String queryultimasCompras = $"call bancos_compras20('{Proveedor}','{rfc}');";
            String queryarticulosProveedores = $"call bancos_articulosproveedores('{Proveedor}','{rfc}');";


            DataTable dtultimasCompras = new DataTable();
            DataTable dtarticulosProveedores = new DataTable();

            if (!MySqlFunciones.consulta_entabla_mysql(queryultimasCompras, dtultimasCompras))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
            {
                this.dataGridView3.DataSource = dtultimasCompras;
            }

            if (!MySqlFunciones.consulta_entabla_mysql(queryarticulosProveedores, dtarticulosProveedores))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
            {
                this.dataGridView2.DataSource = dtarticulosProveedores;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtener el valor actualizado
                string columna = dataGridView1.Columns[e.ColumnIndex].Name; // Nombre de la columna
                string nuevoValor = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                string idFila = dataGridView1.Rows[e.RowIndex].Cells["proveedor_id"].Value?.ToString(); // Suponiendo que tienes una columna 'id'

                // Realizar la actualización en la base de datos
                ActualizarDatoEnBaseDeDatos(idFila, columna, nuevoValor);
            }
        }

        // Función para actualizar los datos en la base de datos
        private void ActualizarDatoEnBaseDeDatos(string idFila, string columna, string nuevoValor)
        {
            try
            {

                String queryUpd = $"UPDATE `etaxes_xml`.`proveedores_empresas` SET `clasificador` = '{nuevoValor}' WHERE (`proveedor_id` = '{idFila}');\r\n";

                if (!MySqlFunciones.query_abierto_mysql(queryUpd))
                    MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
                else
                    consultaProveedores();

                //MessageBox.Show($"{idFila},{columna},{nuevoValor}");
                //2881,importeaplicado,90//
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que no sea un clic en el encabezado
            if (e.RowIndex < 0) return;

            // Obtén el RFC del proveedor de la fila seleccionada
            RFC_Proveedor = dataGridView1.Rows[e.RowIndex].Cells["RFC_Proveedor"].Value.ToString();

            // Actualiza los textos en los GroupBoxes
            string suple = $"List of items purchased from historical supplier: {RFC_Proveedor}.";
            string purchases = $"Last 20 purchases: {RFC_Proveedor}.";

            this.groupBox3.Text = suple;
            this.groupBox4.Text = purchases;

            // Llama a tu función para actualizar los datos relacionados
            ActDatasGrid(RFC_Proveedor);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Array de DataGridView
            DataGridView[] dataGrids = { dataGridView3, dataGridView2, dataGridView1 };

            // Array de nombres para hojas de Excel
            string[] nombresHojas = { $"Ult_20_compras_a_{RFC_Proveedor}", $"Art_comprados_a_{RFC_Proveedor}", "Proveedores" };

           Funciones.ExportarDataGridsAExcel(dataGrids, nombresHojas);
        }
    }
}
