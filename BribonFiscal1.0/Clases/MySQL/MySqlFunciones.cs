using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class MySqlFunciones
{
    public static int conectado = 0;
    //public static string user_app = "crisreyesc26@gmail.com";
    //public static string user_Mysql/*="root"*/;
    //public static string pass_Mysql = "21158012";
    //public static string db_Mysql = "descargapopomasiva";
    //public static string server_Mysql = "localhost";
    public static string ultimo_error_Mysql;
    public static MySqlConnection conexion_mysql;

    public static bool conectar_mysql()
    {
        try
        {



            MySqlConnectionStringBuilder conexion_my = new MySqlConnectionStringBuilder();

            //configuracionManager.AgregarValor("MySql_Empr_", Credenciales.EmprMs);
            //configuracionManager.AgregarValor("MySql_User_", Credenciales.UserMs);
            //configuracionManager.AgregarValor("MySql_Serv_", Credenciales.ServMs);
            //configuracionManager.AgregarValor("MySql_Pasw_", cifrados.Encriptar_sha256(Credenciales.PassMs));

            conexion_my.Server = Credenciales.ServMs;
            conexion_my.UserID = Credenciales.UserMs;
            conexion_my.Password = Credenciales.PassMs;
            conexion_my.Database = Credenciales.EmprMs;


            conexion_mysql = new MySqlConnection(conexion_my.ToString());
            conexion_mysql.Open();
            conexion_mysql.Close();
            return true;
        }
        catch (Exception e)
        {
            ultimo_error_Mysql = e.Message.ToString();
            return false;
        }
    }


    public static bool query_abierto_mysql(string elquery)
    {
        MySqlCommand comando = new MySqlCommand();
        comando.Connection = conexion_mysql;
        try
        {
            conexion_mysql.Open();
            comando.CommandText = "START TRANSACTION";
            comando.ExecuteNonQuery();
            comando.CommandText = elquery;
            comando.ExecuteNonQuery();
            comando.CommandText = "COMMIT";
            comando.ExecuteNonQuery();
            conexion_mysql.Close();
            return true;
        }
        catch (Exception ex)
        {
            if (conexion_mysql.State == System.Data.ConnectionState.Open)
            {
                comando.CommandText = "ROLLBACK";
                comando.ExecuteNonQuery();
                conexion_mysql.Close();
            }
            ultimo_error_Mysql = ex.Message.ToString();
            return false;
        }
    }

    public static bool consulta_entabla_mysql(string elquery, DataTable latabla)
    {
        try
        {
            latabla.Rows.Clear();
            if (conexion_mysql.State == ConnectionState.Closed)
            {
                conexion_mysql.Open();
            }

            MySqlDataAdapter adaptador = new MySqlDataAdapter(elquery, conexion_mysql);
            adaptador.Fill(latabla);
            conexion_mysql.Close();
            return true;
        }
        catch (Exception ex)
        {
            ultimo_error_Mysql = ex.Message.ToString();
            return false;
        }
    }

   

    public static bool datos_combobox_mysql(string elquery, System.Windows.Forms.ComboBox elcombo)
    {
        MySqlCommand comando = new MySqlCommand();
        MySqlDataReader adaptador;
        comando.CommandText = elquery;
        comando.Connection = conexion_mysql;
        try
        {
            if (conexion_mysql.State != System.Data.ConnectionState.Open)
            {
                conexion_mysql.Open();
            }
            adaptador = comando.ExecuteReader();
            if (adaptador.HasRows)
            {
                while (adaptador.Read())
                {
                    elcombo.Items.Add(adaptador.GetString(0));
                }

            }
            adaptador.Dispose();
            adaptador.Close();
            conexion_mysql.Close();
            return true;
        }
        catch (Exception ex)
        {
            if (conexion_mysql.State != System.Data.ConnectionState.Closed)
            {
                conexion_mysql.Close();
            }

            ultimo_error_Mysql = ex.Message.ToString();


            return false;
        }

    }

    public static string consulta_campo_mysql_Int(string elquery)
    {
        MySqlCommand comando = new MySqlCommand();
        MySqlDataReader adaptador1;
        comando.CommandText = elquery;
        comando.Connection = conexion_mysql;
        //var resultado = "Sin Resultados";
        int resultadoE = 0;
        if (conexion_mysql.State != System.Data.ConnectionState.Open)
        {
            conexion_mysql.Open();
        }
        adaptador1 = comando.ExecuteReader();

        try
        {

            if (adaptador1.HasRows)
            {
                while (adaptador1.Read())
                {
                    resultadoE = adaptador1.GetInt32(0);
                }

            }
            adaptador1.Dispose();
            adaptador1.Close();
            conexion_mysql.Close();
            return resultadoE.ToString();

        }
        catch (Exception ex)
        {

            ultimo_error_Mysql = ex.Message.ToString();
            if (conexion_mysql.State != System.Data.ConnectionState.Closed)
            {
                adaptador1.Dispose();
                adaptador1.Close();
                conexion_mysql.Close();
            }
            return "Error Consulta";
        }
    }

    public static string consulta_campo_mysql(string elquery)
    {
        MySqlCommand comando = new MySqlCommand();
        MySqlDataReader adaptador1;
        comando.CommandText = elquery;
        comando.Connection = conexion_mysql;
        string resultado = "Sin Resultados";
        if (conexion_mysql.State != System.Data.ConnectionState.Open)
        {
            conexion_mysql.Open();
        }
        adaptador1 = comando.ExecuteReader();

        try
        {

            if (adaptador1.HasRows)
            {
                while (adaptador1.Read())
                {
                    resultado = (adaptador1.GetString(0));
                }

            }
            adaptador1.Dispose();
            adaptador1.Close();
            conexion_mysql.Close();
            return resultado;

        }
        catch (Exception ex)
        {

            ultimo_error_Mysql = ex.Message.ToString();
            if (conexion_mysql.State != System.Data.ConnectionState.Closed)
            {
                adaptador1.Dispose();
                adaptador1.Close();
                conexion_mysql.Close();
            }
            return "Error Consulta";
        }
    }



}
