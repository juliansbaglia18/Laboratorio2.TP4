using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Entidades
{
    public static class PaqueteDAO
    {
        private static SqlCommand comando;
        private static SqlConnection conexion;

        static PaqueteDAO()
        {
            comando = new SqlCommand();
            conexion = new SqlConnection(Properties.Settings.Default.Conexion);
        }

        /// <summary>
        /// Inserta los datos del paquete en la base de datos.
        /// </summary>
        /// <param name="p">Paquete a guardar.</param>
        /// <returns>True si se pudo guardar. False si no se pudo.</returns>
        public static bool Insertar(Paquete p)
        {
            bool retorno = false;
            StringBuilder sb = new StringBuilder();

            try
            {
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                sb.AppendFormat("INSERT INTO Paquetes(direccionEntrega,trackingID,alumno) values('{0}','{1}','{2}')", p.DireccionEntrega, p.TrackingID, "Luciano.Sinisterra.2A");
                comando.CommandText = sb.ToString();

                comando.ExecuteNonQuery();
                retorno = true;

            }
            catch (Exception e)
            {
                throw new TrackingIdRepetidoException("Error al ingresar el nuevo paquete.", e);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }

            return retorno;
        }
    }
}
