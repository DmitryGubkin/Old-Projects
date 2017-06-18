using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    //In this little assignment you are given a string of space separated numbers, and have to return the highest and lowest number.
    class Task02
    {
        public string GetNum(string str)
        {
    
            if (str.Length < 1)
            { return "throw towel"; }

            int n;
            int min=0, max=0;
            bool haveNumbs =false;
            string[] numbers = str.Split(default(string[]),StringSplitOptions.RemoveEmptyEntries);

            foreach (var thisstr in numbers)
            {
                if (int.TryParse(thisstr,out n))
                {
                    min = max = n;
                    haveNumbs = true;
                    break;
                }
            }

            if (haveNumbs == false)
                return "throw towel";


            foreach (var thisstr in numbers)
            {
                if (int.TryParse(thisstr, out n))
                {
                    if (n > max) max = n;
                    if (n < min) min = n;
                }
            }

            return $"{max} {min}";
        }
    }
}
