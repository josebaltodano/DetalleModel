using AppCore.IServices;
using Domain.Entities;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practicaDepreciacion
{
    public partial class Form1 : Form
    {
        IActivoServices activoServices;
        Empleado empleado;
        private int SelecionarId;
     
        public Form1(IActivoServices ActivoServices /*Empleado*/ /*empleado*/)
        {
            //this.empleado = empleado;
            this.activoServices = ActivoServices;
            InitializeComponent();
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede numeros");
            }
        }

    

        private void txtValor_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtValorR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtVidaU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("No se puede LETRAS");
            }
        }

        private void txtEnviar_Click(object sender, EventArgs e)
        {
            bool verificado = verificar();
            if (verificado == false)
            {
                MessageBox.Show("Tienes que llenar todos los formularios.");
            }
            else
            {
                if (empleado==null)
                {
                    throw new ArgumentException("si");
                }
                Activo activo = new Activo()
                {
                    Nombre = txtNombre.Text,
                    Valor = double.Parse(txtValor.Text),
                    ValorResidual=double.Parse(txtValorR.Text),
                    VidaUtil= int.Parse(txtVidaU.Text),
                    Descripcion=textBox1.Text,
                    Empleado=empleado
                };
                activoServices.Add(activo);
                dataGridView1.DataSource = null;
                limpiar();
                dataGridView1.DataSource = activoServices.Read();

            }
        }
        private bool verificar()
        {
            if (String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(txtValor.Text) || String.IsNullOrEmpty(txtVidaU.Text) || String.IsNullOrEmpty(txtValorR.Text))
            {
              
                return false;
            }
            return true;
        }
        private void limpiar()
        {
            this.txtNombre.Text = String.Empty;
            this.txtValor.Text = "0.00";
            this.txtValorR.Text = "0.00";
            this.txtVidaU.Value = 0;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FrmDepreciacion depreciacion = new FrmDepreciacion(activoServices.Read()[e.RowIndex]);
                depreciacion.ShowDialog();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = activoServices.Read();
           // dataGridView1.DataSource = empleado;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelecionarId = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            MessageBox.Show(SelecionarId.ToString());
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FrmDepreciacion depreciacion = new FrmDepreciacion(activoServices.Read()[e.RowIndex]);
                depreciacion.ShowDialog();
            }
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            try
            {
                bool verficar = verificar();
                if (verficar == false)
                {
                    MessageBox.Show("Tienes que llenar todo los cuadros del formulario");

                }
                else
                {
                    Activo activo = new Activo()
                    {
                        Nombre = txtNombre.Text,
                        Valor = double.Parse(txtValor.Text),
                        ValorResidual = double.Parse(txtValorR.Text),
                        VidaUtil = int.Parse(txtVidaU.Text),
                        Id = (int)numericUpDown1.Value
                    };
                    activoServices.Update(activo);
                    dataGridView1.DataSource = null;
                    limpiar();
                    dataGridView1.DataSource = activoServices.Read();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            Activo activo = new Activo()
            {
                Id = (int)numericUpDown1.Value
            };
            numericUpDown1.Value = 0;
            activoServices.Delete(activo);
            dataGridView1.DataSource = null;
            limpiar();
            dataGridView1.DataSource = activoServices.Read();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
