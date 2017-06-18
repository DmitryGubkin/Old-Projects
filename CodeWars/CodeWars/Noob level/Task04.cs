using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
//    x Simple, given a string of words, return the length of the shortest word(s).

//String will never be empty and you do not need to account for different data types.
    class Task04
    {
        public  int FindShort(string s)
        {
            return s.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).Min(X=>X.Length);
           
        }
    }
}
