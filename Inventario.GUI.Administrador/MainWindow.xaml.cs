using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace Inventario.GUI.Administrador
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum accion
        {
            Nuevo,
            Editar
        }
        IManejadorArticulos manejadorArticulos;

        accion accionArticulos;

        public MainWindow()
        {
            InitializeComponent();

            manejadorArticulos = new ManejadorArticulos(new RepositorioDeArticulos());

            PonerBotonesArticulosEnEdicion(false);
            LimpiarCamposDeArticulos();
            ActualizarTablaArticulos();
            LlenarComboTipo();
        }

        private void LlenarComboTipo()
        {
            cbArticulosTamanio.Items.Add("Extra Chico");
            cbArticulosTamanio.Items.Add("Chico");
            cbArticulosTamanio.Items.Add("Mediano");
            cbArticulosTamanio.Items.Add("Grande");
            cbArticulosTamanio.Items.Add("Extra Grande");
        }

        private void ActualizarTablaArticulos()
        {
            dtgArticulos.ItemsSource = null;
            dtgArticulos.ItemsSource = manejadorArticulos.Listar;
        }

        private void LimpiarCamposDeArticulos()
        {
            txbArticulosId.Text = "";
            txbArticulosDescripcion.Clear();
            txbArticulosTipo.Clear();
            txbPedido.Clear();
            txbPrecioUnitario.Clear();
            txbArticulosUnidad.Clear();
            cbArticulosTamanio.Text="";
        }

        private void PonerBotonesArticulosEnEdicion(bool v)
        {
            btnArticulosCancelar.IsEnabled = v;
            btnArticulosEditar.IsEnabled = !v;
            btnArticulosEliminar.IsEnabled = !v;
            btnArticulosGuardar.IsEnabled = v;
            btnArticulosNuevo.IsEnabled = !v;
            txbArticulosDescripcion.IsEnabled = v;
            txbArticulosTipo.IsEnabled = v;
            txbArticulosUnidad.IsEnabled = v;
            txbPedido.IsEnabled = v;
            txbPrecioTotal.IsEnabled = v;
            txbPrecioUnitario.IsEnabled = v;
            cbArticulosTamanio.IsEnabled = v;
        }

        private void BtnArticulosNuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeArticulos();
            PonerBotonesArticulosEnEdicion(true);
            accionArticulos = accion.Nuevo;
        }

        private void BtnArticulosEliminar_Click(object sender, RoutedEventArgs e)
        {
            Articulo art = dtgArticulos.SelectedItem as Articulo;
            if (art != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar este Articulo?", "Inventarios",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manejadorArticulos.Eliminar(art.Id))
                    {
                        MessageBox.Show("Articulo eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        ActualizarTablaArticulos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el Articulo", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void BtnArticulosCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeArticulos();
            PonerBotonesArticulosEnEdicion(false);
        }

        private void BtnArticulosGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (accionArticulos == accion.Nuevo)
            {
                try
                {
                    Articulo emp = new Articulo()
                    {
                        Descripcion = txbArticulosDescripcion.Text,
                        Tipo = txbArticulosTipo.Text,
                        Pedido = txbPedido.Text,
                        Precio = Convert.ToDouble(txbPrecioUnitario.Text),
                        Unidad = txbArticulosUnidad.Text,
                        Tamanio = cbArticulosTamanio.SelectedItem.ToString()

                    };
                    if (manejadorArticulos.Agregar(emp))
                    {
                        MessageBox.Show("Articulo agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        LimpiarCamposDeArticulos();
                        ActualizarTablaArticulos();
                        PonerBotonesArticulosEnEdicion(false);
                    }
                    else
                    {
                        MessageBox.Show("El Articulo no se pudo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                } catch (FormatException)
                {
                    MessageBox.Show("El Articulo no se pudo agregar, uno de los datos no es correcto", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                Articulo art = dtgArticulos.SelectedItem as Articulo;
                art.Descripcion = txbArticulosDescripcion.Text;
                art.Tipo = txbArticulosTipo.Text;
                art.Pedido = txbPedido.Text;
                art.Precio = Convert.ToDouble(txbPrecioUnitario.Text);
                art.Unidad = txbArticulosUnidad.Text;
                art.Tamanio = cbArticulosTamanio.SelectedItem.ToString();
                if (manejadorArticulos.Modificar(art))
                {
                    MessageBox.Show("Articulo modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCamposDeArticulos();
                    ActualizarTablaArticulos();
                    PonerBotonesArticulosEnEdicion(false);
                }
                else
                {
                    MessageBox.Show("El Articulo no se pudo modificar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnArticulosEditar_Click(object sender, RoutedEventArgs e)
        {
            Articulo art = dtgArticulos.SelectedItem as Articulo;
            if (art != null)
            {
                txbArticulosId.Text = art.Id;
                txbArticulosDescripcion.Text = art.Descripcion;
                txbArticulosTipo.Text = art.Tipo;
                txbPedido.Text = art.Pedido;
                txbArticulosUnidad.Text = art.Unidad;
                cbArticulosTamanio.SelectedValue = art.Tamanio;
                txbPrecioUnitario.Text = art.Precio.ToString();
                accionArticulos = accion.Editar;
                PonerBotonesArticulosEnEdicion(true);
            }

        }
    }
}
