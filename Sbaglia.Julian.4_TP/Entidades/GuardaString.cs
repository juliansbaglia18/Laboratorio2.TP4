using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Entidades
{
    public static class GuardaString
    {
        /// <summary>
        /// Guarda el string en el archivo de texto.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="archivo"></param>
        /// <returns>True si fue posible. False si no lo fue.</returns>
        public static bool Guardar(this string texto, string archivo)
        {
            bool retorno;
            StringBuilder path = new StringBuilder();

            path.Append(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            path.Append("\\");
            path.Append(archivo);

            try
            {
                StreamWriter sw = new StreamWriter(path.ToString(), true);
                sw.WriteLine(texto);

                sw.Close();

                retorno = true;
            }
            catch (Exception)
            {
                retorno = false;
            }

            return retorno;
        }
    }
}
