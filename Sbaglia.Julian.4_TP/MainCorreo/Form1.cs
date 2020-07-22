using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;

namespace MainCorreo
{
    public partial class FrmPpal : Form
    {
        private Correo correo;

        public FrmPpal()
        {
            InitializeComponent();

            this.correo = new Correo();

            this.Text = "Correo UTN por Julian.Sbaglia.2A";
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            Paquete.eventErrorConexion += this.MensajeErrorBaseDeDatos;
            this.FormClosing += new FormClosingEventHandler(FrmPpal_FormClosing);
        }

        /// <summary>
        /// Agrega un nuevo paquete al sistema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Paquete nuevoPaquete = new Paquete(this.txtDireccion.Text, this.mtxtTrackingID.Text);
                nuevoPaquete.InformaEstado += this.paq_InformaEstado;
                this.correo += nuevoPaquete;
            }
            catch (TrackingIdRepetidoException ex)
            {
                MessageBox.Show(ex.Message);
            }
            ActualizarEstados();
        }

        /// <summary>
        /// Muestra la informacion de todos los paquetes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarTodos_Click_1(object sender, EventArgs e)
        {
            this.MostrarInformacion<List<Paquete>>((IMostrar<List<Paquete>>)correo);
        }
        

        /// <summary>
        /// Informa el estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paq_InformaEstado(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Paquete.DelegadoEstado d = new Paquete.DelegadoEstado(paq_InformaEstado);

                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                this.ActualizarEstados();
            }
        }

        private void MostrarInformacion<T>(IMostrar<T> elemento)
        {
            if (elemento != null)
            {
                String datos;

                rtbMostrar.Text = elemento.MostrarDatos(elemento);
                datos = rtbMostrar.Text;

                if (datos.Guardar("salida.txt") == false)
                {
                    MessageBox.Show("Error al guardar los datos.");
                }
            }
        }

        /// <summary>
        /// Actualiza la lista de paquetes a su estado mas reciente
        /// </summary>
        private void ActualizarEstados()
        {
            lstEstadoIngresado.Items.Clear();
            lstEstadoEnViaje.Items.Clear();
            lstEstadoEntregado.Items.Clear();

            foreach (Paquete item in this.correo.Paquetes)
            {
                switch (item.Estado)
                {
                    case Paquete.EEstado.Ingresado:
                        lstEstadoIngresado.Items.Add(item);
                        break;
                    case Paquete.EEstado.EnViaje:
                        lstEstadoEnViaje.Items.Add(item);
                        break;
                    case Paquete.EEstado.Entregado:
                        lstEstadoEntregado.Items.Add(item);
                        break;
                }
            }
        }

        /// <summary>
        /// Al cerrar la ventana, aborta todos los hilos activos.
        /// </summary>
        /// <param name="sender">Emisor</param>
        /// <param name="e">Evento</param>
        private void FrmPpal_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.correo.FinEntrega();
        }

        private void MensajeErrorBaseDeDatos()
        {
            MessageBox.Show("Error de conexion hacia la base de datos", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void mostrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MostrarInformacion<Paquete>((IMostrar<Paquete>)lstEstadoEntregado.SelectedItem);
        }

    }
}
