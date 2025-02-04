using ProyectoExpress.Clases.Generales;
using System;
using System.Collections;
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
    public partial class FrmAssociations : Form
    {
        public FrmAssociations()
        {
            InitializeComponent();
        }
        private void FrmAssociations_Load(object sender, EventArgs e)
        {
            /*We remove the first column in datagridview*/
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersVisible = false;

            /*We set the columns to take the entire length of the datagridview*/
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            /*Read onluy*/
            this.dataGridView1.ReadOnly = true;
            this.dataGridView2.ReadOnly = true;

            /*We remove the last row of the datagridview*/
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToAddRows = false;

            /*We selected all row*/
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                string texto = textBox1.Text;

                // Verifica si contiene la cadena &id= y &re=
                if (texto.Contains("&id=") && texto.Contains("&re="))
                {
                    int inicio = texto.IndexOf("&id=") + "&id=".Length;
                    int fin = texto.IndexOf("&re=");

                    // Extrae el texto entre &id= y &re=
                    string resultado = texto.Substring(inicio, fin - inicio);

                    this.textBox2.Text = resultado;
                    this.button2.Enabled = true;

                }
                else
                {
                    MessageBox.Show("El formato del texto no es válido.", "Error");
                }

                // Evita que la tecla haga otra acción predeterminada
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            String query = $"call bancos_agregarasociacioncfdi('{VariablesSesion.Edo_Cta_Bancaria_Det_Id}','{this.textBox2.Text}','{this.textBox1.Text}');";

            if (!MySqlFunciones.query_abierto_mysql(query))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
                ActBacosAux();
        }

        private void ActBacosAux()
        {
            String query = $"call bancos_auxiliarbancario_det({VariablesSesion.Edo_Cta_Bancaria_Det_Id});";
            DataTable dt = new DataTable();


            if (!MySqlFunciones.consulta_entabla_mysql(query, dt))
                MessageBox.Show("Error: " + MySqlFunciones.ultimo_error_Mysql, "Renovatio Pyme");
            else
                this.dataGridView2.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Array de DataGridView
            DataGridView[] dataGrids = { dataGridView2, dataGridView1 };

            // Array de nombres para hojas de Excel
            string[] nombresHojas = { $"Info01", "Proveedores" };

            Funciones.ExportarDataGridsAExcel(dataGrids, nombresHojas);
        }
    }
}
