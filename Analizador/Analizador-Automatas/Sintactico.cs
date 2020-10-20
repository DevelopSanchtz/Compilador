using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Analizador_Automatas
{
    class Sintactico
    {
        public static string errores = "";
        public static bool ANALISIS_SINTACTICO(string codigo)
        {
            Gramatica gram = new Gramatica();
            LanguageData lenguaje = new LanguageData(gram);
            Parser parse = new Parser(lenguaje);
            ParseTree arbol = parse.Parse(codigo);
            ParseTreeNode raiz = arbol.Root;
            //dispArbol(raiz);
            if (arbol.Root == null)
            {
                foreach (var error in arbol.ParserMessages)
                {
                    errores += "Error: " + error.Message + " en linea: " + (error.Location.Line ) + "\r\n";
                }
                return false;
            }
            else
            {
                return true;
            }
            
        }
        
    }
}
