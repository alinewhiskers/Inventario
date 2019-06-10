using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using System;
using System.Collections.Generic;
using System.Windows;

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

        private const double IVA = 1.16;
        IManejadorArticulos manejadorArticulos;
        List<Articulo> carrito = new List<Articulo>();
        accion accionArticulos;

        public MainWindow()
        {
            InitializeComponent();
            inicial();
            manejadorArticulos = new ManejadorArticulos(new RepositorioDeArticulos());
            PonerBotonesArticulosEnEdicion(false);
            LimpiarCamposDeArticulos();
            ActualizarTablaArticulos();
            LlenarComboTipo();
        }

        private void inicial()
        {
            btnArticulosComprar.IsEnabled = false;
            btnArticulosVaciar.IsEnabled = false;
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
            cbArticulosTamanio.Text = "";
            txbPrecioTotal.Text = "";
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
            guardarArticulo();
        }

        private void guardarArticulo()
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
                }
                catch (FormatException)
                {
                    MessageBox.Show("El Articulo no se pudo agregar, uno de los datos no es correcto", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                try
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
                } catch (NullReferenceException e){ Console.WriteLine(e.Message); }
                
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
                txbPrecioTotal.Text = art.PrecioTotal.ToString("C");
                accionArticulos = accion.Editar;
                PonerBotonesArticulosEnEdicion(true);
            }

        }

        private void CalculaPrecio(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string llave =e.Key.ToString();
            if (llave.Equals("Return"))
            {
                guardarArticulo();
            }
            else
            {
                try
                {
                    txbPrecioTotal.Text = (Convert.ToDouble(txbPrecioUnitario.Text) * IVA).ToString("C");
                }
                catch (Exception)
                {
                    txbPrecioTotal.Text = "No se puede calcular";
                }
            }
            
        }

        private void BotonCarrito(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (dtgArticulos.SelectedItem != null)
            {
                btnArticulosComprar.IsEnabled = true;
            }
            else
            {
                btnArticulosComprar.IsEnabled = false;
            }
            if (carrito.Count == 0)
            {
                btnArticulosVaciar.IsEnabled = false;
            }
            else
            {
                btnArticulosVaciar.IsEnabled = true;
            }
        }

        private void BtnArticulosComprar_Click(object sender, RoutedEventArgs e)
        {
            double total = 0;
            string texto;
            Articulo art = dtgArticulos.SelectedItem as Articulo;

            if (art != null)
            {
                string id_art = art.Id;
                string descripcion_art = art.Descripcion;
                string tipo_art = art.Tipo;
                string pedido_art = art.Pedido;
                string unidad_art = art.Unidad;
                string tamanio_art = art.Tamanio;
                double precio_art = art.Precio;
                double precioTotal_art = art.PrecioTotal;
            }
            carrito.Insert(carrito.Count, art);
            if (carrito.Count == 1)
            {
                texto = " producto ";
            }
            else
            {
                texto = " productos ";
            }
            foreach (var dato in carrito)
            {
                total += dato.PrecioTotal;
            }
            MessageBox.Show("¡El artículo se agregó a la lista!\n\nSe agregó " + carrito.Count + texto + "con total de: " + total.ToString("C"), "Carrito Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        private void BtnArticulosVaciar_Click(object sender, RoutedEventArgs e)
        {
            string texto;
            if (carrito.Count == 1)
            {
                texto = " producto ";
            }
            else
            {
                texto = " productos ";
            }
            carrito.RemoveRange(0, carrito.Count);
            MessageBox.Show(carrito.Count + texto + " se quitaron del carrito", "Carrito Actualizado", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }


    }
}
