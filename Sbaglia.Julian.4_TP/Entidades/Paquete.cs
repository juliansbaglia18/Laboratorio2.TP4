using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Entidades
{
    public class Paquete : IMostrar<Paquete>
    {
        public enum EEstado
        {
            Ingresado,
            EnViaje,
            Entregado
        }
        public delegate void DelegadoEstado(Object sender, EventArgs e);
        public event DelegadoEstado InformaEstado;

        private string direccionEntrega;
        private EEstado estado;
        private string trackingID;

        public delegate void delegadoErrorConexion();
        public static event delegadoErrorConexion eventErrorConexion;

        /// <summary>
        /// Propiedad de lectura y escritura de la direccion de entrega.
        /// </summary>
        public string DireccionEntrega
        {
            get { return this.direccionEntrega; }
            set { this.direccionEntrega = value; }
        }

        /// <summary>
        /// Propiedad de lectura y escritura del estado.
        /// </summary>
        public EEstado Estado
        {
            get { return this.estado; }
            set { this.estado = value; }
        }

        /// <summary>
        /// Propiedad de lectura y escritura del tracking ID.
        /// </summary>
        public string TrackingID
        {
            get { return this.trackingID; }
            set { this.trackingID = value; }
        }

        /// <summary>
        /// Constructor parametrizado.
        /// </summary>
        /// <param name="direccionEntrega">Direccion de entrega del paquete.</param>
        /// <param name="trackingID">Tracking ID del paquete.</param>
        public Paquete(string direccionEntrega, string trackingID)
        {
            this.DireccionEntrega = direccionEntrega;
            this.TrackingID = trackingID;
        }

        /// <summary>
        /// Actualiza el estado del paquete y lo guarda en la base de datos. Si falla lanza el evento correspondiente
        /// </summary>
        public void MockCicloDeVida()
        {
            while (Estado != EEstado.Entregado)
            {
                Thread.Sleep(4000);
                Estado++;
                InformaEstado(this, null);
            }

            if (!(PaqueteDAO.Insertar(this)))
            {
                Paquete.eventErrorConexion();
            }
        }

        /// <summary>
        /// Muestra los datos de un paquete.
        /// </summary>
        /// <returns>Los datos en forma de cadena de caracteres.</returns>
        public string MostrarDatos(IMostrar<Paquete> elemento)
        {
            return string.Format("{0} para {1}", ((Paquete)elemento).TrackingID, ((Paquete)elemento).DireccionEntrega);
        }

        /// <summary>
        /// Muestra los datos de un paquete.
        /// </summary>
        /// <returns>Los datos en forma de cadena de caracteres.</returns>
        public override string ToString()
        {
            return MostrarDatos(this);
        }

        /// <summary>
        /// Compara dos paquetes para saber si son iguales.
        /// </summary>
        /// <param name="p1">Paquete a comparar.</param>
        /// <param name="p2">Paquete a comparar.</param>
        /// <returns>True si son iguales. False si no.</returns>
        public static bool operator ==(Paquete p1, Paquete p2)
        {
            return (p1.TrackingID.Equals(p2.TrackingID));
        }

        /// <summary>
        /// Compara dos paquetes para saber si son iguales.
        /// </summary>
        /// <param name="p1">Paquete a comparar.</param>
        /// <param name="p2">Paquete a comparar.</param>
        /// <returns>False si son iguales. True si no.</returns>
        public static bool operator !=(Paquete p1, Paquete p2)
        {
            return !(p1 == p2);
        }
    }
}
