using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    class Task09
    {
        public  string[] Solution(string str)
        {
            if (str == null)
                return null;

            string[] arr = null;

            if (str.Length%2 != 0)
            {
                str += "_";
            }
            arr = new string[str.Length/2];
            str.ToCharArray();

            int index = 0;
            for (int i = 0; i < str.Length; i++)
            {
                arr[index] = str[i].ToString() + str[i + 1].ToString();
                i++;
                index++;
            }

            return arr;

        }
    }
}
