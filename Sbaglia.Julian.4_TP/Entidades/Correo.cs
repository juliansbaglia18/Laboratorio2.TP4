using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Entidades
{
    public class Correo : IMostrar<List<Paquete>>
    {
        private List<Thread> mockPaquetes;
        private List<Paquete> paquetes;

        public Correo()
        {
            this.mockPaquetes = new List<Thread>();
            this.paquetes = new List<Paquete>();
        }

        public List<Paquete> Paquetes
        {
            get { return this.paquetes; }
            set { this.paquetes = value; }
        }

        /// <summary>
        /// Crea la cadena de texto con los datos del paquete
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public string MostrarDatos(IMostrar<List<Paquete>> elemento)
        {
            string listaPaquetes = "";

            foreach (Paquete p in ((Correo)elemento).Paquetes)
            {
                listaPaquetes += string.Format("{0} para {1} ({2})", p.TrackingID, p.DireccionEntrega, p.Estado.ToString());
                listaPaquetes += "\n";
            }
            return listaPaquetes;
        }

        /// <summary>
        /// Cierra los hilos donde se ejecutan los paquetes.
        /// </summary>
        public void FinEntrega()
        {
            foreach (Thread hiloPaquete in mockPaquetes)
            {
                hiloPaquete.Abort();
            }
        }

        /// <summary>
        /// Agrega el paquete al correo e inicia el hilo con el ciclo de vida.
        /// Si el paquete ya se encuentra, lanza la aexcepcion.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Correo operator +(Correo c, Paquete p)
        {
            foreach (Paquete item in c.paquetes)
            {
                if (item == p)
                {
                    throw new TrackingIdRepetidoException("El paquete que se intento ingrersar ya existe");
                }
            }
            c.paquetes.Add(p);

            Thread paqueteCicloDeVida = new Thread(p.MockCicloDeVida);

            c.mockPaquetes.Add(paqueteCicloDeVida);

            paqueteCicloDeVida.Start();

            return c;
        }
    }
}
