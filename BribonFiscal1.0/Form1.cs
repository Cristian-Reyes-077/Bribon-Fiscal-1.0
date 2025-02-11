using ProyectoExpress.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoExpress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            userandempresaBold(Credenciales.UserSIS, Credenciales.EmprMs);

        }

        private void userandempresaBold(String usuario, String Empresa) 
        {
            string texto = $"User: {usuario} Connected to: {Empresa}";

            // Encuentra las posiciones de las palabras clave
            int inicioUsuario = texto.IndexOf("User:") + "User:".Length;
            int inicioConectadoA = texto.IndexOf("Connected to:");

            // Establece los rangos para aplicar formato
            int longitudUsuario = inicioConectadoA - inicioUsuario;
            int inicioServidor = inicioConectadoA + "Connected to:".Length;

            // Limpia el contenido del RichTextBox
            richTextBox1.Clear();

            // Agrega el texto al RichTextBox
            richTextBox1.Text = texto;

            // Aplica formato en negritas al texto entre "Usuario:" y "Conectado a:"
            richTextBox1.Select(inicioUsuario, longitudUsuario);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);

            // Aplica formato en negritas al texto después de "Conectado a:"
            richTextBox1.Select(inicioServidor, texto.Length - inicioServidor);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);

            // Restablece la selección para que no quede marcado
            richTextBox1.Select(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MySqlFunciones.conectar_mysql())
            {
                // Restaurar el color de todos los botones
                button2.BackColor = SystemColors.Control;
                button3.BackColor = SystemColors.Control;
                button4.BackColor = SystemColors.Control;
                button5.BackColor = SystemColors.Control;

                // Cambiar el color del botón presionado a verde
                ((Button)sender).BackColor = Color.FromArgb(0, 204, 102); // Verde personalizado

                panel1.Controls.Clear();
               FrmAssociations frm = new FrmAssociations
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                };
                panel1.Controls.Add(frm);
                frm.Show();


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MySqlFunciones.conectar_mysql())
            {
                // Restaurar el color de todos los botones
                button1.BackColor = SystemColors.Control;
                button3.BackColor = SystemColors.Control;
                button4.BackColor = SystemColors.Control;
                button5.BackColor = SystemColors.Control;

                // Cambiar el color del botón presionado a verde
                ((Button)sender).BackColor = Color.FromArgb(0,204,102);

                panel1.Controls.Clear();
                FrmAssociationsManual frm = new FrmAssociationsManual
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                };
                panel1.Controls.Add(frm);
                frm.Show();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MySqlFunciones.conectar_mysql())
            {
                // Restaurar el color de todos los botones
                button1.BackColor = SystemColors.Control;
                button2.BackColor = SystemColors.Control;
                 button4.BackColor = SystemColors.Control;
                button5.BackColor = SystemColors.Control;

                // Cambiar el color del botón presionado a verde
                ((Button)sender).BackColor = Color.FromArgb(0, 204, 102); // Verde personalizado


                panel1.Controls.Clear();
                FrmProveedores frm = new FrmProveedores
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                };
                panel1.Controls.Add(frm);
                frm.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MySqlFunciones.conectar_mysql())
            {
                // Restaurar el color de todos los botones
                button1.BackColor = SystemColors.Control;
                button2.BackColor = SystemColors.Control;
                button3.BackColor = SystemColors.Control;
                button5.BackColor = SystemColors.Control;

                // Cambiar el color del botón presionado a verde
                ((Button)sender).BackColor = Color.FromArgb(0, 204, 102); // Verde personalizado


                panel1.Controls.Clear();
                FrmAssociationsReposicionCaja frm = new FrmAssociationsReposicionCaja
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                };
                panel1.Controls.Add(frm);
                frm.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MySqlFunciones.conectar_mysql())
            {
                // Restaurar el color de todos los botones
                button1.BackColor = SystemColors.Control;
                button2.BackColor = SystemColors.Control;
                button3.BackColor = SystemColors.Control;
                button4.BackColor = SystemColors.Control;
                
                // Cambiar el color del botón presionado a verde
                ((Button)sender).BackColor = Color.FromArgb(0, 204, 102); // Verde personalizado


                panel1.Controls.Clear();
                FrmAssociationsDepositosVarios frm = new FrmAssociationsDepositosVarios
                {
                    TopLevel = false,
                    Dock = DockStyle.Fill,
                };
                panel1.Controls.Add(frm);
                frm.Show();
            }
        }
    }
}
