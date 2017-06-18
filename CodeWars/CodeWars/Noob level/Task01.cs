using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
//    Implement a method that accepts 3 integer values a, b, c.The method should return true if a triangle can be built with the sides of given length and false in any other case.
//(In this case, all triangles must have surface greater than 0 to be accepted).
    class Task01
    {
        public bool Check(int a, int b, int c)
        {
            if (a > 0 && b > 0 && c > 0) { return true;}
            return false;
        }
    }
}
