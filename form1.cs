using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Reservas_de_Cine
{
    public partial class frmReservas : Form
    {
        private Dictionary<string, List<string>> peliculasPorCategoria;

        public frmReservas()
        {
            InitializeComponent();
            InicializarDatos();
        }

        private void InicializarDatos()
        {
            peliculasPorCategoria = new Dictionary<string, List<string>>()
            {
                { "Acción", new List<string> { "John Wick", "Mad Max", "The Dark Knight" } },
                { "Comedia", new List<string> { "Superbad", "The Hangover", "Bridesmaids" } },
                { "Drama", new List<string> { "The Shawshank Redemption", "Forrest Gump", "The Godfather" } },
                { "Ciencia Ficción", new List<string> { "Inception", "Interstellar", "The Matrix" } }
            };
        }

        // EVENTO LOAD - Se ejecuta al cargar el formulario
        private void frmReservas_Load(object sender, EventArgs e)
        {
            // Configurar columnas del DataGridView
            ConfigurarDataGridView();

            // Limpiar controles y cargar datos iniciales
            LimpiarControles();
            CargarCategorias();

            // Deshabilitar botón agregar inicialmente
            btnAgregar.Enabled = false;
        }

        private void ConfigurarDataGridView()
        {
            dgvReservas.Columns.Clear();
            dgvReservas.Columns.Add("Nombre", "Nombre");
            dgvReservas.Columns.Add("DUI", "DUI");
            dgvReservas.Columns.Add("Categoria", "Categoría");
            dgvReservas.Columns.Add("Pelicula", "Película");

            // Ajustar ancho de columnas
            dgvReservas.Columns["Nombre"].Width = 120;
            dgvReservas.Columns["DUI"].Width = 100;
            dgvReservas.Columns["Categoria"].Width = 100;
            dgvReservas.Columns["Pelicula"].Width = 150;
        }

        private void LimpiarControles()
        {
            txtNombre.Clear();
            txtNombre.Text = ""; // Quitar el "(vacío)" del diseñador
            mtxtDUI.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbPelicula.SelectedIndex = -1;
            cmbPelicula.Items.Clear();
        }

        private void CargarCategorias()
        {
            cmbCategoria.Items.Clear();
            foreach (var categoria in peliculasPorCategoria.Keys)
            {
                cmbCategoria.Items.Add(categoria);
            }
        }

        // EVENTO SelectedIndexChanged del ComboBox Categoría
        private void cmbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategoria.SelectedItem != null)
            {
                string categoriaSeleccionada = cmbCategoria.SelectedItem.ToString();
                CargarPeliculas(categoriaSeleccionada);
            }
            else
            {
                cmbPelicula.Items.Clear();
                cmbPelicula.SelectedIndex = -1;
            }
            ValidarCampos();
        }

        private void CargarPeliculas(string categoria)
        {
            cmbPelicula.Items.Clear();
            cmbPelicula.SelectedIndex = -1;

            if (peliculasPorCategoria.ContainsKey(categoria))
            {
                foreach (var pelicula in peliculasPorCategoria[categoria])
                {
                    cmbPelicula.Items.Add(pelicula);
                }
            }
        }

        // EVENTO KeyPress para validar solo letras en el nombre
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo letras, espacios y teclas de control (backspace, delete, etc.)
            if (!char.IsLetter(e.KeyChar) &&
                !char.IsControl(e.KeyChar) &&
                !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Bloquear el carácter
            }
        }

        // EVENTO KeyPress para validar solo números en DUI 
        // El MaskedTextBox ya maneja esto, pero se puede agregar validación extra
        private void mtxtDUI_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y teclas de control
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // EVENTO Click del botón Agregar
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validar campos antes de agregar
            if (ValidarCamposCompletos())
            {
                AgregarReserva();
                LimpiarControles();
                MessageBox.Show("Reserva agregada correctamente!", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidarCamposCompletos()
        {
            // Validar nombre no vacío y solo letras
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                return false;

            // Validar DUI completo (sin guiones bajos)
            if (mtxtDUI.Text.Contains("_") || string.IsNullOrWhiteSpace(mtxtDUI.Text))
                return false;

            // Validar que se haya seleccionado una categoría
            if (cmbCategoria.SelectedIndex == -1)
                return false;

            // Validar que se haya seleccionado una película
            if (cmbPelicula.SelectedIndex == -1)
                return false;

            return true;
        }

        private void AgregarReserva()
        {
            dgvReservas.Rows.Add(
                txtNombre.Text.Trim(),
                mtxtDUI.Text,
                cmbCategoria.SelectedItem.ToString(),
                cmbPelicula.SelectedItem.ToString()
            );
        }

        // BONUS: Validación en tiempo real para habilitar/deshabilitar botón
        private void ValidarCampos()
        {
            bool nombreValido = !string.IsNullOrWhiteSpace(txtNombre.Text);
            bool duiCompleto = !mtxtDUI.Text.Contains("_") && !string.IsNullOrWhiteSpace(mtxtDUI.Text);
            bool categoriaSeleccionada = cmbCategoria.SelectedIndex != -1;
            bool peliculaSeleccionada = cmbPelicula.SelectedIndex != -1;

            btnAgregar.Enabled = nombreValido && duiCompleto && categoriaSeleccionada && peliculaSeleccionada;
        }

        // Eventos para validación en tiempo real
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void mtxtDUI_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void cmbPelicula_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        // AUTOEVALUACIÓN
        // NOTA: 10 - Anderson Saul Maravilla Callejas
    }
}
