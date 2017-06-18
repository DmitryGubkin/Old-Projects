using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    class Task06
    {
//        Your task is to write a function which returns the sum of following series upto nth term(parameter).
//Series: 1 + 1/4 + 1/7 + 1/10 + 1/13 + 1/16 +...
//Rules:
//You need to round the answer upto 2 decimal places and return it as String.
//If the given value is 0 then it should return 0.00
//You will only be given Natural Numbers as arguments.
        public  string seriesSum(int n)
        {
            decimal res = 0.00m;
            decimal div = 4.00m;
            
            if (n <= 0)
                return res.ToString();
            if (n == 1)
                return "1.00";
            res =1.00m/div;

            while (n!=2)
            {
                div += 3.00m;
                res = res +(1 / div);
                n--;
            }
            res += 1.00m;
            return Math.Round(res, 2).ToString();

        }
    }
}
