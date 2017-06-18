using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Noob_level
{
    //which takes in numbers num1 and num2 and returns 1 if there is a straight triple of a number at any place in num1 and also a straight double of the same number in num2.
    class Task03
    {
      public  int TripleDouble(long num1, long num2)
        {
            if(num1<1000 || num2<100)
            return 0;
           var str1 = num1.ToString().ToCharArray();
           var str2 = num2.ToString().ToCharArray();
           List<string> StrList1 = new List<string>();

            for (int i = 0; i < str1.Length; i++)
            {
                int numer1 = int.Parse(str1[i].ToString());

                if (i+2<str1.Length)
                {
                    if (numer1 == int.Parse(str1[i + 1].ToString()) && numer1 == int.Parse(str1[i + 2].ToString()))
                    {
                        StrList1.Add(String.Format("{0}{0}{0}",numer1.ToString()));
                        i += 2;
                    }
                }

            }
                 

            for (int i = 0; i < str2.Length; i++)
            {
                int numer2 = int.Parse(str2[i].ToString());

                if (i + 1 < str2.Length)
                {
                    if (numer2 == int.Parse(str2[i + 1].ToString()) && StrList1.Any(X=>X.Contains(numer2.ToString())))
                    {
                        return 1;

                    }
                }

            }

          return 0;

        }
    }
}
