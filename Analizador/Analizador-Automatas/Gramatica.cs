using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Analizador_Automatas
{
    class Gramatica:Grammar
    {
        public Gramatica():base(caseSensitive: true) {
            #region Expresiones Regulares
            NumberLiteral numero = TerminalFactory.CreateCSharpNumber("numero");
            IdentifierTerminal identificador = new IdentifierTerminal("id");
            var texto = new StringLiteral("texto", "\"", StringOptions.AllowsDoubledQuote);
            var caracter = new StringLiteral("caracter", "'", StringOptions.AllowsDoubledQuote);
            #endregion

            #region Comentarios
            CommentTerminal linea = new CommentTerminal("comentario", "//", "\n", "\r\n");
            CommentTerminal lineas = new CommentTerminal("comentarios", "</", "/>");
            NonGrammarTerminals.Add(linea);
            NonGrammarTerminals.Add(lineas);
            #endregion

            #region Palabras reservadas y Terminales
            var palabra_public = ToTerm("public");
            var palabra_class = ToTerm("class");
            var llave_cierre = ToTerm("}");
            var llave_abrir = ToTerm("{");
            var palabra_int = ToTerm("int");
            var palabra_char = ToTerm("char");
            var palabra_double = ToTerm("double");
            var palabra_float = ToTerm("float");
            var palabra_byte = ToTerm("byte");
            var palabra_String = ToTerm("String");
            var punto_coma = ToTerm(";");
            var palabra_private = ToTerm("private");
            var palabra_protected = ToTerm("protected");
            //main
            var palabra_void = ToTerm("void");
            var palabra_main = ToTerm("main");
            var palabra_args = ToTerm("args");
            var corchete_abrir = ToTerm("[");
            var corchete_cerrar = ToTerm("]");
            var palabra_static = ToTerm("static");
            var parentesis_abrir = ToTerm("(");
            var parentesis_cerrar = ToTerm(")");
            //for
            var palabra_for = ToTerm("for");
            var palabra_in = ToTerm("in");
            var palabra_new = ToTerm("new");
            var palabra_print = ToTerm("print");
            //if
            var signo_igual = ToTerm("=");
            var signo_mas = ToTerm("+");
            var signo_menos = ToTerm("-");
            var signo_por = ToTerm("*");
            var signo_div = ToTerm("/");
            var igual_igual = ToTerm("==");
            var mayor_igual = ToTerm(">=");
            var menor_igual = ToTerm("<=");
            var diferente = ToTerm("!=");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var palabra_if = ToTerm("if");
            var palabra_else = ToTerm("else");
            //while
            var palabra_while = ToTerm("while");
            #endregion

            #region NO TERMINALES
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal DECLARACION_VARIABLES = new NonTerminal("DECLARACIONES");
            NonTerminal CODIGO = new NonTerminal("NonTerminal");
            NonTerminal TIPO_CLASE = new NonTerminal("TIPO_CLASE");
            NonTerminal MAIN = new NonTerminal("MAIN");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal FOR = new NonTerminal("FOR");
            NonTerminal ARREGLOS = new NonTerminal("ARREGLO");
            NonTerminal CONDICIONAL = new NonTerminal("CONDICIONAL");
            NonTerminal CONDICIONAL_IF = new NonTerminal("IF");
            NonTerminal PRINT = new NonTerminal("PRINT");
            NonTerminal WHILE = new NonTerminal("WHILE");
            NonTerminal DECLARACION_NORMAL = new NonTerminal("NORMAL");
            #endregion NO TERMINALES

            #region GRAMATICA
            this.Root = INICIO;
            //INICIO
            INICIO.Rule = TIPO_CLASE + palabra_class + identificador + llave_abrir + MAIN + llave_cierre;
            INICIO.ErrorRule = SyntaxError + ToTerm(";");
            //Tipo de clase
            TIPO_CLASE.Rule = Empty
                | palabra_public
                | palabra_private
                | palabra_protected;
            //MAIN
            MAIN.Rule = palabra_public + palabra_static + palabra_void + palabra_main + parentesis_abrir + palabra_String + corchete_abrir + corchete_cerrar + palabra_args + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre;
            MAIN.ErrorRule = SyntaxError + punto_coma;
            //Declaracion de variables
            DECLARACION_VARIABLES.Rule = TIPO + identificador;
            DECLARACION_NORMAL.Rule = DECLARACION_VARIABLES + punto_coma;

            //tipo de variables
            TIPO.Rule = palabra_int
                | palabra_float
                | palabra_byte
                | palabra_char
                | palabra_float
                | palabra_String;
            //Arreglos
            ARREGLOS.Rule = TIPO + identificador + corchete_abrir + corchete_cerrar + ToTerm("=") + palabra_new + TIPO + corchete_abrir + numero + corchete_cerrar + punto_coma
                | TIPO + corchete_abrir + corchete_cerrar + identificador + ToTerm("=") + palabra_new + TIPO + corchete_abrir + numero + corchete_cerrar + punto_coma
                | TIPO + corchete_abrir + corchete_cerrar + identificador + punto_coma
                | TIPO + identificador + corchete_abrir + corchete_cerrar + punto_coma
                | identificador + corchete_abrir + corchete_cerrar + ToTerm("=") + palabra_new + TIPO + corchete_abrir + numero + corchete_cerrar;
            ARREGLOS.ErrorRule = SyntaxError + punto_coma;
            //Ciclo for
            FOR.Rule = palabra_for + identificador + palabra_in + identificador + llave_abrir + CODIGO + llave_cierre
                | palabra_for + identificador + palabra_in + numero + llave_abrir + CODIGO + llave_cierre;
            FOR.ErrorRule = SyntaxError + punto_coma;
            //Codigo
            CODIGO.Rule = Empty
                | CODIGO + DECLARACION_NORMAL | DECLARACION_NORMAL
                | CODIGO + FOR | FOR
                | CODIGO + ARREGLOS | ARREGLOS
                | CODIGO + CONDICIONAL_IF | CONDICIONAL_IF
                | CODIGO + PRINT | PRINT
                | CODIGO + WHILE | WHILE;
            CODIGO.ErrorRule = SyntaxError + punto_coma;
            //condicionales
            CONDICIONAL.Rule = igual_igual
                | mayor_igual
                | menor_igual
                | diferente
                | mayor
                | menor;
            //if
            CONDICIONAL_IF.Rule = palabra_if + parentesis_abrir + numero + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre + palabra_else + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + numero + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre + llave_cierre + palabra_else + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre + llave_cierre + palabra_else + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre + palabra_else + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + numero + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + llave_cierre + palabra_else + llave_abrir + llave_cierre
                | palabra_if + parentesis_abrir + numero + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + llave_cierre + llave_cierre + palabra_else + llave_abrir + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + llave_cierre + llave_cierre + palabra_else + llave_abrir + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + llave_cierre + palabra_else + llave_abrir + llave_cierre
        /**/    | palabra_if + parentesis_abrir + numero + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + numero + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre
                | palabra_if + parentesis_abrir + numero + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + llave_cierre
                | palabra_if + parentesis_abrir + numero + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + llave_cierre
                | palabra_if + parentesis_abrir + identificador + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + llave_cierre;
            CONDICIONAL_IF.ErrorRule = SyntaxError + punto_coma;
            //print
            PRINT.Rule = palabra_print + parentesis_abrir + identificador + parentesis_cerrar + punto_coma;
            PRINT.ErrorRule = SyntaxError + punto_coma;
            //while
            WHILE.Rule = palabra_while + parentesis_abrir + numero + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre
                | palabra_while + parentesis_abrir + identificador + CONDICIONAL + numero + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre
                | palabra_while + parentesis_abrir + numero + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre
                | palabra_while + parentesis_abrir + identificador + CONDICIONAL + identificador + parentesis_cerrar + llave_abrir + CODIGO + llave_cierre;

            #endregion

        }
    }
}
