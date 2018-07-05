using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System{
    public static class StringExtensions{
        public static string QuebrarPalavrasEmUpperCases(this string str){
            var output = "";
            foreach (var letter in str){
                if (Char.IsUpper(letter) && output.Length > 0)
                    output += " " + letter;
                else
                    output += letter;
            }

            return output;
        }

    }
}
