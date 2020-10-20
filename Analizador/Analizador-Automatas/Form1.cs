using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Irony.Parsing;

namespace Analizador_Automatas
{
    public partial class Form1 : Form
    {
        int salto_linea;
        string[] palabras_reservadas = { "public", "static", "void", "main", "String", "args", "int", "double", "float", "char", "byte", "new", "private", "protected", "del", "for", "in", "raise", "assert", "if", "else", "from", "lambda", "return", "try", "class", "except", "while", "continue", "exec", "def", "print" };
        //Espresiones regulares
        string expresion_identificador;
        string expresion_numero;
        string expresion_operadores;
        string expresion_delimitadores;
        string expresion_comparadores;
        string expresion_mensaje = "[a-zA-Z]";
        char comillas = '"';
        string concatenar;
        public Form1()
        {
            InitializeComponent();
            salto_linea = 1;
            expresion_identificador = "[a-zA-Z]_[a-zA-Z]|[a-zA-Z]";
            expresion_numero = "[0-9]";
            expresion_operadores = "[+ | - | * | / | // | %]";
            expresion_comparadores = "[< | > | <= | >= | == | !=]";
            expresion_delimitadores = "[(|)|[|]|;|''|{|}|]";
            concatenar = "";
        }
        
        public void analisisLexico(string codigo)
        {
            salto_linea = 1;
            codigo = areaCodigo.Text+"\n";
            char[] arreglo = codigo.ToArray();
            for (int i = 0; i < arreglo.Length; i++)
            {
                if (arreglo[i].Equals(' ')
                    | arreglo[i].Equals('\n')
                    | arreglo[i].Equals('(')
                    | arreglo[i].Equals(')')
                    | arreglo[i].Equals('"'))
                {
                    for (int j = 0; j < palabras_reservadas.Length; j++)
                    {
                        if (palabras_reservadas[j].Equals(concatenar))
                        {
                            tabla.Rows.Add(concatenar, "Palabra Reservada", salto_linea);
                            concatenar = "";
                        }
                    }
                    if (Regex.IsMatch(arreglo[i].ToString(), " "))
                    {

                    }
                    else if (Regex.IsMatch(arreglo[i].ToString(), expresion_delimitadores))
                    {
                        tabla.Rows.Add(arreglo[i], "Delimitador", salto_linea);
                        concatenar = "";
                    }
                    else if (Regex.IsMatch(concatenar, expresion_identificador))
                    {
                        tabla.Rows.Add(concatenar, "Identificador", salto_linea);
                        concatenar = "";
                    }
                    else if (Regex.IsMatch(concatenar, expresion_numero))
                    {
                        tabla.Rows.Add(concatenar, "Número", salto_linea);
                        concatenar = "";
                    }
                    else if (Regex.IsMatch(concatenar, expresion_operadores))
                    {
                        tabla.Rows.Add(concatenar, "Operador", salto_linea);
                        concatenar = "";
                    }
                    else if (Regex.IsMatch(concatenar, expresion_comparadores))
                    {
                        tabla.Rows.Add(concatenar, "Comparador", salto_linea);
                        concatenar = "";
                    }
                    else if (Regex.IsMatch(concatenar, expresion_delimitadores))
                    {
                        tabla.Rows.Add(concatenar, "Delimitador", salto_linea);
                        concatenar = "";
                    }
                    else if (Regex.IsMatch(arreglo[i].ToString(), '"'.ToString()))
                    {
                        tabla.Rows.Add(arreglo[i], "Delimitador", salto_linea);
                        //concatenar = "";
                    }
                    else
                    {
                        tabla.Rows.Add(concatenar, "Invalida", salto_linea);
                        concatenar = "";
                    }
                }
                else
                {
                    concatenar = concatenar + arreglo[i];
                }
                if (arreglo[i].Equals('\n'))
                {
                    salto_linea++;
                }
            }
            concatenar = "";
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            analisisLexico(areaCodigo.Text);
            if (Sintactico.ANALISIS_SINTACTICO(areaCodigo.Text) == false)
            {
                //areaResultado.AppendText(Sintactico.errores);
                MessageBox.Show(Sintactico.errores);
            }
            else
            {
                //areaResultado.AppendText("Analisis Correcto\n");
                MessageBox.Show("Analisis Correcto");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader leer = new StreamReader(open.FileName);
                    string linea;
                    linea = leer.ReadLine();
                    while (linea != null)
                    {
                        areaCodigo.AppendText(linea + '\n');
                        linea = leer.ReadLine();
                    }
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("" + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabla.Rows.Clear();
        }
    }
}
