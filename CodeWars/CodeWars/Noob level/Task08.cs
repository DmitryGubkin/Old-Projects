using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    internal class Task08
    {
        public string ReverseWords(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            var s = str.Split(new char[] {' '}, StringSplitOptions.None).ToList();
            for (int i = 1; i < s.Count; i++)
            {
               
                s[i] += " ";
                
            }
            s.Reverse();
            
            return string.Concat(s);

        }
    }
}
