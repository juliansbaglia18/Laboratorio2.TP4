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

        public string DireccionEntrega
        {
            get { return this.direccionEntrega; }
            set { this.direccionEntrega = value; }
        }

        public EEstado Estado
        {
            get { return this.estado; }
            set { this.estado = value; }
        }

        public string TrackingID
        {
            get { return this.trackingID; }
            set { this.trackingID = value; }
        }

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
            /*
            if (this.InformaEstado != null)
            {
                this.estado = EEstado.Ingresado;
                this.InformaEstado(this, EventArgs.Empty);
                Thread.Sleep(4000);

                this.Estado = EEstado.EnViaje;
                this.InformaEstado(this, EventArgs.Empty);
                Thread.Sleep(4000);

                this.Estado = EEstado.Entregado;
                this.InformaEstado(this, EventArgs.Empty);
            }
            else
            {
                throw new Exception("Paquete posee evento pero no manejador.");
            }*/

            /*while(this.Estado != EEstado.Entregado)
            {
                Thread.Sleep(4000);
                this.Estado += 1;
                this.InformaEstado(this, new EventArgs());
            }

            PaqueteDAO.Insertar(this);*/

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

        public string MostrarDatos(IMostrar<Paquete> elemento)
        {
            return string.Format("{0} para {1}", ((Paquete)elemento).TrackingID, ((Paquete)elemento).DireccionEntrega);
        }

        public override string ToString()
        {
            return MostrarDatos(this);
        }

        public static bool operator ==(Paquete p1, Paquete p2)
        {
            return (p1.TrackingID.Equals(p2.TrackingID));
        }

        public static bool operator !=(Paquete p1, Paquete p2)
        {
            return !(p1 == p2);
        }
    }
}
