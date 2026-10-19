using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Reservación_de_cine
{
    public partial class Peliculas : Form
    {
        // ARRAYLIST: almacena las películas
        ArrayList peliculas = new ArrayList();

        // ARREGLO BIDIMENSIONAL: organiza los 20 botones
        Button[,] asientos = new Button[4, 5];

        Pelicula actual = null;

        // LISTA SIMPLEMENTE ENLAZADA: almacena las reservas
        NodoReserva inicio = null;
        int fila = -1, columna = -1;

        public Peliculas()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            peliculas.Add(new Pelicula("Rapidos y furiosos"));
            peliculas.Add(new Pelicula("Spider-Man"));
            peliculas.Add(new Pelicula("Batman"));

            asientos = new Button[,]
            {
                { btnA1, btnA2, btnA3, btnA4, btnA5 },
                { btnB1, btnB2, btnBB3, btnB4, btnB5 },
                { btnC1, btnC2, btnC3, btnC4, btnC5 },
                { btnD1, btnD2, btnD3, btnD4, btnD5 }
            };

            for (int f = 0; f < 4; f++)
                for (int c = 0; c < 5; c++)
                {
                    asientos[f, c].Tag = f + "," + c;
                    asientos[f, c].BackColor = Color.DarkGreen;
                    asientos[f, c].Click += Asiento_Click;
                }

            ActualizarPeliculas();
        }

        private bool ValidarTexto(TextBox campo, string mensaje)
        {
            if (campo.Text.Trim() == "")
            {
                MessageBox.Show(mensaje);
                campo.Focus();
                return false;
            }
            return true;
        }

        private void ActualizarPeliculas()
        {
            cmbPelicula.Items.Clear();
            foreach (Pelicula p in peliculas)
                cmbPelicula.Items.Add(p.Nombre);
        }

        private Pelicula BuscarPelicula(string nombre)
        {
            foreach (Pelicula p in peliculas)
                if (string.Equals(p.Nombre, nombre,
                    StringComparison.OrdinalIgnoreCase))
                    return p;

            return null;
        }

        private void cmbPelicula_SelectedIndexChanged(object sender, EventArgs e)
        {
            actual = BuscarPelicula(cmbPelicula.Text);
            fila = columna = -1;
            MostrarAsientos();
        }

        private void MostrarAsientos()
        {
            for (int f = 0; f < 4; f++)
                for (int c = 0; c < 5; c++)
                {
                    if (actual == null)
                        asientos[f, c].BackColor = Color.LightGray;
                    else if (actual.Asientos[f, c] == 1)
                        asientos[f, c].BackColor = Color.DarkRed;
                    else
                        asientos[f, c].BackColor = Color.DarkGreen;
                }

            if (actual != null && fila != -1)
                asientos[fila, columna].BackColor = Color.Yellow;
        }

        private void Asiento_Click(object sender, EventArgs e)
        {
            if (actual == null)
            {
                MessageBox.Show("Seleccione una película");
                return;
            }

            Button boton = (Button)sender;
            string[] posicion = boton.Tag.ToString().Split(',');
            int f = int.Parse(posicion[0]);
            int c = int.Parse(posicion[1]);

            if (actual.Asientos[f, c] == 1)
            {
                MessageBox.Show("Asiento reservado");
                return;
            }

            fila = f;
            columna = c;
            MostrarAsientos();
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            if (!ValidarTexto(txtCliente, "Debe escribir el nombre del cliente"))
                return;

            if (actual == null)
            {
                MessageBox.Show("Debe seleccionar una película");
                return;
            }

            if (fila == -1)
            {
                MessageBox.Show("Debe seleccionar un asiento");
                return;
            }

            Reserva r = new Reserva(actual.Nombre, txtCliente.Text.Trim(),
                asientos[fila, columna].Text, fila, columna);

            actual.Asientos[fila, columna] = 1;

            NodoReserva nuevo = new NodoReserva(r);
            nuevo.Siguiente = inicio;
            inicio = nuevo;

            MostrarReservas();
            txtCliente.Clear();
            fila = columna = -1;
            MostrarAsientos();
            MessageBox.Show("Reserva realizada correctamente");
        }

        private void MostrarReservas()
        {
            lstReservas.Items.Clear();

            for (NodoReserva n = inicio; n != null; n = n.Siguiente)
                lstReservas.Items.Add(n.Datos);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (lstReservas.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una reserva");
                return;
            }

            Reserva r = (Reserva)lstReservas.SelectedItem;
            NodoReserva n = inicio, anterior = null;

            while (n != null && n.Datos != r)
            {
                anterior = n;
                n = n.Siguiente;
            }

            if (n == null) return;

            if (anterior == null)
                inicio = n.Siguiente;
            else
                anterior.Siguiente = n.Siguiente;

            Pelicula p = BuscarPelicula(r.Pelicula);
            if (p != null) p.Asientos[r.Fila, r.Columna] = 0;

            fila = columna = -1;
            MostrarReservas();
            MostrarAsientos();
            MessageBox.Show("Reserva cancelada");
        }

        private void btnAgregarPelicula_Click(object sender, EventArgs e)
        {
            if (!ValidarTexto(txtDato, "Debe escribir el nombre de la película"))
                return;

            string nombre = txtDato.Text.Trim();

            if (BuscarPelicula(nombre) != null)
            {
                MessageBox.Show("Esa película ya existe");
                txtDato.SelectAll();
                txtDato.Focus();
                return;
            }

            peliculas.Add(new Pelicula(nombre));
            ActualizarPeliculas();
            cmbPelicula.SelectedItem = nombre;
            txtDato.Clear();
            MessageBox.Show("Película agregada correctamente");
        }

        private void btnModificarPelicula_Click(object sender, EventArgs e)
        {
            Pelicula p = BuscarPelicula(cmbPelicula.Text);

            if (p == null)
            {
                MessageBox.Show("Seleccione una película");
                return;
            }

            if (!ValidarTexto(txtDato, "Escriba el nuevo nombre de la película"))
                return;

            string nombre = txtDato.Text.Trim();
            Pelicula repetida = BuscarPelicula(nombre);

            if (repetida != null && repetida != p)
            {
                MessageBox.Show("Esa película ya existe");
                return;
            }

            string anterior = p.Nombre;
            p.Nombre = nombre;

            for (NodoReserva n = inicio; n != null; n = n.Siguiente)
                if (n.Datos.Pelicula == anterior)
                    n.Datos.Pelicula = nombre;

            ActualizarPeliculas();
            cmbPelicula.SelectedItem = nombre;
            MostrarReservas();
            txtDato.Clear();
            MessageBox.Show("Película modificada correctamente");
        }

        private void btnEliminarPelicula_Click(object sender, EventArgs e)
        {
            Pelicula p = BuscarPelicula(cmbPelicula.Text);

            if (p == null)
            {
                MessageBox.Show("Seleccione una película");
                return;
            }

            for (NodoReserva n = inicio; n != null; n = n.Siguiente)
                if (n.Datos.Pelicula == p.Nombre)
                {
                    MessageBox.Show("La película tiene reservas activas");
                    return;
                }

            peliculas.Remove(p);
            actual = null;
            fila = columna = -1;
            ActualizarPeliculas();
            cmbPelicula.SelectedIndex = -1;
            MostrarAsientos();
            MessageBox.Show("Película eliminada correctamente");
        }

        private void btnBuscarReserva_Click(object sender, EventArgs e)
        {
            if (!ValidarTexto(txtDato, "Escriba el nombre del cliente"))
                return;

            string cliente = txtDato.Text.Trim();
            bool encontrado = false;

            for (NodoReserva n = inicio; n != null; n = n.Siguiente)
                if (n.Datos.Cliente.IndexOf(cliente,
                    StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    MessageBox.Show(n.Datos.ToString());
                    encontrado = true;
                }

            if (!encontrado) MessageBox.Show("Reserva no encontrada");
        }

        private void btnModificarReserva_Click(object sender, EventArgs e)
        {
            if (lstReservas.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una reserva");
                return;
            }

            if (!ValidarTexto(txtDato, "Escriba el nuevo nombre del cliente"))
                return;

            Reserva r = (Reserva)lstReservas.SelectedItem;
            r.Cliente = txtDato.Text.Trim();
            MostrarReservas();
            txtDato.Clear();
            MessageBox.Show("Reserva modificada correctamente");
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            if (inicio == null)
            {
                MessageBox.Show("No hay reservas para ordenar");
                return;
            }

            for (NodoReserva n = inicio; n != null; n = n.Siguiente)
                for (NodoReserva s = n.Siguiente; s != null; s = s.Siguiente)
                    if (string.Compare(n.Datos.Cliente, s.Datos.Cliente,
                        StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        Reserva temporal = n.Datos;
                        n.Datos = s.Datos;
                        s.Datos = temporal;
                    }

            MostrarReservas();
            MessageBox.Show("Reservas ordenadas por cliente");
        }
    }

    public class Pelicula
    {
        public string Nombre;

        // ARREGLO BIDIMENSIONAL: 4 filas y 5 columnas
        public int[,] Asientos = new int[4, 5];

        public Pelicula(string nombre)
        {
            Nombre = nombre;
        }
    }

    public class Reserva
    {
        public string Pelicula, Cliente, Asiento;
        public int Fila, Columna;

        public Reserva(string p, string c, string a, int f, int col)
        {
            Pelicula = p;
            Cliente = c;
            Asiento = a;
            Fila = f;
            Columna = col;
        }

        public override string ToString()
        {
            return Pelicula + " - " + Asiento + " - " + Cliente;
        }
    }

    // LISTA SIMPLEMENTE ENLAZADA: cada nodo apunta al siguiente
    public class NodoReserva
    {
        public Reserva Datos;
        public NodoReserva Siguiente;

        public NodoReserva(Reserva datos)
        {
            Datos = datos;
            Siguiente = null;
        }
    }
}